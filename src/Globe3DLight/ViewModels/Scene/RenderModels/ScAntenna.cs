using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;


namespace Globe3DLight.Scene
{
    //public class ScAntenna : BaseSceneObject
    //{
    ////    TreeNode TreeNode { get; set; }
    //    SatelliteTranslationModelVerySimple Model { get; set; }
    ////   FrameGraphicsComponent FrameMain { get; set; }
    //   // FrameGraphicsComponent FrameSat { get; set; }
    //   // FrameGraphicsComponent FrameTarget { get; set; }
    //    public ScAntenna(ObjectTreeNode parentNode /*string name, TreeNode parentNode*/) : base(parentNode)
    //    {
    //    //    this.Name = name;
    //      //  this.LogicalTreeNode = logicalTreeNode;

    //        this.Model = new SatelliteTranslationModelVerySimple();
    //        // this.FrameMain = new FrameGraphicsComponent(1.0);
    //        //  this.FrameSat = new FrameGraphicsComponent(2.0);
    //        // this.FrameTarget = new FrameGraphicsComponent(30.0);

    //    //    this.TreeNode = parentNode;// parentNode.AddChild(new TreeNodeContent(name));

    //        //initCircle();
    //    }

    //    public SceneObjectTypes Type => throw new NotImplementedException();

    //    public dmat4 ModelMatrix => throw new NotImplementedException();

    // //   public string Name { get; set; }

    //    //public void Render_old(Context context, SceneState scene)
    //    //{
    //    //    var pSource = TreeNode.LogicalTreeNode.Position;
    //    //    var TarNode = TreeNode.Root.Find((s) => s.Name == "Orbital5");
    //    //    var PTarget = TarNode.LogicalTreeNode.Position;
    //    //    //GL.MatrixMode(MatrixMode.Projection);
    //    //    //GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

    //    //    //GL.MatrixMode(MatrixMode.Modelview);
    //    //    //dmat4 modelView = scene.ViewMatrix;// * mdl;// * modelMatrix;
    //    //    //GL.LoadMatrix(modelView.Values1D);

    //    //    //GL.Color3(1.0, 0.0, 0.0);
    //    //    //GL.LineWidth(30.0f);
    //    //    //GL.Begin(BeginMode.Lines);
    //    //    //GL.Vertex3(pSource.x, pSource.y, pSource.z);
    //    //    //GL.Vertex3(PTarget.x, PTarget.y, PTarget.z);
    //    //    //GL.End();
    //    //    //GL.LineWidth(1.0f);

    //    //    AntennaDataProvider dp = TreeNode.LogicalTreeNode.DataProvider as AntennaDataProvider;

    //    //    if (dp.Enable == true)
    //    //    {
    //    //        string target = dp.Target;

    //    //        if(target == "Retranslator1") { 
    //    //            target = "Orbital5"; }
    //    //        else if (target == "Retranslator2") 
    //    //        {
    //    //            target = "Orbital6"; }
    //    //        else if (target == "Retranslator3") { 
    //    //            target = "Orbital7"; }
               
    //    //        var TargetNode = TreeNode.Root.Find((s) => s.Name == target);
           
    //    //        if (TargetNode != null)
    //    //        {                 
    //    //            dmat4 modelMatrixTarget = TargetNode.LogicalTreeNode.ModelMatrix;
    //    //            dmat4 modelMatrix = TreeNode.LogicalTreeNode.ModelMatrix;
    //    //            //-----
    //    //            dvec3 posTarget_WS = TargetNode.LogicalTreeNode.Position;// dbRetranslator[dbSatelliteTranslation.Target].Position;
    //    //            dmat4 modelSource =
    //    //                getModelMatrix_LeftAntenna(posTarget_WS, modelMatrix/*dbSatellite.SatelliteModelMatrix*/);
    //    //            dvec3 posSource_WS = new dvec3(modelSource * new vec4(0.0f, 0.0f, 0.0f, 1.0f));

    //    //            dvec3 dir = posTarget_WS - posSource_WS;
    //    //            float len = (float)glm.Length(dir);
    //    //            dmat4 modelTarget = modelSource * dmat4.Translate(new dvec3(0.0f, len, 0.0f));
    //    //            modelTarget = modelTarget * dmat4.Rotate(180.0f, new dvec3(1.0f, 0.0f, 0.0f));
    //    //            float ScaleSource = 0.006f;
    //    //            float ScaleTarget = 1.45f;
    //    //            //-----

    //    //            dvec4 centerSource_TS = modelTarget.Inverse * modelSource * new vec4(0.0f, 0.0f, 0.0f, 1.0f);

