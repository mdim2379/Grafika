using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Szeminarium;

namespace GrafikaSzeminarium
{
    internal class Program
    {
        private static IWindow graphicWindow;

        private static GL Gl;

        private static ModelObjectDescriptor[] cubes;

        private static CameraDescriptor camera = new CameraDescriptor();

        private static CubeArrangementModel[] cubeArrangementModels =  new CubeArrangementModel[27];

        private const string ModelMatrixVariableName = "uModel";
        private const string ViewMatrixVariableName = "uView";
        private const string ProjectionMatrixVariableName = "uProjection";

        private static bool elso = true;
        
        private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 vPos;
		layout (location = 1) in vec4 vCol;

        uniform mat4 uModel;
        uniform mat4 uView;
        uniform mat4 uProjection;

		out vec4 outCol;
        
        void main()
        {
			outCol = vCol;
            gl_Position = uProjection*uView*uModel*vec4(vPos.x, vPos.y, vPos.z, 1.0);
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

        private static uint program;

        static void Main(string[] args)
        {
            WindowOptions windowOptions = WindowOptions.Default;
            windowOptions.Title = "Grafika szeminárium";
            windowOptions.Size = new Silk.NET.Maths.Vector2D<int>(500, 500);

            graphicWindow = Window.Create(windowOptions);

            graphicWindow.Load += GraphicWindow_Load;
            graphicWindow.Update += GraphicWindow_Update;
            graphicWindow.Render += GraphicWindow_Render;
            graphicWindow.Closing += GraphicWindow_Closing;

            graphicWindow.Run();
        }

        private static void GraphicWindow_Closing()
        {
            foreach (ModelObjectDescriptor i in cubes)
            {
                i.Dispose();
            }

            Gl.DeleteProgram(program);
        }

        private static void GraphicWindow_Load()
        {
            cubes = new ModelObjectDescriptor[27];
            
            Gl = graphicWindow.CreateOpenGL();

            var inputContext = graphicWindow.CreateInput();
            foreach (var keyboard in inputContext.Keyboards)
            {
                keyboard.KeyDown += Keyboard_KeyDown;
            }
            
            for(int i=0; i<27; i++)
                cubeArrangementModels[i] = new CubeArrangementModel();
            
            cubes = new ModelObjectDescriptor[27];
            for (int i = 0; i < cubes.Length; i++)
                cubes[i] = ModelObjectDescriptor.CreateCube(Gl, i);

            Gl.ClearColor(System.Drawing.Color.White);
            
            Gl.Enable(EnableCap.CullFace);
            Gl.CullFace(TriangleFace.Back);

            Gl.Enable(EnableCap.DepthTest);
            Gl.DepthFunc(DepthFunction.Lequal);


            uint vshader = Gl.CreateShader(ShaderType.VertexShader);
            uint fshader = Gl.CreateShader(ShaderType.FragmentShader);

            Gl.ShaderSource(vshader, VertexShaderSource);
            Gl.CompileShader(vshader);
            Gl.GetShader(vshader, ShaderParameterName.CompileStatus, out int vStatus);
            if (vStatus != (int)GLEnum.True)
                throw new Exception("Vertex shader failed to compile: " + Gl.GetShaderInfoLog(vshader));

            Gl.ShaderSource(fshader, FragmentShaderSource);
            Gl.CompileShader(fshader);
            Gl.GetShader(fshader, ShaderParameterName.CompileStatus, out int fStatus);
            if (fStatus != (int)GLEnum.True)
                throw new Exception("Fragment shader failed to compile: " + Gl.GetShaderInfoLog(fshader));

            program = Gl.CreateProgram();
            Gl.AttachShader(program, vshader);
            Gl.AttachShader(program, fshader);
            Gl.LinkProgram(program);

            Gl.DetachShader(program, vshader);
            Gl.DetachShader(program, fshader);
            Gl.DeleteShader(vshader);
            Gl.DeleteShader(fshader);
            if ((ErrorCode)Gl.GetError() != ErrorCode.NoError)
            {

            }

            Gl.GetProgram(program, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                Console.WriteLine($"Error linking shader {Gl.GetProgramInfoLog(program)}");
            }
        }

        private static void Keyboard_KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            CubeArrangementModel temp = new CubeArrangementModel();
            switch (key)
            {
                case Key.Left:
                    camera.IncreaseZYAngle();
                    break;
                case Key.Right:
                    camera.DecreaseZYAngle();
                    break;
                case Key.Up:
                    camera.IncreaseZXAngle();
                    break;
                case Key.Down:
                    camera.DecreaseZXAngle();
                    break;
                case Key.W:
                    camera.setOffset(0);
                    break;
                case Key.A:
                    camera.setOffset(1);
                    break;
                case Key.S:
                    camera.setOffset(2);
                    break;
                case Key.D:
                    camera.setOffset(3);
                    break;
                case Key.Space:
                    camera.setOffset(4);
                    break;
                case Key.ControlLeft:
                    camera.setOffset(5);
                    break;
                
                case Key.Number1:
                    elso = false;
                    cubeArrangementModels[6].Forgatasok[2] = true;
                    cubeArrangementModels[7].Forgatasok[2] = true;
                    cubeArrangementModels[8].Forgatasok[2] = true;
                    cubeArrangementModels[15].Forgatasok[2] = true;
                    cubeArrangementModels[16].Forgatasok[2] = true;
                    cubeArrangementModels[17].Forgatasok[2]= true;
                    cubeArrangementModels[24].Forgatasok[2] = true;
                    cubeArrangementModels[25].Forgatasok[2] = true;
                    cubeArrangementModels[26].Forgatasok[2] = true;
                    
                    temp = cubeArrangementModels[6];
                    cubeArrangementModels[6] = cubeArrangementModels[24];
                    cubeArrangementModels[24] = cubeArrangementModels[26];
                    cubeArrangementModels[26] = cubeArrangementModels[8];
                    cubeArrangementModels[8] = temp;
                    
                    temp = cubeArrangementModels[15];
                    cubeArrangementModels[15] = cubeArrangementModels[25];
                    cubeArrangementModels[25] = cubeArrangementModels[17];
                    cubeArrangementModels[17] = cubeArrangementModels[7];
                    cubeArrangementModels[7] = temp;
                    
                    break;
                
                case Key.Keypad1:
                    cubeArrangementModels[6].Forgatasok[3] = true;
                    cubeArrangementModels[7].Forgatasok[3] = true;
                    cubeArrangementModels[8].Forgatasok[3] = true;
                    cubeArrangementModels[15].Forgatasok[3] = true;
                    cubeArrangementModels[16].Forgatasok[3] = true;
                    cubeArrangementModels[17].Forgatasok[3]= true;
                    cubeArrangementModels[24].Forgatasok[3] = true;
                    cubeArrangementModels[25].Forgatasok[3] = true;
                    cubeArrangementModels[26].Forgatasok[3] = true;
                    break;
                    
                case Key.Number2:
                    cubeArrangementModels[8].Forgatasok[4] = true;
                    cubeArrangementModels[5].Forgatasok[4] = true;
                    cubeArrangementModels[2].Forgatasok[4] = true;
                    cubeArrangementModels[17].Forgatasok[4]= true;
                    cubeArrangementModels[14].Forgatasok[4] = true;
                    cubeArrangementModels[11].Forgatasok[4] = true;
                    cubeArrangementModels[26].Forgatasok[4] = true;
                    cubeArrangementModels[23].Forgatasok[4] = true;
                    cubeArrangementModels[20].Forgatasok[4] = true;
                    break;
                
                case Key.Keypad2:
                    cubeArrangementModels[8].Forgatasok[5] = true;
                    cubeArrangementModels[5].Forgatasok[5] = true;
                    cubeArrangementModels[2].Forgatasok[5] = true;
                    cubeArrangementModels[17].Forgatasok[5]= true;
                    cubeArrangementModels[14].Forgatasok[5] = true;
                    cubeArrangementModels[11].Forgatasok[5] = true;
                    cubeArrangementModels[26].Forgatasok[5] = true;
                    cubeArrangementModels[23].Forgatasok[5] = true;
                    cubeArrangementModels[20].Forgatasok[5] = true;
                    
                    temp = cubeArrangementModels[8];
                    cubeArrangementModels[8] = cubeArrangementModels[26];
                    cubeArrangementModels[26] = cubeArrangementModels[20];
                    cubeArrangementModels[20] = cubeArrangementModels[2];
                    cubeArrangementModels[2] = temp;
                    
                    temp = cubeArrangementModels[17];
                    cubeArrangementModels[17] = cubeArrangementModels[23];
                    cubeArrangementModels[23] = cubeArrangementModels[11];
                    cubeArrangementModels[11] = cubeArrangementModels[5];
                    cubeArrangementModels[5] = temp;
                    break;
                
                case Key.Number3:
                    cubeArrangementModels[0].Forgatasok[0] = true;
                    cubeArrangementModels[1].Forgatasok[0] = true;
                    cubeArrangementModels[2].Forgatasok[0] = true;
                    cubeArrangementModels[3].Forgatasok[0] = true;
                    cubeArrangementModels[4].Forgatasok[0] = true;
                    cubeArrangementModels[5].Forgatasok[0] = true;
                    cubeArrangementModels[6].Forgatasok[0] = true;
                    cubeArrangementModels[7].Forgatasok[0] = true;
                    cubeArrangementModels[8].Forgatasok[0] = true;

                    temp = cubeArrangementModels[0];
                    cubeArrangementModels[0] = cubeArrangementModels[2];
                    cubeArrangementModels[2] = cubeArrangementModels[8];
                    cubeArrangementModels[8] = cubeArrangementModels[6];
                    cubeArrangementModels[6] = temp;
                    
                    temp = cubeArrangementModels[1];
                    cubeArrangementModels[1] = cubeArrangementModels[5];
                    cubeArrangementModels[5] = cubeArrangementModels[7];
                    cubeArrangementModels[7] = cubeArrangementModels[3];
                    cubeArrangementModels[3] = temp;
                    
                    break;
                
                case Key.Keypad3:
                    cubeArrangementModels[0].Forgatasok[1] = true;
                    cubeArrangementModels[1].Forgatasok[1] = true;
                    cubeArrangementModels[2].Forgatasok[1] = true;
                    cubeArrangementModels[3].Forgatasok[1]= true;
                    cubeArrangementModels[4].Forgatasok[1] = true;
                    cubeArrangementModels[5].Forgatasok[1] = true;
                    cubeArrangementModels[6].Forgatasok[1] = true;
                    cubeArrangementModels[7].Forgatasok[1] = true;
                    cubeArrangementModels[8].Forgatasok[1] = true;
                    break;
                
                case Key.Number4:
                    cubeArrangementModels[18].Forgatasok[0] = true;
                    cubeArrangementModels[19].Forgatasok[0] = true;
                    cubeArrangementModels[20].Forgatasok[0] = true;
                    cubeArrangementModels[21].Forgatasok[0]= true;
                    cubeArrangementModels[22].Forgatasok[0] = true;
                    cubeArrangementModels[23].Forgatasok[0] = true;
                    cubeArrangementModels[24].Forgatasok[0] = true;
                    cubeArrangementModels[25].Forgatasok[0] = true;
                    cubeArrangementModels[26].Forgatasok[0] = true;
                    break;
                
                case Key.Keypad4:
                    cubeArrangementModels[18].Forgatasok[1] = true;
                    cubeArrangementModels[19].Forgatasok[1] = true;
                    cubeArrangementModels[20].Forgatasok[1] = true;
                    cubeArrangementModels[21].Forgatasok[1]= true;
                    cubeArrangementModels[22].Forgatasok[1] = true;
                    cubeArrangementModels[23].Forgatasok[1] = true;
                    cubeArrangementModels[24].Forgatasok[1] = true;
                    cubeArrangementModels[25].Forgatasok[1] = true;
                    cubeArrangementModels[26].Forgatasok[1] = true;
                    break;
                
                case Key.Number5:
                    cubeArrangementModels[0].Forgatasok[2] = true;
                    cubeArrangementModels[1].Forgatasok[2] = true;
                    cubeArrangementModels[2].Forgatasok[2] = true;
                    cubeArrangementModels[9].Forgatasok[2] = true;
                    cubeArrangementModels[10].Forgatasok[2] = true;
                    cubeArrangementModels[11].Forgatasok[2] = true;
                    cubeArrangementModels[18].Forgatasok[2] = true;
                    cubeArrangementModels[19].Forgatasok[2] = true;
                    cubeArrangementModels[20].Forgatasok[2] = true;
                    break;
                
                case Key.Keypad5:
                    cubeArrangementModels[0].Forgatasok[3] = true;
                    cubeArrangementModels[1].Forgatasok[3] = true;
                    cubeArrangementModels[2].Forgatasok[3] = true;
                    cubeArrangementModels[9].Forgatasok[3] = true;
                    cubeArrangementModels[10].Forgatasok[3] = true;
                    cubeArrangementModels[11].Forgatasok[3] = true;
                    cubeArrangementModels[18].Forgatasok[3] = true;
                    cubeArrangementModels[19].Forgatasok[3] = true;
                    cubeArrangementModels[20].Forgatasok[3] = true;
                    break;
                
                case Key.Number6:
                    cubeArrangementModels[0].Forgatasok[4] = true;
                    cubeArrangementModels[3].Forgatasok[4] = true;
                    cubeArrangementModels[6].Forgatasok[4] = true;
                    cubeArrangementModels[9].Forgatasok[4] = true;
                    cubeArrangementModels[12].Forgatasok[4] = true;
                    cubeArrangementModels[15].Forgatasok[4] = true;
                    cubeArrangementModels[18].Forgatasok[4] = true;
                    cubeArrangementModels[21].Forgatasok[4] = true;
                    cubeArrangementModels[24].Forgatasok[4] = true;
                    break;
                
                case Key.Keypad6:
                    cubeArrangementModels[0].Forgatasok[5] = true;
                    cubeArrangementModels[3].Forgatasok[5] = true;
                    cubeArrangementModels[6].Forgatasok[5] = true;
                    cubeArrangementModels[9].Forgatasok[5] = true;
                    cubeArrangementModels[12].Forgatasok[5] = true;
                    cubeArrangementModels[15].Forgatasok[5] = true;
                    cubeArrangementModels[18].Forgatasok[5] = true;
                    cubeArrangementModels[21].Forgatasok[5] = true;
                    cubeArrangementModels[24].Forgatasok[5] = true;
                    break;
            }
        }

