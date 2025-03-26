using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GrafikaSzeminarium
{
    internal class ModelObjectDescriptor:IDisposable
    {
        private bool disposedValue;

        public uint Vao { get; private set; }
        public uint Vertices { get; private set; }
        public uint Colors { get; private set; }
        public uint Indices { get; private set; }
        public uint IndexArrayLength { get; private set; }

        private GL Gl;
        
        public unsafe static ModelObjectDescriptor CreateCube(GL Gl, int tipus)
        {
            void felso(float[] a)
            {
                for (int i = 0; i < 16; i++)
                        a[i] = 0f;
            }

            void eleje(float[] a)
            {
                for (int i = 16; i < 32; i++)
                        a[i] = 0f;
            }

            void bal(float[] a)
            {
                for (int i = 32; i < 48; i++)
                        a[i] = 0f;
            }
            
            void alja(float[] a)
            {
                for (int i = 48; i < 64; i++)
                        a[i] = 0f;
            }

            void hata(float[] a)
            {
                for (int i = 64; i < 80; i++)
                        a[i] = 0f;
            }
            
            void jobb(float[] a)
            {
                for (int i = 80; i < 96; i++)
                        a[i] = 0f;
            }

            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);

            // counter clockwise is front facing
            var vertexArray = new float[] {
                -0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, -0.5f,
                -0.5f, 0.5f, -0.5f,

                -0.5f, 0.5f, 0.5f,
                -0.5f, -0.5f, 0.5f,
                0.5f, -0.5f, 0.5f,
                0.5f, 0.5f, 0.5f,

                -0.5f, 0.5f, 0.5f,
                -0.5f, 0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, 0.5f,

                -0.5f, -0.5f, 0.5f,
                0.5f, -0.5f, 0.5f,
                0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,

                0.5f, 0.5f, -0.5f,
                -0.5f, 0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,

                0.5f, 0.5f, 0.5f,
                0.5f, 0.5f, -0.5f,
                0.5f, -0.5f, -0.5f,
                0.5f, -0.5f, 0.5f,

            };

            float[] colorArray = new float[] {
                1.0f, 0.0f, 0.0f, 1.0f, //RED TETEJE
                1.0f, 0.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 0.0f, 1.0f,

                0.0f, 1.0f, 0.0f, 1.0f, //GREEN ELEJE
                0.0f, 1.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,

                0.0f, 0.0f, 1.0f, 1.0f,//BLUE BAL
                0.0f, 0.0f, 1.0f, 1.0f,
                0.0f, 0.0f, 1.0f, 1.0f,
                0.0f, 0.0f, 1.0f, 1.0f,

                1.0f, 0.0f, 1.0f, 1.0f, //MAGENTA ROZSASZIN VALAMI ALJA
                1.0f, 0.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 1.0f,
                1.0f, 0.0f, 1.0f, 1.0f,

                0.0f, 1.0f, 1.0f, 1.0f, //CIAN HATA
                0.0f, 1.0f, 1.0f, 1.0f,
                0.0f, 1.0f, 1.0f, 1.0f,
                0.0f, 1.0f, 1.0f, 1.0f,

                1.0f, 1.0f, 0.0f, 1.0f, //YELLOW JOBB
                1.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 0.0f, 1.0f,
                1.0f, 1.0f, 0.0f, 1.0f,
            };
            switch (tipus)
            {
                case 0:
                    felso(colorArray);
                    eleje(colorArray);
                    jobb(colorArray);
                    break;
                case 1:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    felso(colorArray);
                    break;
                case 2:
                    hata(colorArray);
                    jobb(colorArray);
                    felso(colorArray);
                    break;
                case 3:
                    eleje(colorArray);
                    jobb(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 4:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 5:
                    hata(colorArray);
                    jobb(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 6:
                    eleje(colorArray);
                    jobb(colorArray);
                    alja(colorArray);
                    break;
                case 7:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    alja(colorArray);
                    break;
                case 8:
                    hata(colorArray);
                    jobb(colorArray);
                    alja(colorArray);
                    break;
                case 9:
                    eleje(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 10:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 11:
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 12:
                    eleje(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 13:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 14:
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 15:
                    eleje(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
                case 16:
                    eleje(colorArray);
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
                case 17:
                    hata(colorArray);
                    jobb(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
                case 18:
                    eleje(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 19:
                    eleje(colorArray);
                    hata(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 20:
                    hata(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    break;
                case 21:
                    eleje(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 22:
                    eleje(colorArray);
                    hata(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 23:
                    hata(colorArray);
                    bal(colorArray);
                    felso(colorArray);
                    alja(colorArray);
                    break;
                case 24:
                    eleje(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
                case 25:
                    eleje(colorArray);
                    hata(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
                case 26:
                    hata(colorArray);
                    bal(colorArray);
                    alja(colorArray);
                    break;
            }


            uint[] indexArray = new uint[] {
                0, 1, 2,
                0, 2, 3,

                4, 5, 6,
                4, 6, 7,

                8, 9, 10,
                10, 11, 8,

                12, 14, 13,
                12, 15, 14,

                17, 16, 19,
                17, 19, 18,

                20, 22, 21,
                20, 23, 22
            };

            uint vertices = Gl.GenBuffer();
            Gl.BindBuffer(GLEnum.ArrayBuffer, vertices);
            Gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)vertexArray.AsSpan(), GLEnum.StaticDraw);
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, null);
            Gl.EnableVertexAttribArray(0);
            Gl.BindBuffer(GLEnum.ArrayBuffer, 0);

            uint colors = Gl.GenBuffer();
            Gl.BindBuffer(GLEnum.ArrayBuffer, colors);
            Gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)colorArray.AsSpan(), GLEnum.StaticDraw);
            Gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, null);
            Gl.EnableVertexAttribArray(1);
            Gl.BindBuffer(GLEnum.ArrayBuffer, 0);

            uint indices = Gl.GenBuffer();
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, indices);
            Gl.BufferData(GLEnum.ElementArrayBuffer, (ReadOnlySpan<uint>)indexArray.AsSpan(), GLEnum.StaticDraw);
            Gl.BindBuffer(GLEnum.ElementArrayBuffer, 0);

            return new ModelObjectDescriptor() {Vao= vao, Vertices = vertices, Colors = colors, Indices = indices, IndexArrayLength = (uint)indexArray.Length, Gl = Gl};

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null


                // always unbound the vertex buffer first, so no halfway results are displayed by accident
                Gl.DeleteBuffer(Vertices);
                Gl.DeleteBuffer(Colors);
                Gl.DeleteBuffer(Indices);
                Gl.DeleteVertexArray(Vao);

                disposedValue = true;
            }
        }

        ~ModelObjectDescriptor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