    //    //            if (centerSource_TS.y >= 0.0)
    //    //            {
    //    //                modelSource.m20 = -modelSource.m20;
    //    //                modelSource.m21 = -modelSource.m21;
    //    //                modelSource.m22 = -modelSource.m22;
    //    //            }

    //    //            dmat4 CircleSource_TS = getCircleSource_TS(modelSource, modelTarget);
    //    //            dmat4 CircleTarget_TS = getCircleTarget_TS(modelSource, modelTarget);

    //    //            dmat4 ModelSource = modelSource;
    //    //            dmat4 ModelTarget = modelTarget;



    //    //     //       Model.Render(modelMatrix, new vec4(0.0f, len, 0.0f, 1.0f), ScaleSource, ScaleTarget, context, scene);
    //    //        }
    //    //    }
    //    //}

    //    public override void Render(Context context, SceneState scene)
    //    {              
    //        AntennaDataProvider dp = ParentNode.LastLogicalTreeNode.DataProvider/* TreeNode.LogicalTreeNode.DataProvider*/ as AntennaDataProvider;

    //        if (dp != null && dp.Enable == true)
    //        {
    //            string target = dp.Target;

    //            var TargetNode = (ParentNode.Root as ObjectTreeNode)[target] as ObjectTreeNode;// TreeNode.Root[target];

    //            if (TargetNode != null)
    //            {
    //                // float ScaleSource = 0.006f;
    //                // float ScaleTarget = 1.45f;
    //                dvec3 PosTarget = dvec3.NaN;
              
    //                if (TargetNode.LastLogicalTreeNode.DataProvider is IDataProvider dataprovider)
    //                {
    //                    PosTarget = dataprovider.Position;
    //                }
    //                else if(TargetNode.LastLogicalTreeNode.DataProvider is IDataProviderCollection dataprovidercollection)
    //                {
    //                    PosTarget = dataprovidercollection.Positions[target];
    //                }
    //              //  var PosTarget = TargetNode.LogicalTreeNode.Position;
    //                var PosSource = (ParentNode.LastLogicalTreeNode.DataProvider/*TreeNode.LogicalTreeNode.DataProvider*/ as IDataProvider).Position;
    //                var ModelMatrixSource = ParentNode.LastLogicalTreeNode.ModelMatrix;// TreeNode.LogicalTreeNode.ModelMatrix;

    //             //   var FrameSource = Frame1(PosTarget, ModelMatrixSource);
    //             //   dvec3 dir = PosTarget - PosSource;
    //             //   float len = (float)glm.Length(dir);


    //                Model.Render(ModelMatrixSource, PosSource, PosTarget, context, scene);

    //                //Model.Render(FrameSource, new vec4(0.0f, len, 0.0f, 1.0f), ScaleSource, ScaleTarget, context, scene);
    //            }
    //        }
    //    }


    //    private int protoCircleSize=36;
    //    private vec3[] protoCircle;

    //    private void initCircle()
    //    {
    //        float curAngle, dAngle = glm.Radians(360.0f / (float)protoCircleSize);

    //        protoCircle = new vec3[protoCircleSize];

    //        for (int i = 0; i < 36; i++)
    //        {
    //            curAngle = dAngle * i;
    //            protoCircle[i] = new vec3((float)Math.Cos(curAngle), 0.0f, (float)-Math.Sin(curAngle));
    //        }
    //    }

    //    //public void Render_test(Context context, SceneState scene)
    //    //{
    //    //    double ScaleS = 0.006;
    //    //    double ScaleT = 1.45;
    //    //    var TargetNode =TreeNode.Root.Find((s) => s.Name == "Orbital5");

    //    //    var PosTarget = TargetNode.LogicalTreeNode.Position;

    //    //    var PosSource = TreeNode.LogicalTreeNode.Position;
    //    //    var ModelMatrixSource = TreeNode.LogicalTreeNode.ModelMatrix;

    //    //    //GL.MatrixMode(MatrixMode.Projection);
    //    //    //GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

    //    //    //GL.MatrixMode(MatrixMode.Modelview);
    //    //    //dmat4 modelView = scene.ViewMatrix;// * mdl;// * modelMatrix;
    //    //    //GL.LoadMatrix(modelView.Values1D);

    //    //    //GL.Color3(1.0, 0.0, 0.0);
    //    //    //GL.LineWidth(3.0f);
    //    //    //GL.Begin(BeginMode.Lines);
    //    //    //GL.Vertex3(PosSource.x, PosSource.y, PosSource.z);
    //    //    //GL.Vertex3(PosTarget.x, PosTarget.y, PosTarget.z);
    //    //    //GL.End();
    //    //    //GL.LineWidth(1.0f);


