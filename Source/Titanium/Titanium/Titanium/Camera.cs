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
using Titanium.Scenes;

namespace Titanium
{
    /// <summary>
    /// Camera class:
    /// A camera object is created to set up the 3D world for the redering of 3D objects.
    /// 
    /// NOTES:
    /// -Does NOT currently set up lighting of the world (this is best left to a rendering class).
    /// -Asks for a BasicEffect object in the constructor rather than the GraphicsDevice; reason for the decision is based off the
    /// fact that Rendering will also need the same BasicEffect object.
    /// 
    /// 
    /// Class Methods:
    /// Camera(BasicEffect effect, int ClientW, int ClientH); 
    /// Takes in the BasicEffect object to and sets up the world camera. Also takes in the client window's Width and Height
    /// in order to create the proper Projection matrix.
    /// 
    /// UpdateCamera(Matrix S, Matrix R, Matrix T); 
    /// Takes in the 3 Matrices needed to update the World and update the World view.
    /// </summary>
    class Camera
    {
        public static float X_DISTANCE = 0f;
        public static float Y_DISTANCE = 400f;
        public static float Z_DISTANCE = 700f;


        private Vector3 position;//camera postion (x,y,z)
        private Vector3 target;//What the camera is looking at
        private Matrix View; //View Matrix (the "eye")
        private Matrix Projection; //Projection Matrix 
        private float _clientW;//the client window's Width
        private float _clientH;//the client window's Height
        private float _aspectRatio;//aspectRatio

        private BasicEffect _effect;//public; accessible for rendering purposes


        /// <summary>
        ///  Constructor to set up the Camera for 3D scenes
        /// </summary>
        /// <param name="effect">the BasicEffect object; is needed to set Pojection, View and World matrices.</param>
        /// <param name="ClientW">The width of the client window.</param>
        /// <param name="ClientH">The height of the cleint window.</param>
        public Camera(BasicEffect effect, float ClientW, float ClientH, float aspectRatio, Vector3 camTarget)
        {
            _effect = effect;

            _clientW = ClientW;
            _clientH = ClientH;

            _aspectRatio = aspectRatio;

            position = new Vector3(X_DISTANCE, Y_DISTANCE, Z_DISTANCE);
            target = camTarget;//new Vector3();//looking at (0,0,0), the Origin/middle

            Vector3 camPos = position + camTarget;

            View = Matrix.CreateLookAt(camTarget, target, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), _aspectRatio, 1f, 10000f);
            

            _effect.Projection = Projection;
            _effect.View = View;

            //lighting            
            //_effect.AmbientLightColor = new Vector3(2.0f, 2.0f, 2.0f);
            //_effect.DirectionalLight0.Enabled = true;
            //_effect.DirectionalLight0.DiffuseColor = Vector3.One;
            //_effect.DirectionalLight0.Direction = Vector3.Normalize(Vector3.One);
            //_effect.LightingEnabled = true;
            _effect.EnableDefaultLighting();

        }

        /// <summary>
        /// gets the view matrix of the camera.
        /// </summary>
        /// <returns>the View matrix.</returns>
        public Matrix getView()
        {
            return View;
        }

        /// <summary>
        /// gets the projection matrix of the camera.
        /// </summary>
        /// <returns>the Projection matrix</returns>
        public Matrix getProjection()
        {
            return Projection;
        }

        public void UpdateCamera(Vector3 CharacterPos)
        {
            Vector3 camPos = position + CharacterPos;
            View = Matrix.CreateLookAt(camPos, CharacterPos, Vector3.Up);
            _effect.View = View;
        }

    }
}
