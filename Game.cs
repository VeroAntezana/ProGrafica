using System;
using OpenTK;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace ProGrafica
{
    public class Game: GameWindow
    {
        private int vertexBufferHandle;
        private int shaderProgramHandle;
        private int vertexArrayHandle;

        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        { 
            this.CenterWindow(new Vector2i(800, 600));
            
        }
        protected override void OnLoad() // Funcion de carga solo se llama una vez
        {
            GL.ClearColor(new Color4(0.3f, 0.2f, 0.3f, 1f));
            float[] vertices = new float[] {
                0.0f,0.5f,0f,
                0.5f,-0.5f,0f,
                -0.5f,-0.5f,0f
            };
            this.vertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);// enlazan el buffer significa que ahora estamos trabajando en este buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),vertices,
            BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.vertexArrayHandle= GL.GenVertexArray();
            GL.BindVertexArray(this.vertexArrayHandle);//vncular la matriz de vertices y pasarla al 
                                                       //identificador de matriz

            //Debemos de decirle en donde va a dibujar y todo esto se vinculara al objeto de matriz 
            // de vertices
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.vertexBufferHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0); // Para  habilitar

            GL.BindVertexArray(0);
            string vertexShaderCode =
                @"
                 #version 330 core
                 layout (location = 0 ) in vec3 aPosition;
                  void main(){
                    gl_Position = vec4(aPosition,1f);
                  }
                ";
            string pixelShaderCode =
                @"
                out vec4 pixelColor;
                void main{
                    pixelColor = vec4(0.8f,0.8f,0.1f,1f);

                }
                 ";

            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader); //sombreado de los vertices
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);
            GL.CompileShader(vertexShaderHandle);

            //Sombreado de pixeles
            int pixelShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(pixelShaderHandle, pixelShaderCode);
            GL.CompileShader(pixelShaderHandle);


            //Vamos a adjuntar los dos sombreado de vertices y pixeles

            this.shaderProgramHandle = GL.CreateProgram();
            GL.AttachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.AttachShader(this.shaderProgramHandle, pixelShaderHandle);

            GL.LinkProgram(this.shaderProgramHandle);
            //Luego de correr el programa podemos deshacernos de los recursos del
            //sombreado individuales

            //Separamos el sombreado de vertices
            GL.DetachShader(this.shaderProgramHandle, vertexShaderHandle);
            GL.DetachShader(this.shaderProgramHandle, pixelShaderHandle);
            base.OnLoad();

            //Eliminarlos estos recursos que ya no necesito en la memoria

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(pixelShaderHandle);

        }

        protected override void OnResize( ResizeEventArgs e) { 
            GL.Viewport(0,0, e.Width, e.Height);
            base.OnResize(e);
            
        }
        protected override void OnUnload() { //Funcion de descarga y se llama 
            //cuando el programa se este cerrando
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(this.vertexBufferHandle);
            GL.UseProgram(0); //Comprobar que no estemos usando nada
            GL.DeleteProgram(this.shaderProgramHandle);
            base.OnUnload();
        }


        protected override void OnUpdateFrame(FrameEventArgs args) 
            //Las actualizaciones se haran aqui
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
            // El rederizado se hara aca 
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(this.shaderProgramHandle);
            GL.BindVertexArray(this.vertexArrayHandle);

            GL.DrawArrays(PrimitiveType.Triangles,0, 3);//Esto dibujar debemos decirle que dibujara Triangulos
            
            this.Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

       
    }
}