        private static void GraphicWindow_Update(double deltaTime)
        {
            // NO OpenGL
            // make it threadsafe
            for (int i=0; i < cubeArrangementModels.Length; i++)
                cubeArrangementModels[i].AdvanceTime(deltaTime);
        }

        private static unsafe void GraphicWindow_Render(double deltaTime)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.Clear(ClearBufferMask.DepthBufferBit);

            Gl.UseProgram(program);

            var viewMatrix = Matrix4X4.CreateLookAt(camera.Position, camera.Target, camera.UpVector);
            SetMatrix(viewMatrix, ViewMatrixVariableName);

            var projectionMatrix = Matrix4X4.CreatePerspectiveFieldOfView<float>((float)(Math.PI / 2), 1024f / 768f, 0.1f, 100f);
            SetMatrix(projectionMatrix, ProjectionMatrixVariableName);
            
            Matrix4X4<float> trans = new Matrix4X4<float>();
            
            int index = 0;
            float[] var = new float[3];
            for (int i=-1;i<=1;i++)
                for (int j=-1;j<=1;j++)
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == -1)
                            var[0] = -0.1f;
                        else if (i == 1)
                            var[0] = 0.1f;
                        else
                            var[0] = 0;
                        if (j == -1)
                            var[1] = -0.1f;
                        else if (j == 1)
                            var[1] = 0.1f;
                        else
                            var[1] = 0;
                        if (k == -1)
                            var[2] = -0.1f;
                        else if (k == 1)
                            var[2] = 0.1f;
                        else
                            var[2] = 0;
                        cubeArrangementModels[index].index = index;
                        trans = Matrix4X4.CreateTranslation((float)i + var[0], (float)j + var[1], (float)k + var[2]);
                        var tempY = Matrix4X4.CreateRotationY((float)cubeArrangementModels[index].DiamondCubeGlobalYAngle);
                        var tempZ = Matrix4X4.CreateRotationZ((float)cubeArrangementModels[index].DiamondCubeGlobalZAngle);
                        var tempX = Matrix4X4.CreateRotationX((float)cubeArrangementModels[index].DiamondCubeGlobalXAngle);
                        var temp = Matrix4X4<float>.Identity;
                        temp *= trans;
                        temp *= tempY;
                        temp *= tempX;
                        temp *= tempZ;
                        SetMatrix(temp, ModelMatrixVariableName);
                        
                        DrawModelObject(cubes[index]);
                        index++;
                    }
        }

        private static unsafe void DrawModelObject(ModelObjectDescriptor modelObject)
        {
            Gl.BindVertexArray(modelObject.Vao);
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, modelObject.Indices);
            Gl.DrawElements(PrimitiveType.Triangles, modelObject.IndexArrayLength, DrawElementsType.UnsignedInt, null);
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);
            Gl.BindVertexArray(0);
        }

        private static unsafe void SetMatrix(Matrix4X4<float> mx, string uniformName)
        {
            int location = Gl.GetUniformLocation(program, uniformName);
            if (location == -1)
            {
                throw new Exception($"{ViewMatrixVariableName} uniform not found on shader.");
            }

            Gl.UniformMatrix4(location, 1, false, (float*)&mx);
            CheckError();
        }

        public static void CheckError()
        {
            var error = (ErrorCode)Gl.GetError();
            if (error != ErrorCode.NoError)
                throw new Exception("GL.GetError() returned " + error.ToString());
        }
    }
}