    //    //    var FrameSource = Frame1(PosTarget, ModelMatrixSource);
    //    //    GL.LineWidth(8.0f);
    //    // //  FrameMain.Render(FrameSource, context, scene);
    //    //    GL.LineWidth(1.0f);

    //    ////    FrameSat.Render(ModelMatrixSource, context, scene);


    //    //    dvec3 dir_ = PosTarget - PosSource;
    //    //    float len_ = (float)glm.Length(dir_);
    //    //    dmat4 JTarget = FrameSource * dmat4.Translate(new dvec3(0.0f, len_, 0.0f));
            
    //    //  //  FrameTarget.Render(JTarget, context, scene);

    //    //    JTarget = JTarget * dmat4.Rotate(glm.Radians(180.0f),  new dvec3(1.0f, 0.0f, 0.0f));

    //    //    GL.LineWidth(8.0f);
    //    ////    FrameTarget.Render(JTarget, context, scene);
    //    //    GL.LineWidth(1.0f);


    //    ////    Model.Render(FrameSource, new vec4(0.0f, len_, 0.0f, 1.0f), 0.006f, 1.45f, context, scene);


    //    //   // GL.MatrixMode(MatrixMode.Projection);
    //    //   // GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

    //    //   // GL.MatrixMode(MatrixMode.Modelview);
    //    //   // dmat4 modelView = scene.ViewMatrix * FrameSource;
    //    //   // GL.LoadMatrix(modelView.Values1D);

    //    //   //// GL.Enable(EnableCap.CullFace);
    //    //   //// GL.CullFace(CullFaceMode.Front);
    //    //   //// GL.FrontFace(FrontFaceDirection.Cw);

    //    //   // GL.Enable(EnableCap.Blend);
    //    //   // GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


    //    //   // GL.Color3(1.0, 1.0, 0.0);
    //    //   // GL.Begin(BeginMode.QuadStrip);

    //    //   // for (int i = 0; i < protoCircleSize; i++)
    //    //   // {
    //    //   //     GL.Vertex3(ScaleS * protoCircle[i].x, 0.0, ScaleS * protoCircle[i].z);
    //    //   //     GL.Vertex3(ScaleT * protoCircle[i].x, len_, ScaleT * protoCircle[i].z);
    //    //   //     if (i + 1 == protoCircleSize)
    //    //   //     {
    //    //   //         GL.Vertex3(ScaleS * protoCircle[0].x, 0.0, ScaleS * protoCircle[0].z);
    //    //   //         GL.Vertex3(ScaleT * protoCircle[0].x, len_, ScaleT * protoCircle[0].z);
    //    //   //     }
    //    //   //     else
    //    //   //     {
    //    //   //         GL.Vertex3(ScaleS * protoCircle[i + 1].x, 0.0, ScaleS * protoCircle[i + 1].z);
    //    //   //         GL.Vertex3(ScaleT * protoCircle[i + 1].x, len_, ScaleT * protoCircle[i + 1].z);
    //    //   //     }
    //    //   // }

    //    //   // GL.End();




    //    //   // GL.Disable(EnableCap.Blend);
    //    //   //// GL.Disable(EnableCap.CullFace);



    //    //    dmat4 modelMatrixTarget = TargetNode.LogicalTreeNode.ModelMatrix;
    //    //            dmat4 modelMatrix = TreeNode.LogicalTreeNode.ModelMatrix;
    //    //            //-----
    //    //            dvec3 posTarget_WS = TargetNode.LogicalTreeNode.Position;// dbRetranslator[dbSatelliteTranslation.Target].Position;
    //    //            dmat4 modelSource =
    //    //                getModelMatrix_LeftAntenna(posTarget_WS, modelMatrix/*dbSatellite.SatelliteModelMatrix*/);
    //    //            dvec3 posSource_WS = new dvec3(modelSource * new vec4(0.0f, 0.0f, 0.0f, 1.0f));

    //    //            dvec3 dir = posTarget_WS - posSource_WS;
    //    //            float len = (float)glm.Length(dir);
    //    //            dmat4 modelTarget = modelSource * dmat4.Translate(new dvec3(0.0f, len, 0.0f));
    //    //            modelTarget = modelTarget * dmat4.Rotate(180.0f, new dvec3(1.0f, 0.0f, 0.0f));
    //    //            float ScaleSource = 0.006f;
    //    //            float ScaleTarget = 1.45f;
    //    //            //-----

    //    //            dvec4 centerSource_TS = modelTarget.Inverse * modelSource * new vec4(0.0f, 0.0f, 0.0f, 1.0f);

    //    //            if (centerSource_TS.y >= 0.0)
    //    //            {
    //    //                modelSource.m20 = -modelSource.m20;
    //    //                modelSource.m21 = -modelSource.m21;
    //    //                modelSource.m22 = -modelSource.m22;
    //    //            }

