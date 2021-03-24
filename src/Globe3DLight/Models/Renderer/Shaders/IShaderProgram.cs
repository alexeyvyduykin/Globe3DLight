using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;


namespace Globe3DLight.Models.Renderer
{
    public interface IShaderProgram : IDisposable, ICleanableObserver
    {
        int Handle { get; }

        string VertexShaderSource { get; set; }

        string GeometryShaderSource { get; set; } 
        
        string FragmentShaderSource { get; set; }
        
        void CreateProgram();

        void Bind();

        void UnBind();

        void Clean();

        //public void Clean(Context context, DrawState drawState, SceneState sceneState)
        //{
        //    //SetDrawAutomaticUniforms(context, drawState, sceneState);

        //    for (int i = 0; i < dirtyUniforms.Count; ++i)
        //    {
        //        dirtyUniforms[i].Clean();
        //    }
        //    dirtyUniforms.Clear();
        //}


        // FragmentOutputs
        int FragmentOutputs(string index);

        void SetUniform<T>(string name, T value) where T : struct;

        void BindAttribLocation(int name, string attrib);

        KeyedCollection<string, IShaderVertexAttribute> /*ShaderVertexAttributeCollection*/ VertexAttributes { get; }


        KeyedCollection<string, IUniform>/*UniformCollection*/ Uniforms { get; }
    }
}
