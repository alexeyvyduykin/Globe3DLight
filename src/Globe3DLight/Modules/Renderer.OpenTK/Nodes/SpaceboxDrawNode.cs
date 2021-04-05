#nullable disable
using GlmSharp;
using Globe3DLight.Models.Image;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Scene;
using A = OpenTK.Graphics.OpenGL;
using B = Globe3DLight.Renderer.OpenTK.Core;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class SpaceboxDrawNode : DrawNode, ISpaceboxDrawNode
    {
        private B.Context _context;
        private B.Device _device;
        private bool _dirty;
        private readonly B.ShaderProgram _sp;
        private B.DrawState _drawState;
        private int _spaceboxCubemapName;
        private bool _isComplete = false;
        private string _key;
        private readonly string _spaceboxVS = @"
#version 330
layout (location = 0) in vec3 POSITION;
uniform mat4 u_mvp;
out vec3 v_texCoords;
void main(void)
{
  gl_Position = u_mvp * vec4(POSITION, 1.0);
  v_texCoords = POSITION;
}";
        private readonly string _spaceboxFS = @"
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
            Spacebox = spacebox;

            _context = new B.Context();
            _device = new B.Device();
            _key = spacebox.SpaceboxCubemapKey;

            _dirty = true;

            _sp = _device.CreateShaderProgram(_spaceboxVS, _spaceboxFS);
        }

        public SpaceboxRenderModel Spacebox { get; set; }

        public bool IsComplete => _isComplete;

        public string WaitKey => _key;

        public override void UpdateGeometry()
        {
            if (_dirty)
            {
                var mesh = Spacebox.Mesh;

                var va = _context.CreateVertexArray(mesh, _sp.VertexAttributes, A.BufferUsageHint.StaticDraw);
                
                var state = _device.CreateRenderState();

                state.FacetCulling.Face = A.CullFaceMode.Front;
                state.FacetCulling.FrontFaceWindingOrder = A.FrontFaceDirection.Cw;

                state.DepthMask = false;

                _drawState = _device.CreateDrawState(state, _sp, va);

                _dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            if (_dirty == false && _isComplete == true)
            {
                _sp.Bind();

                //          GL.DepthMask(false);// Remember to turn depth writing off

                _sp.SetUniform("u_mvp", scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * mat4.Identity);

                // skybox cube
                A.GL.ActiveTexture(A.TextureUnit.Texture0);
                _sp.SetUniform("u_spacebox", 0);
                A.GL.BindTexture(A.TextureTarget.TextureCubeMap, _spaceboxCubemapName);

                _context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

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
}
