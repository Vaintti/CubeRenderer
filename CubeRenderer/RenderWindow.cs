using System;
using System.Drawing.Drawing2D;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace CubeRenderer
{
    class RenderWindow: GameWindow
    {
        float[] vertices =
        {
            -0.5f, 0.5f, -0.5f, 0, 1, 0,
            -0.5f, 0.5f, 0.5f, 0, 1, 0,
            0.5f, 0.5f, 0.5f, 0, 1, 0,
            0.5f, 0.5f, -0.5f, 0, 1, 0,

            -0.5f, 0.5f, -0.5f, 0, 0, 1,
            0.5f, 0.5f, -0.5f, 0, 0, 1,
            0.5f, -0.5f, -0.5f, 0, 0, 1,
            -0.5f, -0.5f, -0.5f, 0, 0, 1,

            0.5f, 0.5f, -0.5f, -1, 0, 0,
            0.5f, 0.5f, 0.5f, -1, 0, 0,
            0.5f, -0.5f, 0.5f, -1, 0, 0,
            0.5f, -0.5f, -0.5f, -1, 0, 0,

            -0.5f, 0.5f, -0.5f, 1, 0, 0,
            -0.5f, 0.5f, 0.5f, 1, 0, 0,
            -0.5f, -0.5f, 0.5f, 1, 0, 0,
            -0.5f, -0.5f, -0.5f, 1, 0, 0,

            -0.5f, 0.5f, 0.5f, 0, 0, -1,
            0.5f, 0.5f, 0.5f, 0, 0, -1,
            0.5f, -0.5f, 0.5f, 0, 0, -1,
            -0.5f, -0.5f, 0.5f, 0, 0, -1,
        };
        uint[] indices =
        {
            12, 14, 13,
            12, 15, 14,

            16, 18, 17,
            16, 19, 18,

            8, 9, 10,
            8, 10, 11,

            4, 5, 6,
            4, 6, 7,

            0, 1, 2,
            0, 2, 3,
        };
        Shader shader;
        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        public RenderWindow(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {}

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1f);
            shader = new Shader("shader.vert", "shader.frag");

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();

            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices,
                BufferUsageHint.DynamicDraw);


            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices,
                BufferUsageHint.DynamicDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            Matrix4 scale = Width > Height
                ? Matrix4.CreateScale((float)Height / Width, 1, 1)
                : Matrix4.CreateScale(1, (float)Width / Height, 1);
            Matrix4 rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45f));
            Matrix4 timeRotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(DateTime.Now.Ticks / 100000 % 360));
            Matrix4 transform = scale * rotation * timeRotation;
            shader.SetMatrix("transform", transform);
            shader.SetMatrix("timeRotation", timeRotation);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            shader.Dispose();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(ElementBufferObject);

            base.OnUnload(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard.GetState().IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e);
        }
    }
}
