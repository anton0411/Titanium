using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Titanium.Arena;
using Titanium.Scenes;
using Titanium.Utilities;
using Microsoft.Xna.Framework.Content;

namespace Titanium.Entities
{
    public enum ForwardDir {UP, DOWN, LEFT, RIGHT};
    class Character : Entity
    {
        /// <summary>
        /// The rate at which the player moves between tiles.
        /// </summary>
        public static int MOVE_RATE = 10;

        /// <summary>
        /// The minimum distance from the center of the destination target before the player can move again.
        /// </summary>
        public static float MIN_MOVE_DIS = 20;

        private Tile _StartTile;//the inital starting tile
        private Tile _currentTile;//the current tile we are standing on
        private Vector3 _Position;
        private ForwardDir _forward;//1 = up; 2 = right; 3 = down; 4 = left 

        //previous peripheral device state (Gamepad/Keyboard/Mouse)
        private GamePadState _PrevGPState;
        private KeyboardState _PrevKBState;
        private MouseState _PrevMState;

        //current peripheral device state (Gamepad/Keyboard/Mouse)
        private GamePadState _GPState;
        private KeyboardState _KBState;
        private MouseState _MState;

        //MovableModel
        private float aspectRatio, modelRotation;
        public Model myModel;
        //public Matrix ModelMatrix;
        //private SpriteBatch spriteBatch;
        //private String modelPath;
        private Vector3 modelPosition;
        
        private float rotAngle;
        private float scale;

        /// <summary>
        /// default constructor.
        /// </summary>
        public Character()
        {
            _StartTile = ArenaScene.instance.getStartTile();
            _currentTile = _StartTile;
            _Position = new Vector3(_StartTile.getModelPos().X, 0, _StartTile.getModelPos().Z); //should start in the middle of the start tile (X, Y, Z);
            
            _forward = ForwardDir.UP;

            _PrevGPState = GamePad.GetState(PlayerIndex.One);
            _PrevKBState = Keyboard.GetState();
            _PrevMState = Mouse.GetState();
            
            rotAngle = 0;
            scale = 0.5f;
            
            modelRotation = 0.0f;

            myModel = null;
        }

        public void LoadModel(ContentManager cm, float aspectRatio)
        {
            myModel = cm.Load<Model>("Models/hero");
            this.aspectRatio = aspectRatio;
        }

