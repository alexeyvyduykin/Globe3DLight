using System;
using System.Collections.Generic;
using System.Text;
using A = OpenTK.Graphics.OpenGL;
using GlmSharp;
using System.Collections.ObjectModel;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class ShaderProgram : Disposable, ICleanableObserver
    {
        public ShaderProgram(string vertexShaderSource, string fragmentShaderSource) : 
            this(vertexShaderSource, string.Empty, fragmentShaderSource)
        {
        }

        public ShaderProgram(string vertexShaderSource, string geometryShaderSource, string fragmentShaderSource)
        {
            vertexShader = new ShaderObject(vertexShaderSource, A.ShaderType.VertexShader);
            if (geometryShaderSource.Length > 0)
                geometryShader = new ShaderObject(geometryShaderSource, A.ShaderType.GeometryShader);
            fragmentShader = new ShaderObject(fragmentShaderSource, A.ShaderType.FragmentShader);

            program = A.GL.CreateProgram();
            A.GL.AttachShader(program, vertexShader.Handle);
      
            if (geometryShaderSource.Length > 0)
                A.GL.AttachShader(program, geometryShader.Handle);
            A.GL.AttachShader(program, fragmentShader.Handle);

            A.GL.LinkProgram(program);

            A.GL.GetProgram(program, A.GetProgramParameterName.LinkStatus, out int linkStatus);

            if (linkStatus == 0)
                throw new Exception("Could not link shader program. Link Log: /n/n" + ProgramInfoLog);

            fragmentOutputs = new FragmentOutputs(program);
            vertexAttributes = FindVertexAttributes(program);
            dirtyUniforms = new List<ICleanable>();
            uniforms = FindUniforms(program);
        }

        private static ShaderVertexAttributeCollection FindVertexAttributes(int program)
        {
            int programHandle = program;

            A.GL.GetProgram(programHandle, A.GetProgramParameterName.ActiveAttributes, out var numberOfAttributes);

            A.GL.GetProgram(programHandle, A.GetProgramParameterName.ActiveAttributeMaxLength, out var attributeNameMaxLength);

            ShaderVertexAttributeCollection vertexAttributes = new ShaderVertexAttributeCollection();
            for (int i = 0; i < numberOfAttributes; ++i)
            {                              
                StringBuilder attributeNameBuilder = new StringBuilder(attributeNameMaxLength);

                A.GL.GetActiveAttrib(programHandle, i, attributeNameMaxLength,
                    out var attributeNameLength, out var attributeLength, out var attributeType, attributeNameBuilder);

                string attributeName = attributeNameBuilder.ToString();

                if (attributeName.StartsWith("gl_", StringComparison.InvariantCulture))
                {
                    //
                    // Names starting with the reserved prefix of "gl_" have a location of -1.
                    //
                    continue;
                }

                int attributeLocation = A.GL.GetAttribLocation(programHandle, attributeName);

                vertexAttributes.Add(new ShaderVertexAttribute(
                    attributeName, attributeLocation, attributeType, attributeLength));
            }

            return vertexAttributes;
        }

        private UniformCollection FindUniforms(int program)
        {
            int programHandle = program;

            A.GL.GetProgram(programHandle, A.GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);

            A.GL.GetProgram(programHandle, A.GetProgramParameterName.ActiveUniformMaxLength, out int uniformNameMaxLength);

            UniformCollection uniforms = new UniformCollection();
            for (int i = 0; i < numberOfUniforms; ++i)
            {                        
                StringBuilder uniformNameBuilder = new StringBuilder(uniformNameMaxLength);

                A.GL.GetActiveUniform(programHandle, i, uniformNameMaxLength,
                    out _, out var uniformSize, out var uniformType, uniformNameBuilder);

                string uniformName = uniformNameBuilder.ToString();

                if (uniformName.StartsWith("gl_", StringComparison.InvariantCulture))
                {
                    //
                    // Names starting with the reserved prefix of "gl_" have a location of -1.
                    //
                    continue;
                }

                //
                // Skip uniforms in a named block
                //          
                A.GL.GetActiveUniforms(programHandle, 1, ref i, A.ActiveUniformParameter.UniformBlockIndex, out int uniformBlockIndex);
                if (uniformBlockIndex != -1)
                {
                    continue;
                }

                if (uniformSize != 1)
                {
                    // TODO:  Support arrays
                    throw new NotSupportedException("Uniform arrays are not supported.");
                }

                int uniformLocation = A.GL.GetUniformLocation(programHandle, uniformName);
                uniforms.Add(CreateUniform(uniformName, uniformLocation, uniformType));
            }

            return uniforms;
        }

        private Uniform CreateUniform(string name, int location, A.ActiveUniformType type)
        {
            switch (type)
            {
                case A.ActiveUniformType.Float:
                    return new UniformFloat(name, location, this);
                case A.ActiveUniformType.FloatVec2:
                    return new UniformFloatVector2(name, location, this);
                case A.ActiveUniformType.FloatVec3:
                    return new UniformFloatVector3(name, location, this);
                case A.ActiveUniformType.FloatVec4:
                    return new UniformFloatVector4(name, location, this);
                case A.ActiveUniformType.Int:
                    return new UniformInt(name, location, A.ActiveUniformType.Int, this);
                case A.ActiveUniformType.Bool:
                    return new UniformBool(name, location, this);
                case A.ActiveUniformType.FloatMat3:
                    return new UniformFloatMatrix33(name, location, this);
                case A.ActiveUniformType.FloatMat4:
                    return new UniformFloatMatrix44(name, location, this);
                case A.ActiveUniformType.FloatMat4x2:
                    return new UniformFloatMatrix42(name, location, this);
                case A.ActiveUniformType.Sampler1D:
                case A.ActiveUniformType.Sampler2D:
                case A.ActiveUniformType.Sampler2DRect:
                case A.ActiveUniformType.Sampler2DRectShadow:
                case A.ActiveUniformType.Sampler3D:
                case A.ActiveUniformType.SamplerCube:
                case A.ActiveUniformType.Sampler1DShadow:
                case A.ActiveUniformType.Sampler2DShadow:
                case A.ActiveUniformType.Sampler1DArray:
                case A.ActiveUniformType.Sampler2DArray:
                case A.ActiveUniformType.Sampler1DArrayShadow:
                case A.ActiveUniformType.Sampler2DArrayShadow:
                case A.ActiveUniformType.SamplerCubeShadow:
                case A.ActiveUniformType.IntSampler1D:
                case A.ActiveUniformType.IntSampler2D:
                case A.ActiveUniformType.IntSampler2DRect:
                case A.ActiveUniformType.IntSampler3D:
                case A.ActiveUniformType.IntSamplerCube:
                case A.ActiveUniformType.IntSampler1DArray:
                case A.ActiveUniformType.IntSampler2DArray:
                case A.ActiveUniformType.UnsignedIntSampler1D:
                case A.ActiveUniformType.UnsignedIntSampler2D:
                case A.ActiveUniformType.UnsignedIntSampler2DRect:
                case A.ActiveUniformType.UnsignedIntSampler3D:
                case A.ActiveUniformType.UnsignedIntSamplerCube:
                case A.ActiveUniformType.UnsignedIntSampler1DArray:
                case A.ActiveUniformType.UnsignedIntSampler2DArray:
                    return new UniformInt(name, location, type, this);
            }

            //
            // A new Uniform derived class needs to be added to support this uniform type.
            //
            throw new NotSupportedException("An implementation for uniform type " + type.ToString() + " does not exist.");
        }

        public int Handle
        {
            get { return program; }
        }

        public void Bind()
        {
            A.GL.UseProgram(program);
        }

        public void Clean(Context context, DrawState drawState/*, SceneState sceneState*/)
        {
            //SetDrawAutomaticUniforms(context, drawState, sceneState);

            for (int i = 0; i < dirtyUniforms.Count; ++i)
            {
                dirtyUniforms[i].Clean();
            }
            dirtyUniforms.Clear();
        }

        private string ProgramInfoLog
        {
            get { return A.GL.GetProgramInfoLog(program); }
        }

        public static void UnBind()
        {
            A.GL.UseProgram(0);
        }

        public FragmentOutputs FragmentOutputs
        {
            get { return fragmentOutputs; }
        }

        public ShaderVertexAttributeCollection VertexAttributes
        {
            get { return vertexAttributes; }
        }

        public UniformCollection Uniforms
        {
            get { return uniforms; }
        }

        #region Uniforms DataTypes

        public void SetUniform(string name, vec2 vector)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform2(loc, 1, vector.Values);
        }

        public void SetUniform(string name, dvec2 vector)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform2(loc, 1, ((vec2)vector).Values);
        }

        public void SetUniform(string name, vec3 vector)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform3(loc, 1, vector.Values);
        }

        public void SetUniform(string name, dvec3 vector)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform3(loc, 1, ((vec3)vector).Values);
        }

        public void SetUniform(string name, vec4 vector)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform4(loc, 1, vector.Values);
        }

        //public void SetUniform(string name, dvec4 vector)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.Uniform4(loc, 1, ((vec4)vector).Values);
        //}

        public void SetUniform(string name, float value)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform1(loc, value);
        }

        //public void SetUniform(string name, double value)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.Uniform1(loc, (float)value);
        //}

        public void SetUniform(string name, mat3 matrix)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.UniformMatrix3(loc, 1, false, matrix.Values1D);
        }

        //public void SetUniform(string name, dmat3 matrix)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.UniformMatrix3(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        //}

        public void SetUniform(string name, mat4 matrix)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.UniformMatrix4(loc, 1, false, matrix.Values1D);
        }

        public void SetUniform(string name, dmat4 matrix)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.UniformMatrix4(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        }

        public void SetUniform(string name, mat4x2 matrix)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.UniformMatrix4x2(loc, 1, false, matrix.Values1D);
        }

        //public void SetUniform(string name, dmat4x2 matrix)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.UniformMatrix4x2(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        //}

        public void SetUniform(string name, int value)
        {
            int loc = A.GL.GetUniformLocation(program, name);
            A.GL.Uniform1(loc, value);
        }

        #endregion

        #region Disposeble Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (program != 0)
                {
                    A.GL.DeleteProgram(program);
                }
                vertexShader.Dispose();
                if (geometryShader != null)
                    geometryShader.Dispose();
                fragmentShader.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ICleanableObserver Members

        public void NotifyDirty(ICleanable value)
        {
            dirtyUniforms.Add(value);
        }

        #endregion

        private readonly ShaderObject vertexShader;
        private readonly ShaderObject geometryShader;
        private readonly ShaderObject fragmentShader;

        private readonly int program;

        private readonly FragmentOutputs fragmentOutputs;
        private readonly ShaderVertexAttributeCollection vertexAttributes;
        private readonly IList<ICleanable> dirtyUniforms;
        private readonly UniformCollection uniforms;
    }
}
