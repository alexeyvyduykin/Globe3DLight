using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    //internal class ShaderObject : Disposable
    //{
    //    public ShaderObject(string source, ShaderType type)
    //    {
    //        shaderObject = GL.CreateShader(type);

    //        GL.ShaderSource(shaderObject, source);
    //        GL.CompileShader(shaderObject);

    //        GL.GetShader(shaderObject, ShaderParameter.CompileStatus, out int compileStatus);

    //        if (compileStatus == 0)
    //        {
    //            throw new Exception("Could not compile shader object. Compile Log: \n\n" + CompileLog);
    //        }
    //    }

    //    private string CompileLog
    //    {
    //        get
    //        {
    //            return GL.GetShaderInfoLog(shaderObject);
    //        }
    //    }

    //    public int Handle
    //    {
    //        get { return shaderObject; }
    //    }

    //    #region Disposable Members

    //    protected override void Dispose(bool disposing)
    //    {
    //        // Всегда удалять шейдер, даже в финализации
    //        if (shaderObject != 0)
    //        {
    //            GL.DeleteShader(shaderObject);
    //            shaderObject = 0;
    //        }

    //        base.Dispose(disposing);
    //    }

    //    #endregion

    //    private int shaderObject;
    //}
}
