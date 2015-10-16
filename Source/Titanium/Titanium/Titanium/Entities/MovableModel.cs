using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Titanium.Entities
{
    class MovableModel : Entity
    {
        private float aspectRatio, modelRotation;
        private Model myModel;
        private SpriteBatch spriteBatch;
        private String modelPath;
        private Vector3 modelPosition, cameraPosition;

        public MovableModel()
        {
            modelRotation = 0.0f;
            modelPosition = Vector3.Zero;
            cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        }

        public void setPath(String s) { modelPath = s; }
        public String getPath() { return modelPath; }
        public void setAspectRatio(float f) { aspectRatio = f; }
        public void setModel(Model m) { myModel = m; }

        public int LoadContent()
        {
            return 0;
        }

        override public void Update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            //held off until later; avoiding conflict with Character class
        }

        override public void Draw(SpriteBatch sb)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

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
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
                        * Matrix.CreateTranslation(modelPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
