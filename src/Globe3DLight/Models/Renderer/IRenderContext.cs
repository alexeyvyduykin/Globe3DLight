using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Geometry;
using Globe3DLight.Scene;
using GlmSharp;
using Globe3DLight.Style;
using Globe3DLight.Containers;


namespace Globe3DLight.Renderer
{
    public interface IRenderContext
    {
        //ITextureUnits TextureUnits { get; }

        //IVertexArray CreateVertexArray_NEW(IMesh mesh, IEnumerable<IShaderVertexAttribute> shaderAttributes, BufferUsageHint usageHint);

        //void Draw(PrimitiveType primitiveType, IRenderState renderState, IVertexArray vertexArray, IShaderProgram shaderProgram, ISceneState sceneState);

        //void TEMP__ActiveTextureForSpacebox(int textureId);


        //void EnableCullFace(CullFaceMode cullFaceMode, FrontFaceDirection frontFaceDirection);

        //void DisableCullFace();

        //void EnableBlend(BlendingFactorSrc sfactor, BlendingFactorDest dfactor);

        //void DisableBlend();


        //void LoadProjectionMatrix(dmat4 mat);

        //void LoadModelviewMatrix(dmat4 mat);

        //void DrawPoints(IEnumerable<vec3> points);

        //void DrawPoints(IEnumerable<vec3> points, IColor color, float pointSize);

        //void DrawLineLoop(IEnumerable<vec3> points, IColor color, float lineWidth);

        //void DrawLine(vec3 pos0, vec3 pos1, IColor color, float lineWidth);

        //void DrawModel(IShaderProgram program, IMesh mesh, int vao);
        //void SetupMesh(IMesh mesh, out int VAO, out int VBO, out int EBO);


        void DrawScenario(object dc, IScenarioContainer container);

        void DrawSun(object dc, ISunRenderModel sun, dmat4 modelMatrix, ISceneState scene);
        void DrawEarth(object dc, IEarthRenderModel earth, dmat4 modelMatrix, ISceneState scene);
        
        void DrawFrame(object dc, IFrameRenderModel frame, dmat4 modelMatrix, ISceneState scene);
        void DrawOrbit(object dc, IOrbitRenderModel orbit, dmat4 modelMatrix, ISceneState scene);
        void DrawGroundStation(object dc, IGroundStationRenderModel groundStation, dmat4 modelMatrix, ISceneState scene);
        void DrawGroundObjectList(object dc, IGroundObjectListRenderModel groundobject, IEnumerable<dmat4> modelMatrices, ISceneState scene);
        void DrawRetranslator(object dc, IRetranslatorRenderModel retranslator, dmat4 modelMatrix, ISceneState scene);
        void DrawSatellite(object dc, ISatelliteRenderModel satellite, dmat4 modelMatrix, ISceneState scene);
        void DrawSensor(object dc, ISensorRenderModel sensor, dmat4 modelMatrix, ISceneState scene);

        void DrawAntenna(object dc, IAntennaRenderModel antenna, dmat4 modelMatrix, ISceneState scene);

        void DrawSpacebox(object dc, ISpaceboxRenderModel spacebox, dmat4 modelMatrix, ISceneState scene);
    }
}
