using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;
using GlmSharp;
using Globe3DLight.Containers;
using System.Collections.Immutable;
using Globe3DLight.ScenarioObjects;

namespace Globe3DLight.Renderer
{
    public abstract class NodeRenderer : ObservableObject, IRenderContext
    {
        private readonly IServiceProvider _serviceProvider;
    //    private IShapeRendererState _state;
    //    private readonly ICache<string, IDisposable> _biCache;
        private readonly ICache<object, IDrawNode> _drawNodeCache;
        private readonly IDrawNodeFactory _drawNodeFactory;

        private readonly IImageLibrary _imageLibrary;
        private readonly ICache<string, int> _textureCache;

        public NodeRenderer(IServiceProvider serviceProvider, IDrawNodeFactory drawNodeFactory)
        {
            _serviceProvider = serviceProvider;
       //     _state = _serviceProvider.GetService<IFactory>().CreateShapeRendererState();
        //    _biCache = _serviceProvider.GetService<IFactory>().CreateCache<string, IDisposable>(x => x.Dispose());
            _drawNodeCache = _serviceProvider.GetService<IFactory>().CreateCache<object, IDrawNode>(x => x.Dispose());

            _imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            _textureCache = _serviceProvider.GetService<IFactory>().CreateCache<string, int>();

            _drawNodeFactory = drawNodeFactory;
        }


        public void DrawSun(object dc, ISunRenderModel sun, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(sun);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                //if (sun.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                if (drawNodeCached is IThreadLoadingNode threadLoadingNode && threadLoadingNode.IsComplete == false)
                {
                    _imageLibrary.Pass(threadLoadingNode, _textureCache);
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateSunDrawNode(sun);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(sun, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawEarth(object dc, IEarthRenderModel earth, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(earth);
            if (drawNodeCached != null)
            {
                if(drawNodeCached is IThreadLoadingNode threadLoadingNode && threadLoadingNode.IsComplete == false)
                {
                    _imageLibrary.Pass(threadLoadingNode, _textureCache);
                }
                
                
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                //if (earth.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateEarthDrawNode(earth);

                drawNode.UpdateStyle(); 
                
                drawNode.UpdateGeometry();  // ????????????????????? earth.IsDirty() not work

                _drawNodeCache.Set(earth, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawFrame(object dc, IFrameRenderModel frame, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(frame);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (frame.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateFrameDrawNode(frame);

                drawNode.UpdateStyle();

                _drawNodeCache.Set(frame, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawOrbit(object dc, IOrbitRenderModel orbit, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(orbit);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (orbit.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateOrbitDrawNode(orbit);

                drawNode.UpdateStyle();
                drawNode.UpdateGeometry();

                _drawNodeCache.Set(orbit, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawGroundStation(object dc, IGroundStationRenderModel groundStation, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(groundStation);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (groundStation.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateGroundStationDrawNode(groundStation);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(groundStation, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawRetranslator(object dc, IRetranslatorRenderModel retranslator, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(retranslator);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (retranslator.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateRetranslatorDrawNode(retranslator);

                drawNode.UpdateStyle(); 
                drawNode.UpdateGeometry();

                _drawNodeCache.Set(retranslator, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawSatellite(object dc, ISatelliteRenderModel satellite, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(satellite);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (satellite.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateSatelliteDrawNode(satellite, _textureCache);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(satellite, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawSensor(object dc, ISensorRenderModel sensor, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(sensor);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (sensor.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateSensorDrawNode(sensor);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(sensor, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawAntenna(object dc, IAntennaRenderModel antenna, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(antenna);
            if (drawNodeCached != null)
            {
                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

                if (antenna.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateAntennaDrawNode(antenna);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(antenna, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }
        public void DrawSpacebox(object dc, ISpaceboxRenderModel spacebox, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(spacebox);
            if (drawNodeCached != null)
            {

                if (drawNodeCached is IThreadLoadingNode threadLoadingNode && threadLoadingNode.IsComplete == false)
                {
                    _imageLibrary.Pass(threadLoadingNode, _textureCache);
                }

                //if (sun.Style.IsDirty() || drawNodeCached.Style != sun.Style)
                //{
                //    drawNodeCached.Style = sun.Style;
                //    drawNodeCached.UpdateStyle();
                //    sun.Style.Invalidate();
                //}

            //    if (spacebox.IsDirty())
            //    {
            //        drawNodeCached.UpdateGeometry();
            //    }

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateSpaceboxDrawNode(spacebox);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(spacebox, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawScenario(object dc, IScenarioContainer container)
        {
            foreach (var obj in container.ScenarioObjects)
            {
                DrawScenarioObject(dc, obj, container.SceneState);
            }
        }

        private void DrawScenarioObject(object dc, IScenarioObject scenarioObject, ISceneState scene)
        {
            if (scenarioObject != null && scenarioObject is IDrawable drawable)
            {
                drawable.DrawShape(dc, this, scene);

                foreach (var obj in scenarioObject.Children)
                {
                    DrawScenarioObject(dc, obj, scene);
                    //obj.DrawShape(dc, this, scene);
                }
            }
        }

        public void DrawGroundObjectList(object dc, IGroundObjectListRenderModel groundobject, IEnumerable<dmat4> modelMatrices, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(groundobject);
            if (drawNodeCached != null)
            {
                if (groundobject.IsDirty())
                {
                    drawNodeCached.UpdateGeometry();
                }

                drawNodeCached.Draw(dc, modelMatrices, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateGroundObjectListDrawNode(groundobject);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(groundobject, drawNode);

                drawNode.Draw(dc, modelMatrices, scene/*_state.ZoomX*/);
            }

        }
    }
}
