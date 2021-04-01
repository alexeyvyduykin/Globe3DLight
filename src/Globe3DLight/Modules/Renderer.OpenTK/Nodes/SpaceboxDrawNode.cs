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
        private bool dirty;
        private readonly B.ShaderProgram sp;
        private readonly B.DrawState drawState;
        private int _spaceboxCubemapName;
        private bool _isComplete = false;
        private string _key;
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
            Spacebox = spacebox;

            _context = new B.Context();
            _device = new B.Device();
            _key = spacebox.SpaceboxCubemapKey;

            dirty = true;

            sp = _device.CreateShaderProgram(spaceboxVS, spaceboxFS);

            drawState = new B.DrawState();
            drawState.ShaderProgram = sp;
        }

        public SpaceboxRenderModel Spacebox { get; set; }

        public bool IsComplete => _isComplete;

        public string WaitKey => _key;

        public override void UpdateGeometry()
        {
            if (dirty)
            {
                var mesh = Spacebox.Mesh;

                drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                drawState.RenderState.FacetCulling.FrontFaceWindingOrder = A.FrontFaceDirection.Cw;
                
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
}
