using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Titanium.Scenes;

namespace Titanium.Entities
{
    /// <summary>
    /// The arena controller, responsible for managing arena state.
    /// </summary>
    public class ArenaController
    {
        public static ArenaController instance;

        private ArenaDifficulty curDifficulty;
        private bool playerMoved;
        private Random generator;

        /// <summary>
        /// The default constructor for the arena controller.
        /// </summary>
        public ArenaController()
        {
            instance = this;
            playerMoved = false;
            curDifficulty = ArenaDifficulty.EASY;

            generator = new Random();
        }

        /// <summary>
        /// This function is called every frame.
        /// </summary>
        public void update()
        {
            // The player has no longer moved
            if (playerMoved)
            {
                playerMoved = false;
            }
        }

        /// <summary>
        /// This function moves the arena scene to the next arena.
        /// </summary>
        public void moveToNextArena()
        {
            ArenaScene.instance.loadNewArena(curDifficulty);

            // Increment the difficulty
            if (curDifficulty != ArenaDifficulty.HARD)
            {
                curDifficulty++;
            }
        }


        /// <summary>
        /// This function marks that the player has moved this frame.
        /// </summary>
        public void setMoved()
        {
            playerMoved = true;
        }

        /// <summary>
        /// This function returns the player moved state for this frame.
        /// </summary>
        /// <returns>The player moved state</returns>
        public bool getPlayerMoved()
        {
            return playerMoved;
        }

        public Random getGenerator()
        {
            return generator;
        }
    }
}
