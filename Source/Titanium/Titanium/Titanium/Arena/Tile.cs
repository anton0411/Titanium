using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Titanium.Entities;
using Titanium.Scenes;

namespace Titanium.Arena
{
    /// <summary>
    /// This enum contains all possible connection directions to another tile.
    /// </summary>
    public enum TileConnections
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM,
        NONE
    }

    /// <summary>
    /// This class represents a single tile in the arena.
    /// </summary>
    public class Tile : Entity
    {
        public static int TILE_WIDTH = 128;
        public static int TILE_HEIGHT = 128;
        public static float tileScale = 0.5f;

        private ArenaTiles tile;
        private Texture2D texture;
        private Model model;
        //MovableModel
        public Matrix ModelMatrix;
        private Vector3 modelPosition;


        private Vector2 pos;
        private Vector2 drawPos;

        private Tile[] connections = new Tile[4];
        private Tile parent;

        private List<Entity> entityList = new List<Entity>();
        
        /// <summary>
        /// The base Tile constructor. This is responsible for loading the tile art and setting its position on the 'board'.
        /// </summary>
        /// <param name="parent">The tile's parent (creator)</param>
        /// <param name="xPos">The x position of the tile in the array</param>
        /// <param name="yPos">The y position of the tile in the array</param>
        public Tile(Tile parent, int xPos, int yPos)
        {
            this.parent = parent;      

            // Set the position in the array
            pos = new Vector2(xPos, yPos);

            // Set the world position based on the position in the array
            drawPos = new Vector2(xPos * TILE_WIDTH, yPos * TILE_HEIGHT);

            // Determine the location on-screen compared to the start tile
            Tile startTile = ArenaBuilder.instance.getStartTile();
                        
            modelPosition = new Vector3(drawPos.X, -10, drawPos.Y);

        }

        /// <summary>
        /// This function updates the tile's entities.
        /// </summary>
        /// <param name="gamepad">The gamepad state</param>
        /// <param name="keyboard">The keyboard state</param>
        /// <param name="mouse">The mouse state</param>
        public override void Update(GamePadState gamepad, KeyboardState keyboard, MouseState mouse)
        {
            // Update each entity
            foreach (Entity entity in entityList)
            {
                entity.Update(gamepad, keyboard, mouse);
            }
        }

        /// <summary>
        /// This function renders the tile to the screen
        /// </summary>
        /// <param name="sb">The sprite batch object used for rendering</param>
        public override void Draw(SpriteBatch sb)
        {

            if (model != null)//don't do anything if the model is null
            {
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model. A model can have multiple meshes, so loop.
                foreach (ModelMesh mesh in model.Meshes)
                {

                    // This is where the mesh orientation is set, as well as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        // Set the tile's UV Map
                        effect.TextureEnabled = true;
                        effect.Texture = texture;

                        effect.World = transforms[mesh.ParentBone.Index]  * Matrix.CreateRotationY(-1.5708f) * Matrix.CreateScale(0.5f) * Matrix.CreateTranslation(modelPosition);

                        effect.View = ArenaScene.instance.camera.getView();//Matrix.CreateLookAt(cameraPosition, Target, Vector3.Up);//Vector3.Zero
                        effect.Projection = ArenaScene.instance.camera.getProjection();//Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                                                                                 //aspectRatio, 1.0f, 10000.0f);//1366/768
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }

            foreach (Entity entity in entityList)
            {
                entity.Draw(sb);
            }
        }

        /// <summary>
        /// This function sets the tile's graphical component
        /// </summary>
        /// <param name="tile">The tile type</param>
        /// <param name="Content">The content manager for loading the art</param>
        public void setArenaTile(ArenaTiles tile, ContentManager Content)
        {
            this.tile = tile;

            // Load the UV Map and model
            texture = Content.Load<Texture2D>("Models/UVMap" + (int) tile);
            model = Content.Load<Model>("Models/tileBase");
        }

        /// <summary>
        /// This function sets a connection between this tile and another.
        /// </summary>
        /// <param name="dir">The connection direction</param>
        /// <param name="t">The tile to connect with</param>
        public void setConnection(TileConnections dir, Tile t)
        {
            connections[(int) dir] = t;
        }

        /// <summary>
        /// This function returns a connection between this tile and another.
        /// </summary>
        /// <param name="dir">The connection direction</param>
        /// <returns>The tile object if the connection exists, otherwise null</returns>
        public Tile getConnection(TileConnections dir)
        {
            return connections[(int) dir];
        }

        public Tile[] getConnections()
        {
            return connections;
        }

        public int getNumConnections()
        {
            int numConnections = 0;

            for (int i = 0; i < connections.Length; i++)
            {
                if (connections[i] != null)
                {
                    numConnections++;
                }
            }

            return numConnections;
        }

        public void addEntity(Entity entity)
        {
            entityList.Add(entity);
        }

        /// <summary>
        /// This function returns the list of entities currently on this tile.
        /// </summary>
        /// <returns>The list of entities on the tile.</returns>
        public List<Entity> getEntities()
        {
            return entityList;
        }

        /// <summary>
        /// This function returns the logical position of the tile in the array.
        /// </summary>
        /// <returns>The position of the tile</returns>
        public Vector2 getPos()
        {
            return pos;
        }

        /// <summary>
        /// This function returns the visual position of the tile.
        /// </summary>
        /// <returns>The visual position of the tile</returns>
        public Vector2 getDrawPos()
        {
            return drawPos;
        }

        public Vector3 getModelPos()
        {
            return modelPosition;
        }

        public ArenaTiles getType()
        {
            return tile;
        }
    }
}
