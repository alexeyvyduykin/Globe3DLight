using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Image;
using Globe3DLight.Models.Scene;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Geometry;
using B = Globe3DLight.Renderer.OpenTK.Core;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class SpaceboxDrawNode : DrawNode, ISpaceboxDrawNode
    {
        private B.Context _context;
        // private float width = 150000.0f;// 25000.0f;//200000.0f;
        private B.Device _device;
        //private int idCubemapText;
        private bool dirty;

        private readonly B.ShaderProgram sp;
        private readonly B.DrawState drawState;
        private IAMesh mesh;

        private int _spaceboxCubemapName;
        private bool _isComplete = false;
        private string _key;
        public SpaceboxRenderModel Spacebox { get; set; }

        public bool IsComplete => _isComplete;

        public string WaitKey => _key;

        private readonly string spaceboxVS = @"
#version 330
layout (location = 0) in vec3 POSITION;
uniform mat4 u_mvp;
out vec3 v_texCoords;
void main(void)
{
  gl_Position = u_mvp * vec4(POSITION, 1.0);
  v_texCoords = POSITION;
}";

        private readonly string spaceboxFS = @"
#version 330
uniform samplerCube u_spacebox;
in vec3 v_texCoords;
out vec4 color;
void main(void)
{
  color = texture(u_spacebox, v_texCoords);
}";

        public SpaceboxDrawNode(SpaceboxRenderModel spacebox)
        {
            this.Spacebox = spacebox;

            _context = new B.Context(/*null, 600, 600*/);
            _device = new B.Device();
            _key = spacebox.SpaceboxCubemapKey;

            dirty = true;

            sp = _device.CreateShaderProgram(spaceboxVS, spaceboxFS);

            drawState = new B.DrawState();
            drawState.ShaderProgram = sp;
        }

        public override void UpdateGeometry() 
        {
            if (dirty)
            {
                //SOIL soil = new SOIL();

                //   GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
                //idCubemapText = soil.SOIL_load_OGL_single_cubemap("C:/data/textures/Spacebox/Spacebox4096x4096Compressed.dds", 0, SOILFlags.DdsLoadDirect);

                //A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.ClampToEdge);
                //A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.ClampToEdge);
                //A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapR, (int)A.TextureWrapMode.ClampToEdge);

            //    mesh = new Cube((float)Spacebox.Scale);

                mesh = this.Spacebox.Mesh;

                drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                drawState.RenderState.FacetCulling.FrontFaceWindingOrder = 
                    (mesh.FrontFaceWindingOrder == FrontFaceDirection.Cw) ? A.FrontFaceDirection.Cw : A.FrontFaceDirection.Ccw;
                dirty = false;

                drawState.RenderState.DepthMask = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            if (dirty == false && _isComplete == true)
            {
                sp.Bind();

                //          GL.DepthMask(false);// Remember to turn depth writing off

                sp.SetUniform("u_mvp", scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * mat4.Identity);

                // skybox cube
                A.GL.ActiveTexture(A.TextureUnit.Texture0);
                sp.SetUniform("u_spacebox", 0);
                A.GL.BindTexture(A.TextureTarget.TextureCubeMap, _spaceboxCubemapName);

                _context.Draw(A.PrimitiveType.Triangles, drawState, scene);

                B.ShaderProgram.UnBind();


                A.GL.DepthMask(true);
            }
        }

        public int SetImage(IDdsImage image)
        {
            var class1 = new B.TextureCreator();
            _spaceboxCubemapName = class1.Create(image, 0, 1);

            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.ClampToEdge);
            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.ClampToEdge);
            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapR, (int)A.TextureWrapMode.ClampToEdge);
           
            _isComplete = true;
            
            return _spaceboxCubemapName;
        }

        public void SetName(int name)
        {
            _spaceboxCubemapName = name; 
            
            _isComplete = true;
        }
    }

    //internal class SpaceboxDrawNode : DrawNode, ISpaceboxDrawNode
    //{
    //    private float width = 25000.0f;//200000.0f;
    //    private readonly ShaderProgram _shaderProgram;
    //    //   private readonly RenderState _renderState = new RenderState();
    //    //  private VertexArray _vertexArray;
    //    //   private readonly IDevice _device;
    //    //   private A.FrontFaceDirection _frontFaceDirection;

    //    //private float width = 25000.0f;//200000.0f;

    //    private int _idCubemapText;

    //    private DrawState _drawState;
    //    private readonly Context _context;




    //    public ISpacebox Spacebox { get; set; }

    //    public SpaceboxDrawNode(ISpacebox spacebox)
    //    {
    //        this._shaderProgram = new ShaderProgram(spaceboxVS, spaceboxFS);// device.CreateShaderProgram();
    //        this.Spacebox = spacebox;


    //        _drawState = new DrawState();
    //        _drawState.ShaderProgram = _shaderProgram;

    //        _context = new Context();
    //    }

    //    public override void UpdateStyle()
    //    {
    //        base.UpdateStyle();

    //        if (Spacebox.IsTextureLoading == false)
    //        {
    //            var id = Spacebox.Loader.LoadOGLSingleCubemap(Spacebox.Filename);

    //            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.ClampToEdge);
    //            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.ClampToEdge);
    //            A.GL.TexParameter(A.TextureTarget.TextureCubeMap, A.TextureParameterName.TextureWrapR, (int)A.TextureWrapMode.ClampToEdge);

    //            Spacebox.IdCubemapText = id;
    //            this._idCubemapText = id;


    //            Spacebox.IsTextureLoading = true;
    //        }
    //    }

    //    public override void UpdateGeometry()
    //    {
    //        var mesh = new Cube(width);

    //        // var mesh = Spacebox.Mesh;

    //        _drawState.VertexArray =
    //            _context.CreateVertexArray_NEW(mesh, _drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);

    //        _drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
    //        _drawState.RenderState.FacetCulling.FrontFaceWindingOrder = mesh.FrontFaceWindingOrder;

    //        _drawState.RenderState.DepthMask = false;
    //    }

    //    public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
    //    {
    //        _shaderProgram.Bind();

    //        //          GL.DepthMask(false);// Remember to turn depth writing off

    //        _shaderProgram.SetUniform("u_mvp", scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * mat4.Identity);

    //        // skybox cube
    //        A.GL.ActiveTexture(A.TextureUnit.Texture0);
    //        _shaderProgram.SetUniform("u_spacebox", 0);
    //        A.GL.BindTexture(A.TextureTarget.TextureCubeMap, Spacebox.IdCubemapText);

    //        _context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

    //        _shaderProgram.UnBind();


    //    }

    //    //public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
    //    ////public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    //{
    //    //    _shaderProgram.Bind();

    //    //    _shaderProgram.SetUniform("u_mvp", scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * mat4.Identity);

    //    //    // skybox cube      
    //    //    _shaderProgram.SetUniform("u_spacebox", 0);

    //    //    // skybox cube
    //    //    A.GL.ActiveTexture(A.TextureUnit.Texture0);         
    //    //    A.GL.BindTexture(A.TextureTarget.TextureCubeMap, _idCubemapText);

    //    //    A.GL.CullFace(A.CullFaceMode.Front);
    //    //    A.GL.FrontFace(_frontFaceDirection);

    //    //    A.GL.DepthMask(false);

    //    //    _vertexArray.Bind();
    //    //    _vertexArray.Clean();

    //    //    _shaderProgram.Clean();

    //    //    A.GL.DrawRangeElements(A.PrimitiveType.Triangles, 0, _vertexArray.MaximumArrayIndex(),
    //    //        _vertexArray.IndexBuffer.Count, _vertexArray.IndexBuffer.Datatype.ToOpenTKType(), new IntPtr());

    //    //    _shaderProgram.UnBind();

    //    //    A.GL.DepthMask(true);      
    //    //}

    //}

}
