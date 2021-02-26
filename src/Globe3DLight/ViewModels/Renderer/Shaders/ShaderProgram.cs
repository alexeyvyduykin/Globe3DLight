using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Renderer
{
    //public class ShaderProgram : Disposable, ICleanableObserver
    //{
    //    public ShaderProgram(string vertexShaderSource, string fragmentShaderSource) :
    //        this(vertexShaderSource, string.Empty, fragmentShaderSource)
    //    {
    //    }

    //    public ShaderProgram(string vertexShaderSource, string geometryShaderSource, string fragmentShaderSource)
    //    {
    //        vertexShader = new ShaderObject(vertexShaderSource, ShaderType.VertexShader);
    //        if (geometryShaderSource.Length > 0)
    //            geometryShader = new ShaderObject(geometryShaderSource, ShaderType.GeometryShader);
    //        fragmentShader = new ShaderObject(fragmentShaderSource, ShaderType.FragmentShader);

    //        program = GL.CreateProgram();
    //        GL.AttachShader(program, vertexShader.Handle);

    //        if (geometryShaderSource.Length > 0)
    //            GL.AttachShader(program, geometryShader.Handle);
    //        GL.AttachShader(program, fragmentShader.Handle);

    //        GL.LinkProgram(program);

    //        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int linkStatus);

    //        if (linkStatus == 0)
    //            throw new Exception("Could not link shader program. Link Log: /n/n" + ProgramInfoLog);

    //        fragmentOutputs = new FragmentOutputs(program);
    //        vertexAttributes = FindVertexAttributes(program);
    //        dirtyUniforms = new List<ICleanable>();
    //        uniforms = FindUniforms(program);
    //    }

    //    private static ShaderVertexAttributeCollection FindVertexAttributes(int program)
    //    {
    //        int programHandle = program;

    //        GL.GetProgram(programHandle, GetProgramParameterName.ActiveAttributes, out var numberOfAttributes);

    //        GL.GetProgram(programHandle, GetProgramParameterName.ActiveAttributeMaxLength, out var attributeNameMaxLength);

    //        ShaderVertexAttributeCollection vertexAttributes = new ShaderVertexAttributeCollection();
    //        for (int i = 0; i < numberOfAttributes; ++i)
    //        {
    //            StringBuilder attributeNameBuilder = new StringBuilder(attributeNameMaxLength);

    //            GL.GetActiveAttrib(programHandle, i, attributeNameMaxLength,
    //                out var attributeNameLength, out var attributeLength, out var attributeType, attributeNameBuilder);

    //            string attributeName = attributeNameBuilder.ToString();

    //            if (attributeName.StartsWith("gl_", StringComparison.InvariantCulture))
    //            {
    //                //
    //                // Names starting with the reserved prefix of "gl_" have a location of -1.
    //                //
    //                continue;
    //            }

    //            int attributeLocation = GL.GetAttribLocation(programHandle, attributeName);

    //            vertexAttributes.Add(new ShaderVertexAttribute(
    //                attributeName, attributeLocation, attributeType, attributeLength));
    //        }

    //        return vertexAttributes;
    //    }

    //    private UniformCollection FindUniforms(int program)
    //    {
    //        int programHandle = program;

    //        GL.GetProgram(programHandle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);

    //        GL.GetProgram(programHandle, GetProgramParameterName.ActiveUniformMaxLength, out int uniformNameMaxLength);

    //        UniformCollection uniforms = new UniformCollection();
    //        for (int i = 0; i < numberOfUniforms; ++i)
    //        {
    //            StringBuilder uniformNameBuilder = new StringBuilder(uniformNameMaxLength);

    //            GL.GetActiveUniform(programHandle, i, uniformNameMaxLength,
    //                out var uniformNameLength, out var uniformSize, out var uniformType, uniformNameBuilder);

    //            string uniformName = uniformNameBuilder.ToString();

    //            if (uniformName.StartsWith("gl_", StringComparison.InvariantCulture))
    //            {
    //                //
    //                // Names starting with the reserved prefix of "gl_" have a location of -1.
    //                //
    //                continue;
    //            }

    //            //
    //            // Skip uniforms in a named block
    //            //          
    //            GL.GetActiveUniforms(programHandle, 1, ref i, ActiveUniformParameter.UniformBlockIndex, out int uniformBlockIndex);
    //            if (uniformBlockIndex != -1)
    //            {
    //                continue;
    //            }

    //            if (uniformSize != 1)
    //            {
    //                // TODO:  Support arrays
    //                throw new NotSupportedException("Uniform arrays are not supported.");
    //            }

    //            int uniformLocation = GL.GetUniformLocation(programHandle, uniformName);
    //            uniforms.Add(CreateUniform(uniformName, uniformLocation, uniformType));
    //        }

    //        return uniforms;
    //    }

    //    private Uniform CreateUniform(string name, int location, ActiveUniformType type)
    //    {
    //        switch (type)
    //        {
    //            case ActiveUniformType.Float:
    //                return new UniformFloat(name, location, this);
    //            case ActiveUniformType.FloatVec2:
    //                return new UniformFloatVector2(name, location, this);
    //            case ActiveUniformType.FloatVec3:
    //                return new UniformFloatVector3(name, location, this);
    //            case ActiveUniformType.FloatVec4:
    //                return new UniformFloatVector4(name, location, this);
    //            case ActiveUniformType.Int:
    //                return new UniformInt(name, location, ActiveUniformType.Int, this);
    //            case ActiveUniformType.Bool:
    //                return new UniformBool(name, location, this);
    //            case ActiveUniformType.FloatMat3:
    //                return new UniformFloatMatrix33(name, location, this);
    //            case ActiveUniformType.FloatMat4:
    //                return new UniformFloatMatrix44(name, location, this);
    //            case ActiveUniformType.FloatMat4x2:
    //                return new UniformFloatMatrix42(name, location, this);
    //            case ActiveUniformType.Sampler1D:
    //            case ActiveUniformType.Sampler2D:
    //            case ActiveUniformType.Sampler2DRect:
    //            case ActiveUniformType.Sampler2DRectShadow:
    //            case ActiveUniformType.Sampler3D:
    //            case ActiveUniformType.SamplerCube:
    //            case ActiveUniformType.Sampler1DShadow:
    //            case ActiveUniformType.Sampler2DShadow:
    //            case ActiveUniformType.Sampler1DArray:
    //            case ActiveUniformType.Sampler2DArray:
    //            case ActiveUniformType.Sampler1DArrayShadow:
    //            case ActiveUniformType.Sampler2DArrayShadow:
    //            case ActiveUniformType.SamplerCubeShadow:
    //            case ActiveUniformType.IntSampler1D:
    //            case ActiveUniformType.IntSampler2D:
    //            case ActiveUniformType.IntSampler2DRect:
    //            case ActiveUniformType.IntSampler3D:
    //            case ActiveUniformType.IntSamplerCube:
    //            case ActiveUniformType.IntSampler1DArray:
    //            case ActiveUniformType.IntSampler2DArray:
    //            case ActiveUniformType.UnsignedIntSampler1D:
    //            case ActiveUniformType.UnsignedIntSampler2D:
    //            case ActiveUniformType.UnsignedIntSampler2DRect:
    //            case ActiveUniformType.UnsignedIntSampler3D:
    //            case ActiveUniformType.UnsignedIntSamplerCube:
    //            case ActiveUniformType.UnsignedIntSampler1DArray:
    //            case ActiveUniformType.UnsignedIntSampler2DArray:
    //                return new UniformInt(name, location, type, this);
    //        }

    //        //
    //        // A new Uniform derived class needs to be added to support this uniform type.
    //        //
    //        throw new NotSupportedException("An implementation for uniform type " + type.ToString() + " does not exist.");
    //    }

    //    public int Handle
    //    {
    //        get { return program; }
    //    }

    //    public void Bind()
    //    {
    //        GL.UseProgram(program);
    //    }

    //    public void Clean(Context context, DrawState drawState/*, SceneState sceneState*/)
    //    {
    //        //SetDrawAutomaticUniforms(context, drawState, sceneState);

    //        for (int i = 0; i < dirtyUniforms.Count; ++i)
    //        {
    //            dirtyUniforms[i].Clean();
    //        }
    //        dirtyUniforms.Clear();
    //    }

    //    private string ProgramInfoLog
    //    {
    //        get { return GL.GetProgramInfoLog(program); }
    //    }

    //    public static void UnBind()
    //    {
    //        GL.UseProgram(0);
    //    }

    //    public FragmentOutputs FragmentOutputs
    //    {
    //        get { return fragmentOutputs; }
    //    }

    //    public ShaderVertexAttributeCollection VertexAttributes
    //    {
    //        get { return vertexAttributes; }
    //    }

    //    public UniformCollection Uniforms
    //    {
    //        get { return uniforms; }
    //    }

    //    #region Uniforms DataTypes

    //    public void SetUniform(string name, vec2 vector)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform2(loc, 1, vector.Values);
    //    }

    //    public void SetUniform(string name, dvec2 vector)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform2(loc, 1, ((vec2)vector).Values);
    //    }

    //    public void SetUniform(string name, vec3 vector)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform3(loc, 1, vector.Values);
    //    }

    //    public void SetUniform(string name, dvec3 vector)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform3(loc, 1, ((vec3)vector).Values);
    //    }

    //    public void SetUniform(string name, vec4 vector)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform4(loc, 1, vector.Values);
    //    }

    //    //public void SetUniform(string name, dvec4 vector)
    //    //{
    //    //    int loc = GL.GetUniformLocation(program, name);
    //    //    GL.Uniform4(loc, 1, ((vec4)vector).Values);
    //    //}

    //    public void SetUniform(string name, float value)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform1(loc, value);
    //    }

    //    //public void SetUniform(string name, double value)
    //    //{
    //    //    int loc = GL.GetUniformLocation(program, name);
    //    //    GL.Uniform1(loc, (float)value);
    //    //}

    //    public void SetUniform(string name, mat3 matrix)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.UniformMatrix3(loc, 1, false, matrix.Values1D);
    //    }

    //    //public void SetUniform(string name, dmat3 matrix)
    //    //{
    //    //    int loc = GL.GetUniformLocation(program, name);
    //    //    GL.UniformMatrix3(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
    //    //}

    //    public void SetUniform(string name, mat4 matrix)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.UniformMatrix4(loc, 1, false, matrix.Values1D);
    //    }

    //    public void SetUniform(string name, dmat4 matrix)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.UniformMatrix4(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
    //    }

    //    public void SetUniform(string name, mat4x2 matrix)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.UniformMatrix4x2(loc, 1, false, matrix.Values1D);
    //    }

    //    //public void SetUniform(string name, dmat4x2 matrix)
    //    //{
    //    //    int loc = GL.GetUniformLocation(program, name);
    //    //    GL.UniformMatrix4x2(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
    //    //}

    //    public void SetUniform(string name, int value)
    //    {
    //        int loc = GL.GetUniformLocation(program, name);
    //        GL.Uniform1(loc, value);
    //    }

    //    #endregion

    //    #region Disposeble Members

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (program != 0)
    //            {
    //                GL.DeleteProgram(program);
    //            }
    //            vertexShader.Dispose();
    //            if (geometryShader != null)
    //                geometryShader.Dispose();
    //            fragmentShader.Dispose();
    //        }

    //        base.Dispose(disposing);
    //    }

    //    #endregion

    //    #region ICleanableObserver Members

    //    public void NotifyDirty(ICleanable value)
    //    {
    //        dirtyUniforms.Add(value);
    //    }

    //    #endregion

    //    private readonly ShaderObject vertexShader;
    //    private readonly ShaderObject geometryShader;
    //    private readonly ShaderObject fragmentShader;

    //    private readonly int program;

    //    private readonly FragmentOutputs fragmentOutputs;
    //    private readonly ShaderVertexAttributeCollection vertexAttributes;
    //    private readonly IList<ICleanable> dirtyUniforms;
    //    private readonly UniformCollection uniforms;
    //}
}
