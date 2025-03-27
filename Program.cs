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

        private static CubeArrangementModel[] pointerek = new CubeArrangementModel[27];

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
            
            for (int i = 0; i < 27; i++)
                pointerek[i] = cubeArrangementModels[i];

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
            CubeArrangementModel temp;
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
                    pointerek[6].Forgatasok[2] = true;
                    pointerek[7].Forgatasok[2] = true;
                    pointerek[8].Forgatasok[2] = true;
                    pointerek[15].Forgatasok[2] = true;
                    pointerek[16].Forgatasok[2] = true;
                    pointerek[17].Forgatasok[2]= true;
                    pointerek[24].Forgatasok[2] = true;
                    pointerek[25].Forgatasok[2] = true;
                    pointerek[26].Forgatasok[2] = true;

                    temp = pointerek[6];
                    pointerek[6] = pointerek[24];
                    pointerek[24] = pointerek[26];
                    pointerek[26] = pointerek[8];
                    pointerek[8] = temp;
                    
                    temp = pointerek[15];
                    pointerek[15] = pointerek[25];
                    pointerek[25] = pointerek[17];
                    pointerek[17] = pointerek[7];
                    pointerek[7] = temp;
                    break;
                
                case Key.Keypad1:
                    pointerek[6].Forgatasok[3] = true;
                    pointerek[7].Forgatasok[3] = true;
                    pointerek[8].Forgatasok[3] = true;
                    pointerek[15].Forgatasok[3] = true;
                    pointerek[16].Forgatasok[3] = true;
                    pointerek[17].Forgatasok[3]= true;
                    pointerek[24].Forgatasok[3] = true;
                    pointerek[25].Forgatasok[3] = true;
                    pointerek[26].Forgatasok[3] = true;
                    
                    temp = pointerek[6];
                    pointerek[6] = pointerek[8];
                    pointerek[8] = pointerek[26];
                    pointerek[26] = pointerek[24];
                    pointerek[24] = temp;
                    
                    temp = pointerek[15];
                    pointerek[15] = pointerek[7];
                    pointerek[7] = pointerek[17];
                    pointerek[17] = pointerek[25];
                    pointerek[25] = temp;
                    
                    break;
                    
                case Key.Number2:
                    pointerek[8].Forgatasok[4] = true;
                    pointerek[5].Forgatasok[4] = true;
                    pointerek[2].Forgatasok[4] = true;
                    pointerek[17].Forgatasok[4]= true;
                    pointerek[14].Forgatasok[4] = true;
                    pointerek[11].Forgatasok[4] = true;
                    pointerek[26].Forgatasok[4] = true;
                    pointerek[23].Forgatasok[4] = true;
                    pointerek[20].Forgatasok[4] = true;
                    
                    temp = pointerek[8];
                    pointerek[8] = pointerek[26];
                    pointerek[26] = pointerek[20];
                    pointerek[20] = pointerek[2];
                    pointerek[2] = temp;
                    
                    temp = pointerek[17];
                    pointerek[17] = pointerek[23];
                    pointerek[23] = pointerek[11];
                    pointerek[11] = pointerek[5];
                    pointerek[5] = temp;
                    break;
                
                case Key.Keypad2:
                    pointerek[8].Forgatasok[5] = true;
                    pointerek[5].Forgatasok[5] = true;
                    pointerek[2].Forgatasok[5] = true;
                    pointerek[17].Forgatasok[5]= true;
                    pointerek[14].Forgatasok[5] = true;
                    pointerek[11].Forgatasok[5] = true;
                    pointerek[26].Forgatasok[5] = true;
                    pointerek[23].Forgatasok[5] = true;
                    pointerek[20].Forgatasok[5] = true;
                    
                    temp = pointerek[8];
                    pointerek[8] = pointerek[2];
                    pointerek[2] = pointerek[20];
                    pointerek[20] = pointerek[26];
                    pointerek[26] = temp;
                    
                    temp = pointerek[17];
                    pointerek[17] = pointerek[5];
                    pointerek[5] = pointerek[11];
                    pointerek[11] = pointerek[23];
                    pointerek[23] = temp;
                    break;
                
                case Key.Number3:
                    pointerek[0].Forgatasok[0] = true;
                    pointerek[1].Forgatasok[0] = true;
                    pointerek[2].Forgatasok[0] = true;
                    pointerek[3].Forgatasok[0] = true;
                    pointerek[4].Forgatasok[0] = true;
                    pointerek[5].Forgatasok[0] = true;
                    pointerek[6].Forgatasok[0] = true;
                    pointerek[7].Forgatasok[0] = true;
                    pointerek[8].Forgatasok[0] = true;

                    temp = pointerek[0];
                    pointerek[0] = pointerek[2];
                    pointerek[2] = pointerek[8];
                    pointerek[8] = pointerek[6];
                    pointerek[6] = temp;
                    
                    temp = pointerek[1];
                    pointerek[1] = pointerek[5];
                    pointerek[5] = pointerek[7];
                    pointerek[7] = pointerek[3];
                    pointerek[3] = temp;
                    
                    break;
                
                case Key.Keypad3:
                    pointerek[0].Forgatasok[1] = true;
                    pointerek[1].Forgatasok[1] = true;
                    pointerek[2].Forgatasok[1] = true;
                    pointerek[3].Forgatasok[1]= true;
                    pointerek[4].Forgatasok[1] = true;
                    pointerek[5].Forgatasok[1] = true;
                    pointerek[6].Forgatasok[1] = true;
                    pointerek[7].Forgatasok[1] = true;
                    pointerek[8].Forgatasok[1] = true;
                
                    temp = pointerek[0];
                    pointerek[0] = pointerek[6];
                    pointerek[6] = pointerek[8];
                    pointerek[8] = pointerek[2];
                    pointerek[2] = temp;
                    
                    temp = pointerek[1];
                    pointerek[1] = pointerek[3];
                    pointerek[3] = pointerek[7];
                    pointerek[7] = pointerek[5];
                    pointerek[5] = temp;
                    break;
                
                case Key.Number4:
                    pointerek[18].Forgatasok[0] = true;
                    pointerek[19].Forgatasok[0] = true;
                    pointerek[20].Forgatasok[0] = true;
                    pointerek[21].Forgatasok[0] = true;
                    pointerek[22].Forgatasok[0] = true;
                    pointerek[23].Forgatasok[0] = true;
                    pointerek[24].Forgatasok[0] = true;
                    pointerek[25].Forgatasok[0] = true;
                    pointerek[26].Forgatasok[0] = true;
                    
                    temp = pointerek[18];
                    pointerek[18] = pointerek[22];
                    pointerek[22] = pointerek[24];
                    pointerek[24] = pointerek[20];
                    pointerek[20] = temp;
                    
                    temp = pointerek[25];
                    pointerek[25] = pointerek[23];
                    pointerek[23] = pointerek[17];
                    pointerek[17] = pointerek[19];
                    pointerek[19] = temp;
                    break;
                
                case Key.Keypad4:
                    pointerek[18].Forgatasok[1] = true;
                    pointerek[19].Forgatasok[1] = true;
                    pointerek[20].Forgatasok[1] = true;
                    pointerek[21].Forgatasok[1] = true;
                    pointerek[22].Forgatasok[1] = true;
                    pointerek[23].Forgatasok[1] = true;
                    pointerek[24].Forgatasok[1] = true;
                    pointerek[25].Forgatasok[1] = true;
                    pointerek[26].Forgatasok[1] = true;
                    
                    temp = pointerek[18];
                    pointerek[18] = pointerek[20];
                    pointerek[20] = pointerek[24];
                    pointerek[24] = pointerek[22];
                    pointerek[22] = temp;
                    
                    temp = pointerek[25];
                    pointerek[25] = pointerek[19];
                    pointerek[19] = pointerek[17];
                    pointerek[17] = pointerek[23];
                    pointerek[23] = temp;
                    break;
                
                case Key.Number5:
                    pointerek[0].Forgatasok[2] = true;
                    pointerek[1].Forgatasok[2] = true;
                    pointerek[2].Forgatasok[2] = true;
                    pointerek[9].Forgatasok[2] = true;
                    pointerek[10].Forgatasok[2] = true;
                    pointerek[11].Forgatasok[2] = true;
                    pointerek[18].Forgatasok[2] = true;
                    pointerek[19].Forgatasok[2] = true;
                    pointerek[20].Forgatasok[2] = true;

                    temp = pointerek[0];
                    pointerek[0] = pointerek[18];
                    pointerek[18] = pointerek[20];
                    pointerek[20] = pointerek[2];
                    pointerek[2] = temp;
                    
                    temp = pointerek[1];
                    pointerek[1] = pointerek[9];
                    pointerek[9] = pointerek[19];
                    pointerek[19] = pointerek[11];
                    pointerek[11] = temp;
                    break;
                
                case Key.Keypad5:
                    pointerek[0].Forgatasok[3] = true;
                    pointerek[1].Forgatasok[3] = true;
                    pointerek[2].Forgatasok[3] = true;
                    pointerek[9].Forgatasok[3] = true;
                    pointerek[10].Forgatasok[3] = true;
                    pointerek[11].Forgatasok[3] = true;
                    pointerek[18].Forgatasok[3] = true;
                    pointerek[19].Forgatasok[3] = true;
                    pointerek[20].Forgatasok[3] = true;
                    
                    temp = pointerek[0];
                    pointerek[0] = pointerek[2];
                    pointerek[2] = pointerek[20];
                    pointerek[20] = pointerek[18];
                    pointerek[18] = temp;
                    
                    temp = pointerek[1];
                    pointerek[1] = pointerek[11];
                    pointerek[11] = pointerek[19];
                    pointerek[19] = pointerek[9];
                    pointerek[9] = temp;
                    break;
                
                case Key.Number6:
                    pointerek[0].Forgatasok[4] = true;
                    pointerek[3].Forgatasok[4] = true;
                    pointerek[6].Forgatasok[4] = true;
                    pointerek[9].Forgatasok[4] = true;
                    pointerek[12].Forgatasok[4] = true;
                    pointerek[15].Forgatasok[4] = true;
                    pointerek[18].Forgatasok[4] = true;
                    pointerek[21].Forgatasok[4] = true;
                    pointerek[24].Forgatasok[4] = true;

                    temp = pointerek[6];
                    pointerek[6] = pointerek[24];
                    pointerek[24] = pointerek[18];
                    pointerek[18] = pointerek[0];
                    pointerek[0] = temp;
                    
                    temp = pointerek[3];
                    pointerek[3] = pointerek[15];
                    pointerek[15] = pointerek[21];
                    pointerek[21] = pointerek[9];
                    pointerek[9] = temp;
                    break;
                
                case Key.Keypad6:
                    pointerek[0].Forgatasok[5] = true;
                    pointerek[3].Forgatasok[5] = true;
                    pointerek[6].Forgatasok[5] = true;
                    pointerek[9].Forgatasok[5] = true;
                    pointerek[12].Forgatasok[5] = true;
                    pointerek[15].Forgatasok[5] = true;
                    pointerek[18].Forgatasok[5] = true;
                    pointerek[21].Forgatasok[5] = true;
                    pointerek[24].Forgatasok[5] = true;
                    
                    temp = pointerek[6];
                    pointerek[6] = pointerek[0];
                    pointerek[0] = pointerek[18];
                    pointerek[18] = pointerek[24];
                    pointerek[24] = temp;
                    
                    temp = pointerek[3];
                    pointerek[3] = pointerek[9];
                    pointerek[9] = pointerek[21];
                    pointerek[21] = pointerek[15];
                    pointerek[15] = temp;
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