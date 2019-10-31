using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace OpnTKAssmnt
{
    class Window : GameWindow
    {
        private readonly float[] Vertices =
        {
            //Front
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            //Back
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            //Left
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

             //Right
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

             //Bottom
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

             //Top
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f

        };

        private readonly Vector3 LightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int VertexBufferObject;
        private int VaoModel;
        private int VaoLamp;

        private Shader LampShader;
        private Shader LightingShader;

        private Camera TheCamera;
        private bool FirstMove = true;
        private Vector2 LastPos;

        public Window(int W, int H, string Title) : base(W, H, GraphicsMode.Default, Title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            LightingShader = new Shader("Shades/shade.vert", "Shades/lights.frag");
            LampShader = new Shader("Shades/shade.vert", "Shades/shade.frag");

            VaoModel = GL.GenVertexArray();
            GL.BindVertexArray(VaoModel);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            var positionLocation = LightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = LightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));


            VaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(VaoLamp);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            positionLocation = LampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            TheCamera = new Camera(Vector3.UnitZ * 3, Width / (float)Height);

            CursorVisible = false;

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(VaoModel);

            LightingShader.Use();

            LightingShader.SetMatrix4("model", Matrix4.Identity);
            LightingShader.SetMatrix4("view", TheCamera.GetViewMatrix());
            LightingShader.SetMatrix4("projection", TheCamera.GetProjectionMatrix());

            LightingShader.SetVector3("viewPos", TheCamera.Position);

            LightingShader.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            LightingShader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            LightingShader.SetVector3("material.specular", new Vector3(1.0f, 0.5f, 0.31f));
            LightingShader.SetFloat("material.shininess", 32.0f);

            Vector3 LightColor;

            float time = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
            LightColor.X = (float)Math.Sin(time * 2.0f);
            LightColor.Y = (float)Math.Sin(time * 0.7f);
            LightColor.Z = (float)Math.Sin(time * 1.3f);

            Vector3 AmbientColor = LightColor * new Vector3(0.2f);
            Vector3 DiffuseColor = LightColor * new Vector3(0.5f);

            LightingShader.SetVector3("light.position", LightPos);
            LightingShader.SetVector3("light.ambient", AmbientColor);
            LightingShader.SetVector3("light.diffuse", DiffuseColor);
            LightingShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.BindVertexArray(VaoModel);

            LampShader.Use();

            Matrix4 LampMatrix = Matrix4.Identity;
            LampMatrix *= Matrix4.CreateScale(0.2f);
            LampMatrix *= Matrix4.CreateTranslation(LightPos);

            //Pick up on line 167
            LampShader.SetMatrix4("model", LampMatrix);
            LampShader.SetMatrix4("view", TheCamera.GetViewMatrix());
            LampShader.SetMatrix4("projection", TheCamera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();

            base.OnRenderFrame(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused)
            {
                return;
            }

            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            const float CamSpeed = 1.5f;
            const float Sensitivity = 0.2f;

            if (input.IsKeyDown(Key.W))
            {
                TheCamera.Position += TheCamera.Fronty * CamSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Key.S))
            {
                TheCamera.Position -= TheCamera.Fronty * CamSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Key.A))
            {
                TheCamera.Position -= TheCamera.Righty * CamSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Key.D))
            {
                TheCamera.Position += TheCamera.Righty * CamSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Key.Space))
            {
                TheCamera.Position += TheCamera.Upy * CamSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Key.LShift))
            {
                TheCamera.Position -= TheCamera.Upy * CamSpeed * (float)e.Time;
            }

            var TheMouse = Mouse.GetState();

            if (FirstMove)
            {
                LastPos = new Vector2(TheMouse.X, TheMouse.Y);
                FirstMove = false;
            }
            else
            {
                var DeltaX = TheMouse.X - LastPos.X;
                var DeltaY = TheMouse.Y - LastPos.Y;
                LastPos = new Vector2(TheMouse.X, TheMouse.Y);

                TheCamera.Yawy += DeltaX * Sensitivity;
                TheCamera.Pitchy -= DeltaY * Sensitivity;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            TheCamera.Fovy -= e.DeltaPrecise;
            base.OnMouseWheel(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            TheCamera.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VaoModel);
            GL.DeleteVertexArray(VaoLamp);

            GL.DeleteProgram(LampShader.Handle);
            GL.DeleteProgram(LightingShader.Handle);

            base.OnUnload(e);
        }
    }
    
}