    //    //            dmat4 CircleSource_TS = getCircleSource_TS(modelSource, modelTarget);
    //    //            dmat4 CircleTarget_TS = getCircleTarget_TS(modelSource, modelTarget);

    //    //            dmat4 ModelSource = modelSource;
    //    //            dmat4 ModelTarget = modelTarget;



    //    //  //  Model.Render(modelTarget/*modelMatrixTarget*/, CircleSource_TS, ScaleSource, CircleTarget_TS, ScaleTarget, context, scene);



    //    //}

    //    private dmat4 Frame1(dvec3 posTarget_WS, dmat4 modelMatrix)
    //    {
    //        dmat4 model = modelMatrix;//modelSatellite;

    //        dvec4 posTarget = model.Inverse * new dvec4(posTarget_WS, 1.0f);                     // позиция цели в сист коорд антенны связанной с КА
    //        dvec4 posSource_WS = model * new dvec4(0.0f, 0.0f, 0.0f, 1.0f);                                  // позиция центра сист коорд антенны в World Space

    //        dvec3 to = new dvec3(posTarget);
    //        to = glm.Normalized(to);
    //        dmat4 orientation = Orient(new dvec3(0.0f, 1.0f, 0.0f), to);

    //        return model * orientation;
    //    }

    //    private dmat4 getCircleTarget_TS(dmat4 modelSource, dmat4 modelTarget)
    //    {
    //        dvec4 centerSource_TS = modelTarget.Inverse * modelSource * new dvec4(0.0f, 0.0f, 0.0f, 1.0f);
    //        dvec4 point0Source_TS = modelTarget.Inverse * modelSource * new dvec4(1.0f, 0.0f, 0.0f, 1.0f);

    //        dvec4 dirPoint0_TS = point0Source_TS - centerSource_TS;
    //        dirPoint0_TS.y = 0.0f;
    //        dirPoint0_TS = glm.Normalized(dirPoint0_TS);

    //        dmat4 orientation = Orient(new dvec3(dirPoint0_TS), new dvec3(1.0f, 0.0f, 0.0f));

    //        return orientation.Inverse;
    //    }

    //    private dmat4 getCircleSource_TS(dmat4 modelSource, dmat4 modelTarget)
    //    {
    //        return modelTarget.Inverse * modelSource;
    //    }

    //    private dmat4 getModelMatrix_LeftAntenna(dvec3 posTarget_WS, dmat4 modelMatrix)
    //    {
    //        dmat4 model = modelMatrix;//modelSatellite;

    //        dvec3 p0LeftAntenna = new dvec3(67.74, -12.22, -23.5) * World.Scale();

    //        dvec3 pos = p0LeftAntenna;                                                                 // позиция антенны в системе коорд КА
    //        model = model * dmat4.Translate(pos);                                                              // модель антенны в сист коорд КА

    //        dvec4 posTarget = model.Inverse * new dvec4(posTarget_WS, 1.0f);                     // позиция цели в сист коорд антенны связанной с КА
    //        dvec4 posSource_WS = model * new dvec4(0.0f, 0.0f, 0.0f, 1.0f);                                  // позиция центра сист коорд антенны в World Space

    //        dvec3 to = new dvec3(posTarget);
    //        to = glm.Normalized(to);
    //        dmat4 orientation = Orient(new dvec3(0.0f, 1.0f, 0.0f), to);

    //        return model * orientation;
    //    }

    //    private dmat4 Orient(dvec3 from, dvec3 to)
    //    {
    //        double c = dvec3.Dot(from, to);

    //        dvec3 u = dvec3.Cross(from, to);
    //        double h;

    //        if (c == 1.0 || dvec3.Dot(u, u) == 0.0)
    //            h = 0.0f;
    //        else
    //            h = (1.0f - c) / dvec3.Dot(u, u);

    //        dmat4 mtx = new dmat4();
    //        mtx.m00 = c + h * u.x * u.x; mtx.m01 = h * u.y * u.x + u.z; mtx.m02 = h * u.z * u.x - u.y; mtx.m03 = 0.0f;
    //        mtx.m10 = h * u.x * u.y - u.z; mtx.m11 = c + h * u.y * u.y; mtx.m12 = h * u.z * u.y + u.x; mtx.m13 = 0.0f;
    //        mtx.m20 = h * u.x * u.z + u.y; mtx.m21 = h * u.y * u.z - u.x; mtx.m22 = c + h * u.z * u.z; mtx.m23 = 0.0f;
    //        mtx.m30 = 0.0f; mtx.m31 = 0.0f; mtx.m32 = 0.0f; mtx.m33 = 1.0f;

    //        return mtx;
    //    }

    //    public void Update()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
