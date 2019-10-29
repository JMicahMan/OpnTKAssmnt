using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpnTKAssmnt
{
    class Camera
    {
        private Vector3 Front = -Vector3.UnitZ;
        private Vector3 Up = Vector3.UnitY;
        private Vector3 Right = Vector3.UnitX;

        private float Pitch;
        private float Yaw = -MathHelper.PiOver2;
        private float Fov = MathHelper.PiOver2;

        public Vector3 Fronty => Front;
        public Vector3 Upy => Up;
        public Vector3 Righty => Right;
          
        public Vector3 Position
        {
            get;
            set;
        }

        public float AspectRatio 
        {
            private get;
            set;
        }

        public void UpdateVectors()
        {
            Front.X = (float)Math.Cos(Pitch) * (float)Math.Cos(Yaw);
            Front.Y = (float)Math.Sin(Pitch);
            Front.Z = (float)Math.Cos(Pitch) * (float)Math.Sin(Yaw);

            Front = Vector3.Normalize(Front);

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            
        }
        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }
        public float Pitchy
        {
            get => MathHelper.RadiansToDegrees(Pitch);
            set
            {
                var Angle = MathHelper.Clamp(value, -89f, 89f);
                Pitch = MathHelper.DegreesToRadians(Angle);
                UpdateVectors();
            }
        }

        public float Yawy
        {
            get => MathHelper.RadiansToDegrees(Yaw);
            set
            {
                Yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float Fovy
        {
            get => MathHelper.RadiansToDegrees(Fov);
            set
            {
                var Angle = MathHelper.Clamp(value, 1f, 45f);
                Fov = MathHelper.DegreesToRadians(Angle);
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, 0.01f, 100f);
        }
    }
}