        /// <summary>
        /// inherited draw method from Entity class.
        /// </summary>
        /// <param name="sb"></param>
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
                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale, scale, scale)* Matrix.CreateRotationY(modelRotation)
                            * Matrix.CreateTranslation(_Position);
                        effect.View = ArenaScene.instance.camera.getView();
                        effect.Projection = ArenaScene.instance.camera.getProjection();
                    }
                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }
        }

        /// <summary>
        /// inherited update method from Entity class.
        /// Move character here.
        /// </summary>
        /// <param name="gamepadState"></param>
        /// <param name="keyboardState"></param>
        /// <param name="mouseState"></param>
        public override void Update(GamePadState gamepadState, KeyboardState keyboardState, MouseState mouseState)
        {
            _GPState = gamepadState;
            _KBState = keyboardState;
            _MState = mouseState;

            if (Vector2.Distance(new Vector2(_Position.X, _Position.Z), _currentTile.getDrawPos()) <= MIN_MOVE_DIS)
            {
                // Only allow movement if the player is on the next tile
                moveCharacter();
            }

            _Position.X += MathUtils.smoothChange(_Position.X, _currentTile.getDrawPos().X, MOVE_RATE);
            _Position.Z += MathUtils.smoothChange(_Position.Z, _currentTile.getDrawPos().Y, MOVE_RATE);
        }

        /// <summary>
        /// Method to get the characters current position.
        /// </summary>
        /// <returns>The position of the player character as a Vector3.</returns>
        public Vector3 getPosition()
        {
            return _Position;
        }

        /// <summary>
        /// will take the input states from the update method call.
        /// If any of the states indicate character movement, the characters _positon will be updated.
        /// </summary>
        /// <param name="baseArena"></param>
        private void moveCharacter()
        {
            //GamePad
            if (_GPState.IsConnected)
            {
                //move up
                if(_GPState.DPad.Up == ButtonState.Pressed && _PrevGPState.DPad.Up == ButtonState.Released)
                {
                    if(_currentTile.getConnection(TileConnections.TOP) != null)
                    {
                        //deltaZ = deltaZ - Tile.TILE_HEIGHT;
                        Tile temp = _currentTile.getConnection(TileConnections.TOP);
                        _currentTile = temp;

                        ArenaController.instance.setMoved();
                    }

                    rotAngle = MathHelper.ToRadians(180);
                }

                //move down
                if(_GPState.DPad.Down == ButtonState.Pressed && _PrevGPState.DPad.Down == ButtonState.Released)
                {
                    if(_currentTile.getConnection(TileConnections.BOTTOM)!= null)
                    {
                        //deltaZ = deltaZ + Tile.TILE_HEIGHT;
                        Tile temp = _currentTile.getConnection(TileConnections.BOTTOM);
                        _currentTile = temp;

                        ArenaController.instance.setMoved();
                    }

                    rotAngle = MathHelper.ToRadians(0);
                }

                //move left
                if(_GPState.DPad.Left == ButtonState.Pressed && _PrevGPState.DPad.Left == ButtonState.Released)
                {
                    if(_currentTile.getConnection(TileConnections.LEFT)!=null)
                    {
                        //deltaX = deltaX - Tile.TILE_WIDTH;
                        Tile temp = _currentTile.getConnection(TileConnections.LEFT);
                        _currentTile = temp;

                        ArenaController.instance.setMoved();
                    }

                    rotAngle = MathHelper.ToRadians(270);
                }

                //move right
                if(_GPState.DPad.Right == ButtonState.Pressed && _PrevGPState.DPad.Right == ButtonState.Released)
                {
                    if(_currentTile.getConnection(TileConnections.RIGHT) != null)
                    {
                        //deltaX = deltaX + Tile.TILE_WIDTH;
                        Tile temp = _currentTile.getConnection(TileConnections.RIGHT);
                        _currentTile = temp;

                        ArenaController.instance.setMoved();
                    }

                    rotAngle = MathHelper.ToRadians(90);
                }
            }
            else
            {
                //controller is disconnected!
            }

            //Keyboard
            //move up
            if ((_KBState.IsKeyDown(Keys.Up) && _PrevKBState.IsKeyUp(Keys.Up)) || (_KBState.IsKeyDown(Keys.W) && _PrevKBState.IsKeyUp(Keys.W)))
            {
                if (_currentTile.getConnection(TileConnections.TOP) != null)
                {
                    //deltaZ = deltaZ - Tile.TILE_HEIGHT;
                    Tile temp = _currentTile.getConnection(TileConnections.TOP);
                    _currentTile = temp;

                    ArenaController.instance.setMoved();
                }

                rotAngle = MathHelper.ToRadians(180);
            }

            //move down
            if ((_KBState.IsKeyDown(Keys.Down) && _PrevKBState.IsKeyUp(Keys.Down)) || (_KBState.IsKeyDown(Keys.S) && _PrevKBState.IsKeyUp(Keys.S)))
            {
                if (_currentTile.getConnection(TileConnections.BOTTOM) != null)
                {
                    //deltaZ = deltaZ + Tile.TILE_HEIGHT;
                    _currentTile = _currentTile.getConnection(TileConnections.BOTTOM);

                    ArenaController.instance.setMoved();
                }

                rotAngle = MathHelper.ToRadians(0);
            }

            //move left
            if ((_KBState.IsKeyDown(Keys.Left) && _PrevKBState.IsKeyUp(Keys.Left)) || (_KBState.IsKeyDown(Keys.A) && _PrevKBState.IsKeyUp(Keys.A)))
            {
                if (_currentTile.getConnection(TileConnections.LEFT) != null)
                {
                    //deltaX = deltaX - Tile.TILE_WIDTH;
                    _currentTile = _currentTile.getConnection(TileConnections.LEFT);

                    ArenaController.instance.setMoved();
                }

                rotAngle = MathHelper.ToRadians(270);
            }

            //move right
            if ((_KBState.IsKeyDown(Keys.Right) && _PrevKBState.IsKeyUp(Keys.Right)) || (_KBState.IsKeyDown(Keys.D) && _PrevKBState.IsKeyUp(Keys.D)))
            {
                if (_currentTile.getConnection(TileConnections.RIGHT) != null)
                {
                    //deltaX = deltaX + Tile.TILE_WIDTH;
                    Tile temp = _currentTile.getConnection(TileConnections.RIGHT);
                    _currentTile = temp;

                    ArenaController.instance.setMoved();
                }

                rotAngle = MathHelper.ToRadians(90);
            }

            //Mouse?

            //save current input as previous
            _PrevGPState = _GPState;
            _PrevKBState = _KBState;
            _PrevMState = _MState;
            
            //update model rotation
            modelRotation = rotAngle;           
        }


    }
}
