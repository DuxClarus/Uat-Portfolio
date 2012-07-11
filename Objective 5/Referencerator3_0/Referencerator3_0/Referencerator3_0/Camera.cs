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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using RefrenceratorV3_0.Framework;
using Referencerator3_0;

namespace RefrenceratorV3_0
{
    public class Camera
    {
        #region Fields

        private Matrix view;
        private Matrix projection;

        private Vector3 position;
        private Vector3 target;

        private float farPlane;
        private float nearPlane;

        private float yaw = 0f;
        private float pitch = 0f;

        private Ray mouseRay;

        protected GraphicsDevice device;
        private InputState input;


        private int cameraSpeed = 50;
        private bool cameraInMovement;
        #endregion

        #region Initialization

        public Camera(InputState input, Game1 game)
        {
            if (position == target) target.Z += 10f;

            //TODO: COMMENTS!
            this.position = new Vector3(0, 10, 75);
            this.target = new Vector3(0);

            this.device = game.GraphicsDevice;
            this.nearPlane = 0.01f;
            this.farPlane = 800f;

            this.input = input;

            // If the camera's looking straight down it has to be fixed

            while (Math.Abs(pitch) >= MathHelper.ToRadians(80))
            {
                this.position.Z += 10;
            }

            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, nearPlane, farPlane);
            view = Matrix.CreateLookAt(this.position, target, this.Up);
        }

        #endregion

        #region Ray calculation

        public Ray GetMouseRay()
        {
            Vector3 nearPoint = new Vector3(input.Mouse.Position, 0);
            Vector3 farPoint = new Vector3(input.Mouse.Position, 1);

            nearPoint = device.Viewport.Unproject(nearPoint, projection, view, Matrix.Identity);
            farPoint = device.Viewport.Unproject(farPoint, projection, view, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        public virtual void UpdateMouseRay()
        {
            mouseRay = this.GetMouseRay();
        }

        #endregion

        #region Cam Movement

        public virtual void Zoom(float amount)
        {
            Vector3 temp = position;
            position += amount * this.Direction;
            target += amount * this.Direction;
        }

        public virtual void PanLeftRight(float amount)
        {
            position += amount * this.Right;
            target += amount * this.Right;
        }

        public virtual void PanUpDown(float amount)
        {
            position.Y += amount;
            target.Y += amount;
        }

        public void CameraController()
        {
            Zoom(input.Mouse.ScrollDelta / cameraSpeed);

            if (input.IsKeyDown(Keys.LeftAlt))
            {
                cameraInMovement = true;

                if (input.Mouse.IsButtonDown(MouseButtons.Middle))
                    CalculateCameraPanMovement();

                if (input.Mouse.IsButtonDown(MouseButtons.Left))
                    CalculateCameraPositionMovement();
            }

            if (input.WasKeyPressed(Keys.F))
                ResetCamera();
        }


        private void ResetCamera()
        {
            Position = new Vector3(0, 10, 75);
            target = new Vector3(0);
        }

        private void CalculateCameraPanMovement()
        {
            PanLeftRight(input.Mouse.Delta.X / cameraSpeed);
            PanUpDown(input.Mouse.Delta.Y / cameraSpeed);
        }

        private void CalculateCameraPositionMovement()
        {
            Position = Vector3.Transform(position - target, Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), (input.Mouse.Delta.X / cameraSpeed))) + target;
            Position = Vector3.Transform(position - target, Matrix.CreateFromAxisAngle(new Vector3(1, 0, 0), (input.Mouse.Delta.Y / cameraSpeed))) + target;
        }

        #endregion

        #region Update

        public virtual void Update()
        {
            view = Matrix.CreateLookAt(position, target, this.Up);
            CameraController();
            UpdateMouseRay();
        }

        #endregion

        #region Properties

        public bool CameraInMovement
        {
            get
            {
                return cameraInMovement;
            }
            set
            {
                cameraInMovement = value;
            }
        }

        public int CameraSpeed
        {
            get
            {
                return cameraSpeed;
            }
            set
            {
                cameraSpeed = value;
            }
        }
        public BoundingFrustum GetFrustum
        {
            get
            {
                return new BoundingFrustum(View * Projection);
            }
        }

        public Matrix View
        {
            get
            {
                return view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(target - position);
            }
        }

        public Vector3 Up
        {
            get
            {
                return Vector3.Up;
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(this.Direction, this.Up));
                //return Vector3.Normalize(Vector3.Cross(this.Up, this.Direction));
            }
        }

        public Ray MouseRay
        {
            get
            {
                return mouseRay;
            }
        }


        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public float Near
        {
            get
            {
                return nearPlane;
            }
            set
            {
                nearPlane = value;
            }
        }

        public float Far
        {
            get
            {
                return farPlane;
            }
            set
            {
                farPlane = value;
            }
        }

        #endregion
    }
}
