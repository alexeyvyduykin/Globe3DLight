#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class ShaderProgram : Disposable, ICleanableObserver
    {
        private readonly ShaderObject _vertexShader;
        private readonly ShaderObject? _geometryShader;
        private readonly ShaderObject _fragmentShader;
        private readonly int _program;
        private readonly FragmentOutputs _fragmentOutputs;
        private readonly ShaderVertexAttributeCollection _vertexAttributes;
        private readonly IList<ICleanable> _dirtyUniforms;
        private readonly UniformCollection _uniforms;

        public ShaderProgram(string vertexShaderSource, string fragmentShaderSource) :
            this(vertexShaderSource, string.Empty, fragmentShaderSource)
        {
        }

        public ShaderProgram(string vertexShaderSource, string geometryShaderSource, string fragmentShaderSource)
        {
            _vertexShader = new ShaderObject(vertexShaderSource, A.ShaderType.VertexShader);
            if (string.IsNullOrEmpty(geometryShaderSource) == false)
            {
                _geometryShader = new ShaderObject(geometryShaderSource, A.ShaderType.GeometryShader);
            }
            _fragmentShader = new ShaderObject(fragmentShaderSource, A.ShaderType.FragmentShader);

            _program = A.GL.CreateProgram();
            A.GL.AttachShader(_program, _vertexShader.Handle);

            if (_geometryShader is not null)
            {
                A.GL.AttachShader(_program, _geometryShader.Handle);
            }
            A.GL.AttachShader(_program, _fragmentShader.Handle);

            A.GL.LinkProgram(_program);

            A.GL.GetProgram(_program, A.GetProgramParameterName.LinkStatus, out int linkStatus);

            if (linkStatus == 0)
                throw new Exception("Could not link shader program. Link Log: /n/n" + ProgramInfoLog);

            _fragmentOutputs = new FragmentOutputs(_program);
            _vertexAttributes = FindVertexAttributes(_program);
            _dirtyUniforms = new List<ICleanable>();
            _uniforms = FindUniforms(_program);
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

        public int Handle => _program;         

        public void Bind()
        {
            A.GL.UseProgram(_program);
        }

        public void Clean(Context context, DrawState drawState/*, SceneState sceneState*/)
        {
            //SetDrawAutomaticUniforms(context, drawState, sceneState);

            for (int i = 0; i < _dirtyUniforms.Count; ++i)
            {
                _dirtyUniforms[i].Clean();
            }

            _dirtyUniforms.Clear();
        }

        private string ProgramInfoLog => A.GL.GetProgramInfoLog(_program);         

        public static void UnBind()
        {
            A.GL.UseProgram(0);
        }

        public FragmentOutputs FragmentOutputs => _fragmentOutputs; 
        
        public ShaderVertexAttributeCollection VertexAttributes => _vertexAttributes;         

        public UniformCollection Uniforms => _uniforms;        

        public void SetUniform(string name, vec2 vector)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform2(loc, 1, vector.Values);
        }

        public void SetUniform(string name, dvec2 vector)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform2(loc, 1, ((vec2)vector).Values);
        }

        public void SetUniform(string name, vec3 vector)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform3(loc, 1, vector.Values);
        }

        public void SetUniform(string name, dvec3 vector)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform3(loc, 1, ((vec3)vector).Values);
        }

        public void SetUniform(string name, vec4 vector)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform4(loc, 1, vector.Values);
        }

        //public void SetUniform(string name, dvec4 vector)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.Uniform4(loc, 1, ((vec4)vector).Values);
        //}

        public void SetUniform(string name, float value)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform1(loc, value);
        }

        //public void SetUniform(string name, double value)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.Uniform1(loc, (float)value);
        //}

        public void SetUniform(string name, mat3 matrix)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.UniformMatrix3(loc, 1, false, matrix.Values1D);
        }

        //public void SetUniform(string name, dmat3 matrix)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.UniformMatrix3(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        //}

        public void SetUniform(string name, mat4 matrix)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.UniformMatrix4(loc, 1, false, matrix.Values1D);
        }

        public void SetUniform(string name, dmat4 matrix)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.UniformMatrix4(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        }

        public void SetUniform(string name, mat4x2 matrix)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.UniformMatrix4x2(loc, 1, false, matrix.Values1D);
        }

        //public void SetUniform(string name, dmat4x2 matrix)
        //{
        //    int loc = GL.GetUniformLocation(program, name);
        //    GL.UniformMatrix4x2(loc, 1, false, Array.ConvertAll(matrix.Values1D, x => (float)x));
        //}

        public void SetUniform(string name, int value)
        {
            int loc = A.GL.GetUniformLocation(_program, name);
            A.GL.Uniform1(loc, value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_program != 0)
                {
                    A.GL.DeleteProgram(_program);
                }
                _vertexShader.Dispose();
                if (_geometryShader != null)
                    _geometryShader.Dispose();
                _fragmentShader.Dispose();
            }

            base.Dispose(disposing);
        }

        public void NotifyDirty(ICleanable value)
        {
            _dirtyUniforms.Add(value);
        }
    }
}
