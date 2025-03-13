using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Szeminarium1
{
    internal static class Program
    {
        private static IWindow graphicWindow;

        private static GL Gl;

        private static uint program;

        private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 vPos;
		layout (location = 1) in vec4 vCol;

		out vec4 outCol;
        
        void main()
        {
			outCol = vCol;
            gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
        }
        ";


        private static readonly string FragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;
		
		in vec4 outCol;

        void main()
        {
            FragColor = outCol;
        }
        ";

        static void Main(string[] args)
        {
            WindowOptions windowOptions = WindowOptions.Default;
            windowOptions.Title = "1. szeminárium - háromszög";
            windowOptions.Size = new Silk.NET.Maths.Vector2D<int>(1000, 1000);

            graphicWindow = Window.Create(windowOptions);

            graphicWindow.Load += GraphicWindow_Load;
            graphicWindow.Update += GraphicWindow_Update;
            graphicWindow.Render += GraphicWindow_Render;

            graphicWindow.Run();
        }

        private static void GraphicWindow_Load()
        {
            // egszeri beallitasokat
            //Console.WriteLine("Loaded");

            Gl = graphicWindow.CreateOpenGL();

            Gl.ClearColor(System.Drawing.Color.White);

            uint vshader = Gl.CreateShader(ShaderType.VertexShader);
            uint fshader = Gl.CreateShader(ShaderType.FragmentShader);

            Gl.ShaderSource(vshader, VertexShaderSource);
            Gl.CompileShader(vshader);
            Gl.GetShader(vshader, ShaderParameterName.CompileStatus, out int vStatus);
            if (vStatus != (int)GLEnum.True)
                throw new Exception("Vertex shader failed to compile: " + Gl.GetShaderInfoLog(vshader));

            Gl.ShaderSource(fshader, FragmentShaderSource);
            Gl.CompileShader(fshader);

            program = Gl.CreateProgram();
            Gl.AttachShader(program, vshader);
            Gl.AttachShader(program, fshader);
            Gl.LinkProgram(program);
            Gl.DetachShader(program, vshader);
            Gl.DetachShader(program, fshader);
            Gl.DeleteShader(vshader);
            Gl.DeleteShader(fshader);

            Gl.GetProgram(program, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                Console.WriteLine($"Error linking shader {Gl.GetProgramInfoLog(program)}");
            }

        }

        private static void GraphicWindow_Update(double deltaTime)
        {
            // NO GL
            // make it threadsave
            //Console.WriteLine($"Update after {deltaTime} [s]");
        }

        private static unsafe void GraphicWindow_Render(double deltaTime)
        {
            //Console.WriteLine($"Render after {deltaTime} [s]");

            Gl.Clear(ClearBufferMask.ColorBufferBit);

            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);
            
            float[] vertexArray = new float[] {
                 0.0f, 0.40f, 0.0f,//A0
                -0.5f, 0.65f, 0.0f,//E1
                 -0.5f, 0f, 0.0f,//C2
                 0.0f, -0.3f, 0.0f,//B3
                 0.5f, 0f, 0.0f,//F4
                 0.5f, 0.645f, 0.0f,//G5
                 0.0f, 0.9f, 0.0f,//D6
                 
                 0.0f, 0.40f, 0.0f,//A7
                 -0.5f, 0.65f, 0.0f,//E8
                 0.0f, 0.9f, 0.0f,//D9
                 
                 0.5f, 0.645f, 0.0f,//G10
                 0.0f, 0.9f, 0.0f,//D11
                 0.0f, 0.40f, 0.0f,//A12
                 
                 0.5f, 0f, 0.0f,//F13
                 0.5f, 0.645f, 0.0f,//G14
                 0.0f, 0.40f, 0.0f,//A15
                 
                 0.5f, 0f, 0.0f,//F16
                 0.0f, 0.40f, 0.0f,//A17
                 0.0f, -0.3f,0.0f,//B18
                 
                 //vonalak
                 //VP = vastagsagpont
                 
                 0f, 0.17f, 0f, //A2 19
                 0f, 0.16f, 0f, //A2 vastagsagpont 20
                 0f, -0.063f, 0f, //B2 21
                 0f, -0.073f, 0f, //B2 vastagsagpont 22
                 
                 -0.5f, 0.40f, 0f, //E1 23
                 -0.5f, 0.41f, 0f, //E1 VP 24
                 
                 -0.5f, 0.2f, 0f, //C1 25
                 -0.5f, 0.19f, 0f, //C1 VP 26
                               
                 0.5f, 0.40f, 0f, //G1 27
                 0.5f, 0.41f, 0f, //G1 VP 28
                 
                 0.5f, 0.2f, 0f, //C1 29
                 0.5f, 0.19f, 0f, //C1 VP 30
                 //----------------------------------------------
                 
                 -0.33f, 0.73f, 0f, //E2 31
                 -0.32f, 0.735f, 0f, //E2V 32
                 
                 -0.167f, 0.817f, 0f, //D2 33
                 -0.157f, 0.817f, 0f,//D2V 34
                 
                 0.173f,0.818f, 0f, //D1 35
                 0.183f,0.809f, 0f,//D1V 36
                 
                 0.347f,0.736f, 0f, //G1 37
                 0.357f,0.728f, 0f, //G1V 38
                
                 0.347f, 0.568f, 0f, //G2 39
                 0.354f, 0.574f, 0f,//G2V 40
                 
                 0.173f, 0.485f, 0f, //A1 41
                 0.180f, 0.492f, 0f, //A1V 42
                                 
                 -0.182f, 0.475f, 0f, //A3 43
                 -0.178f, 0.470f, 0f, //A3V 44
                 
                 -0.34f, 0.56f, 0f, //E1 45
                 -0.335f, 0.555f, 0f, //E1V 46
                 
                 //----------------------
                 
                 0.35f, -0.090f, 0f, //F1 47
                 0.36f, -0.090f, 0f, //F1V 48
                 
                 0.17f, -0.2f, 0f, //B1 49
                 0.18f, -0.18f, 0f,//B1V 50
                 
                 -0.35f, -0.095f, 0f, //C1 51
                 -0.34f, -0.090f, 0f,//C!V 52
                 
                 -0.17f, -0.2f, 0f, //B2 53
                 -0.16f, -0.18f, 0f//B2V 54
                 
            };
            
            float[] colorArray = new float[] {
                1.0f, 0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                
                0,1,0,0,
                0,1,0,0,
                0,1,0,0,
                
                0,1,0,0,
                0,1,0,0,
                0,1,0,0,
                
                0,0,1,0,
                0,0,1,0,
                0,0,1,0,
                
                0,0,1,0,
                0,0,1,0,
                0,0,1,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0
                
            };
            
            uint[] indexArray = new uint[] { 
                2,3,0,
                0,1,2,
                4,5,0,
                0,3,4,
                6,5,0,
                6,1,0,
                7,9,8,
                10,11,12,
                13,14,15,
                16,17,18,
                
                //vonalak piros fuggoleges
                19,20,23,
                21,22,25,
                19,24,23,
                26,25,22,
                
                //vonalak kek fuggoleges
                28,19, 27,
                27, 19,20,
                30,29,21,
                29,21,22,
                
                //vonalak felso 1
                
                36,35,45,
                45,46,36,
                
                38,37,43,
                43,44,38,
                
                //vonalak felso 2
                
                31, 32, 42,
                42, 41, 31,
                
                33,34,40,
                40, 39, 33,
                
                //vonalak az oldalakon
                
                45, 51, 52, 
                52, 46, 45,
                
                44, 43, 53,
                53, 54, 44,
                
                42, 41, 49,
                49, 50, 42,
                
                40, 39, 47,
                47, 48, 40
                
            };

            
            uint vertices = Gl.GenBuffer();

            Gl.BindBuffer(GLEnum.ArrayBuffer, vertices);
            Gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)vertexArray.AsSpan(), GLEnum.StaticDraw);
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, null);
            Gl.EnableVertexAttribArray(0);

            uint colors = Gl.GenBuffer();
            Gl.BindBuffer(GLEnum.ArrayBuffer, colors);
            Gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)colorArray.AsSpan(), GLEnum.StaticDraw);
            Gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, null);
            Gl.EnableVertexAttribArray(1);

            uint indices = Gl.GenBuffer();
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, indices);
            Gl.BufferData(GLEnum.ElementArrayBuffer, (ReadOnlySpan<uint>)indexArray.AsSpan(), GLEnum.StaticDraw);

            Gl.BindBuffer(GLEnum.ArrayBuffer, 0);

            Gl.UseProgram(program);
            
            Gl.DrawElements(GLEnum.Triangles, (uint)indexArray.Length, GLEnum.UnsignedInt, null); // we used element buffer
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);
            Gl.BindVertexArray(vao);

            // always unbound the vertex buffer first, so no halfway results are displayed by accident
            Gl.DeleteBuffer(vertices);
            Gl.DeleteBuffer(colors);
            Gl.DeleteBuffer(indices);
            Gl.DeleteVertexArray(vao);
        }
    }
}