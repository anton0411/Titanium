using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Titanium.Arena;
using Titanium.Entities;

namespace Titanium
{
    /// <summary>
    /// A list of possible arena tiles.
    /// </summary>
    public enum ArenaTiles
    {
        EMPTY,
        CROSS,
        STR_HOR,
        STR_VERT,
        DE_TOP,
        DE_BOTTOM,
        DE_LEFT,
        DE_RIGHT,
        CORNER_TL,
        CORNER_TR,
        CORNER_BL,
        CORNER_BR,
        TRI_UP,
        TRI_DOWN,
        TRI_LEFT,
        TRI_RIGHT
    }

    /// <summary>
    /// The possible difficulties for the arena.
    /// </summary>
    public enum ArenaDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    /// <summary>
    /// This class provides static functions to construct an arena.
    /// </summary>
    public class ArenaBuilder
    {
        public static ArenaBuilder instance;

        private static int EDGE_START_BUFFER = 2;
        private static int MAX_TRIES = 50;

        private Tile[,] tiles;
        private Tile startTile;

        private int width;
        private int height;
        private ContentManager Content;
        private ArenaDifficulty difficulty;
        private Random r = new Random();
        

        /// <summary>
        /// The base ArenaBuilder constructor.
        /// </summary>
        public ArenaBuilder(int width, int height, ContentManager Content, float aspectRatio, ArenaDifficulty difficulty)
        {
            instance = this;

            this.width = width;
            this.height = height;
            this.Content = Content;
            this.difficulty = difficulty;

        }

        /// <summary>
        /// This function constructs the layout for an arena.
        /// </summary>
        /// <param name="width">The width of the arena</param>
        /// <param name="height">The height of the arena</param>
        /// <param name="Content">The content manager for loading assets</param>
        /// <returns></returns>
        public Tile[,] buildArenaBase()
        {
            // Create the tile array
            tiles = new Tile[height, width];

            // Determine the maximum length of a path
            int maxLength = (int) Math.Floor(Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2)));

            // Generate the arena, starting with the Start Tile
            generateTile(null, TileConnections.NONE, 1, maxLength);
            
            // Fill empty tiles
            fillEmptyTiles();

            // Set the exit point of the arena
            setExit();

            // Generate the enemies in the arena
            generateEnemies();

