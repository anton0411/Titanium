using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Titanium.Entities;
using Titanium.Arena;
using Titanium.Scenes;

namespace Titanium.Entities
{
    /**
     * This class provides a base for all in-game entities that must be updated and rendered to the screen.
     */
    public class ArenaExit : Entity
    {
        private Tile _currentTile;//the current tile we are standing on
        private Vector3 _Position;
        private ForwardDir _forward;//1 = up; 2 = right; 3 = down; 4 = left 

        //MovableModel
        private float modelRotation;
        public Model myModel;
        //public Matrix ModelMatrix;
        //private SpriteBatch spriteBatch;
        //private String modelPath;
        private Vector3 modelPosition, cameraPosition;
        private Vector3 Target;
        
        private float scale;
        
        /**
         * The default entity constructor.
         */
        public ArenaExit(Tile createTile, ContentManager Content)
        {
            _currentTile = createTile;
            _Position = new Vector3(_currentTile.getModelPos().X, 0, _currentTile.getModelPos().Z); //should start in the middle of the start tile (X, Y, Z);

            //_Position = Vector3.Zero;
            _forward = ForwardDir.UP;

            if (_currentTile.getType() == ArenaTiles.DE_BOTTOM || _currentTile.getType() == ArenaTiles.DE_TOP)
            {
                modelRotation = 1.5708f;
            }
            else
            {
                modelRotation = 0;
            }

            scale = 0.3f;
            
            // Load the model
            myModel = myModel = Content.Load<Model>("Models/exitDoor");
        }
        
        /**
         * The update function called in each frame.
         */
        public override void Update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {

        }

        /**
         * The draw function called at the end of each frame.
         */
        public override void Draw(SpriteBatch sb)
        {
            if (myModel != null)//don't do anything if the model is null
            {
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[myModel.Bones.Count];
                myModel.CopyAbsoluteBoneTransformsTo(transforms);

                // Draw the model. A model can have multiple meshes, so loop.
                foreach (ModelMesh mesh in myModel.Meshes)
                {

                    // This is where the mesh orientation is set, as well as our camera and projection.
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale, scale, scale) * Matrix.CreateRotationY(modelRotation)
                            * Matrix.CreateTranslation(_Position);
                        effect.View = ArenaScene.instance.camera.getView();
                        effect.Projection = ArenaScene.instance.camera.getProjection();
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }
        }
    }
}
