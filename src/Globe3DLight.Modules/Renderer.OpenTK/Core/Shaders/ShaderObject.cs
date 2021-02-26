using System;
using A = OpenTK.Graphics.OpenGL;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class ShaderObject : Disposable
    {
        public ShaderObject(string source, A.ShaderType type)
        {
            shaderObject = A.GL.CreateShader(type);

            A.GL.ShaderSource(shaderObject, source);
            A.GL.CompileShader(shaderObject);

            A.GL.GetShader(shaderObject, A.ShaderParameter.CompileStatus, out int compileStatus);

            if (compileStatus == 0)
            {
                throw new Exception("Could not compile shader object. Compile Log: \n\n" + CompileLog);
            }
        }

        private string CompileLog
        {
            get
            {
                return A.GL.GetShaderInfoLog(shaderObject);
            }
        }

        public int Handle
        {
            get { return shaderObject; }
        }

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            // Всегда удалять шейдер, даже в финализации
            if (shaderObject != 0)
            {
                A.GL.DeleteShader(shaderObject);
                shaderObject = 0;
            }

            base.Dispose(disposing);
        }

        #endregion

        private int shaderObject;
    }
}
