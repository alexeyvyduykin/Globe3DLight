﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Renderer
{
    public abstract class NodeRenderer : ViewModelBase, IRenderContext
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

        public void DrawScenario(object dc, ScenarioContainerViewModel scenario)
        {
            DrawFrames(dc, scenario.OutlinerEditor.FrameRoot.Single(), scenario.SceneState);

            foreach (var entity in scenario.OutlinerEditor.Entities)
            {
                DrawEntities(dc, entity, scenario.SceneState);
            }
        }

        private void DrawEntities(object dc, BaseEntity entity, ISceneState scene)
        {
            if (entity != null)
            {
                if (entity is IDrawable drawable)
                {
                    drawable.DrawShape(dc, this, scene);

                    if (entity is IChildren obj)
                    {
                        foreach (var item in obj.Children)
                        {
                            DrawEntities(dc, item, scene);
                            //obj.DrawShape(dc, this, scene);
                        }
                    }
                }
                //else if (scenarioObject is IDrawableCollection drawableCollection)
                //{
                //    drawableCollection.DrawShapeCollection(dc, this, scene);
                //}
            }
        }

        private void DrawFrames(object dc, FrameViewModel frame, ISceneState scene)
        {
            if (frame != null)
            {
                if (frame is IDrawable drawable)
                {
                    drawable.DrawShape(dc, this, scene);

                    foreach (var item in frame.Children)
                    {
                        DrawFrames(dc, item, scene);                    
                    }
                }
            }
        }

        public void DrawSun(object dc, SunRenderModel sun, dmat4 modelMatrix, ISceneState scene)
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

        public void DrawEarth(object dc, EarthRenderModel earth, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(earth);
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

        public void DrawFrame(object dc, FrameRenderModel frame, dmat4 modelMatrix, ISceneState scene)
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

                //if (frame.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateFrameDrawNode(frame);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(frame, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawFrameList(object dc, FrameRenderModel frame, IEnumerable<dmat4> modelMatrices, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(frame);
            if (drawNodeCached != null)
            {
                //if (frame.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrices, scene);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateFrameDrawNode(frame);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(frame, drawNode);

                drawNode.Draw(dc, modelMatrices, scene);
            }
        }

        public void DrawOrbit(object dc, OrbitRenderModel orbit, dmat4 modelMatrix, ISceneState scene)
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

                //if (orbit.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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

        public void DrawRetranslator(object dc, RenderModel retranslator, dmat4 modelMatrix, ISceneState scene)
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

                //if (retranslator.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateRetranslatorDrawNode(retranslator, _textureCache);

                drawNode.UpdateStyle();
                drawNode.UpdateGeometry();

                _drawNodeCache.Set(retranslator, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawSatellite(object dc, RenderModel satellite, dmat4 modelMatrix, ISceneState scene)
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

                //if (satellite.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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

        public void DrawSensor(object dc, SensorRenderModel sensor, dmat4 modelMatrix, ISceneState scene)
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

                //if (sensor.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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

        public void DrawAntenna(object dc, AntennaRenderModel antenna, dmat4 modelMatrix, ISceneState scene)
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

                //if (antenna.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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

        public void DrawSpacebox(object dc, SpaceboxRenderModel spacebox, dmat4 modelMatrix, ISceneState scene)
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

        public void DrawGroundStation(object dc, RenderModel groundStation, dmat4 modelMatrix, ISceneState scene)
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

                //if (groundStation.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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

        public void DrawGroundStationList(object dc, RenderModel groundStation, IEnumerable<dmat4> modelMatrices, ISceneState scene)
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

                //if (groundStation.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrices, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateGroundStationDrawNode(groundStation);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(groundStation, drawNode);

                drawNode.Draw(dc, modelMatrices, scene/*_state.ZoomX*/);
            }
        }

        public void DrawGroundObject(object dc, GroundObjectRenderModel groundobject, dmat4 modelMatrix, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(groundobject);
            if (drawNodeCached != null)
            {
                //if (groundobject.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

                drawNodeCached.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
            else
            {
                var drawNode = _drawNodeFactory.CreateGroundObjectListDrawNode(groundobject);

                drawNode.UpdateStyle();

                drawNode.UpdateGeometry();

                _drawNodeCache.Set(groundobject, drawNode);

                drawNode.Draw(dc, modelMatrix, scene/*_state.ZoomX*/);
            }
        }

        public void DrawGroundObjectList(object dc, GroundObjectRenderModel groundobject, IEnumerable<dmat4> modelMatrices, ISceneState scene)
        {
            var drawNodeCached = _drawNodeCache.Get(groundobject);
            if (drawNodeCached != null)
            {
                //if (groundobject.IsDirty())
                //{
                //    drawNodeCached.UpdateGeometry();
                //}

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