            return tiles;
        }

        /// <summary>
        /// This function generates a tile recursively in the arena.
        /// </summary>
        /// <param name="parent">The parent tile (or null if it's the first tile)</param>
        /// <param name="dir">The direction from the parent (or null)</param>
        /// <param name="numConnections">The number of connections to try to create</param>
        /// <param name="maxLength">The maximum length of this path</param>
        /// <returns></returns>
        private Tile generateTile(Tile parent, TileConnections dir, int numConnections, int maxLength)
        {
            Tile tile;

            if (parent == null)
            {
                // This is the first tile in the arena
                int xPos = r.Next(EDGE_START_BUFFER, tiles.GetLength(1) - EDGE_START_BUFFER);
                int yPos = r.Next(EDGE_START_BUFFER, tiles.GetLength(0) - EDGE_START_BUFFER);

                tile = new Tile(parent, xPos, yPos);
                startTile = tile;
                tiles[yPos, xPos] = tile;
            }
            else
            {
                // This is an extension of the arena
                
                // Set the connection with the parent
                switch(dir)
                {
                    case TileConnections.LEFT:
                    {
                        tile = new Tile(parent, (int)parent.getPos().X - 1, (int)parent.getPos().Y);
                        tile.setConnection(TileConnections.RIGHT, parent);
                        tiles[(int)parent.getPos().Y, (int)parent.getPos().X - 1] = tile;

                        break;
                    }

                    case TileConnections.RIGHT:
                    {
                        tile = new Tile(parent, (int)parent.getPos().X + 1, (int)parent.getPos().Y);
                        tile.setConnection(TileConnections.LEFT, parent);
                        tiles[(int)parent.getPos().Y, (int)parent.getPos().X + 1] = tile;

                        break;
                    }

                    case TileConnections.TOP:
                    {
                        tile = new Tile(parent, (int)parent.getPos().X, (int)parent.getPos().Y - 1);
                        tile.setConnection(TileConnections.BOTTOM, parent);
                        tiles[(int)parent.getPos().Y - 1, (int)parent.getPos().X] = tile;

                        break;
                    }

                    case TileConnections.BOTTOM:
                    {
                        tile = new Tile(parent, (int)parent.getPos().X, (int)parent.getPos().Y + 1);
                        tile.setConnection(TileConnections.TOP, parent);
                            tiles[(int)parent.getPos().Y + 1, (int)parent.getPos().X] = tile;

                        break;
                    }

                    default:
                    {
                        // We won't get here
                        tile = new Tile(parent, (int)parent.getPos().X, (int)parent.getPos().Y);

                        break;
                    }
                }
            }

            // Determine the directions
            for (int i = 0; i < numConnections; i++)
            {
                TileConnections connection = TileConnections.NONE;

                // Find a free connection
                for (int j = 0; j < MAX_TRIES; j++)
                {
                    connection = (TileConnections)r.Next(4);

                    if (tile.getConnection(connection) == null)
                    {
                        int x = (int) tile.getPos().X;
                        int y = (int) tile.getPos().Y;

                        // Check the position in the array
                        if ((y + 1 < height && connection == TileConnections.BOTTOM && tiles[y + 1, x] == null)||
                            (y - 1 >= 0 && connection == TileConnections.TOP && tiles[y - 1, x] == null) ||
                            (x + 1 < width && connection == TileConnections.RIGHT && tiles[y, x + 1] == null) ||
                            (x - 1 >= 0 && connection == TileConnections.LEFT && tiles[y, x - 1] == null))
                        {
                            // We've found our next path
                            break;
                        }
                        else
                        {
                            connection = TileConnections.NONE;
                        }
                    }
                    else
                    {
                        connection = TileConnections.NONE;
                    }
                }

                // If we found the next connection
                if (connection != TileConnections.NONE)
                {
                    // Determine the next number of connections
                    int nextNumConnections = r.Next(1, 4);
                    if (maxLength == 0)
                    {
                        nextNumConnections = 0;
                    }

                    tile.setConnection(connection, generateTile(tile, connection, nextNumConnections, maxLength - 1));
                }
                else
                {
                    // We have no more connection opportunities
                    break;
                }
            }

            // Set the tile's art
            setArenaTile(tile);
            
            return tile;
        }

        /// <summary>
        /// This function sets the exit point of the arena
        /// </summary>
        private void setExit()
        {
            Tile curTile = getStartTile();
            List<Tile> visited = new List<Tile>();

            // Keep digging into the maze until we reach an end point
            while (curTile == startTile || curTile.getNumConnections() > 1)
            {
                int nextDir;

                do
                {
                    // Look in a random direction
                    nextDir = r.Next(4);    
                }
                while (visited.Contains(curTile.getConnection((TileConnections) nextDir)) ||
                    curTile.getConnection((TileConnections) nextDir) == null);

                // Set the next tile in the search
                visited.Add(curTile);
                curTile = curTile.getConnection((TileConnections) nextDir);
            }

            // Add the exit door to the tile
            curTile.addEntity(new ArenaExit(curTile, Content));
        }
        
        /// <summary>
        /// This function generates the enemies within the arena
        /// </summary>
        private void generateEnemies()
        {
            int numEnemies = 0;

            // Determine the number of enemies based on the difficulty
            switch(difficulty)
            {
                case ArenaDifficulty.EASY:
                {
                    numEnemies = r.Next(2, 5);
                    break;
                }

                case ArenaDifficulty.MEDIUM:
                {
                    numEnemies = r.Next(3, 6);
                    break;
                }

                case ArenaDifficulty.HARD:
                {
                    numEnemies = r.Next(4, 7);
                    break;
                }
            }

            for (int i = 0; i < numEnemies; i++)
            {
                int x, y;

                // Look for a not-empty, not-starting tile
                do
                {
                    x = r.Next(width);
                    y = r.Next(height);
                }
                while (tiles[y, x].getNumConnections() == 0 || tiles[y, x] == startTile);

                // Create an enemy
                tiles[y, x].addEntity(new ArenaEnemy(tiles[y, x], Content));
            }
        }

        /// <summary>
        /// This function sets the tile's graphical component.
        /// </summary>
        /// <param name="tile">The tile to set the art of</param>
        private void setArenaTile(Tile tile)
        {
            Tile[] connections = new Tile[4];

            // Retrieve the connections from the tile
            for (int i = 0; i < connections.Length; i++)
            {
                connections[i] = tile.getConnection((TileConnections) i);
            }

            if (connections[0] != null)
            {
                // LEFT CONNECTION //

                if (connections[1] != null)
                {
                    if (connections[2] != null)
                    {
                        if (connections[3] != null)
                        {
                            // Cross
                            tile.setArenaTile(ArenaTiles.CROSS, Content);
                        }
                        else
                        {
                            // Tri-Up
                            tile.setArenaTile(ArenaTiles.TRI_UP, Content);
                        }
                    }
                    else
                    {
                        if (connections[3] != null)
                        {
                            // Tri-left
                            tile.setArenaTile(ArenaTiles.TRI_LEFT, Content);
                        }
                        else
                        {
                            // Corner Bottom Right
                            tile.setArenaTile(ArenaTiles.CORNER_BR, Content);
                        }
                    }
                }
                else
                {
                    if (connections[2] != null)
                    {
                        if (connections[3] != null)
                        {
                            // Tri-Bottom
                            tile.setArenaTile(ArenaTiles.TRI_DOWN, Content);
                        }
                        else
                        {
                            // Straight Horizontal
                            tile.setArenaTile(ArenaTiles.STR_HOR, Content);
                        }
                    }
                    else
                    {
                        if (connections[3] != null)
                        {
                            // Corner Top Right
                            tile.setArenaTile(ArenaTiles.CORNER_TR, Content);
                        }
                        else
                        {
                            // Dead End Right
                            tile.setArenaTile(ArenaTiles.DE_RIGHT, Content);
                        }
                    }
                }
            }
            else
            {
                // NO LEFT CONNECTION //

                if (connections[1] != null)
                {
                    if (connections[2] != null)
                    {
                        if (connections[3] != null)
                        {
                            // Tri-Right
                            tile.setArenaTile(ArenaTiles.TRI_RIGHT, Content);
                        }
                        else
                        {
                            // Corner Bottom Left
                            tile.setArenaTile(ArenaTiles.CORNER_BL, Content);
                        }
                    }
                    else
                    {
                        if (connections[3] != null)
                        {
                            // Straight Vertical
                            tile.setArenaTile(ArenaTiles.STR_VERT, Content);
                        }
                        else
                        {
                            // Dead End Bottom
                            tile.setArenaTile(ArenaTiles.DE_BOTTOM, Content);
                        }
                    }
                }
                else
                {
                    if (connections[2] != null)
                    {
                        if (connections[3] != null)
                        {
                            // Corner Top Left
                            tile.setArenaTile(ArenaTiles.CORNER_TL, Content);
                        }
                        else
                        {
                            // Dead End Left
                            tile.setArenaTile(ArenaTiles.DE_LEFT, Content);
                        }
                    }
                    else
                    {
                        if (connections[3] != null)
                        {
                            // Dead End Top
                            tile.setArenaTile(ArenaTiles.DE_TOP, Content);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function fills the rest of the arena with empty tiles.
        /// </summary>
        private void fillEmptyTiles()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i, j] == null)
                    {
                        tiles[i, j] = new Tile(null, j, i);
                        tiles[i, j].setArenaTile(ArenaTiles.EMPTY, Content);
                    }
                }
            }
        }

        /// <summary>
        /// This function returns the starting tile of the arena.
        /// </summary>
        /// <returns>The starting tile of the arena</returns>
        public Tile getStartTile()
        {
            return startTile;
        }
    }
}
