using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using Globe3DLight.Renderer;

namespace Globe3DLight.Scene
{
    //public class ShapefileRenderer : IRenderable, IDisposable
    //{
    //    public ShapefileRenderer(
    //        string filename,
    //        Context context,
    //        Ellipsoid globeShape,
    //        ShapefileAppearance appearance)
    //    {
    //        Shapefile shapefile = new Shapefile(filename);

    //        switch (shapefile.ShapeType)
    //        {
    //            case ShapeType.Point:
    //                //PointShapefile pointShapefile = new PointShapefile(shapefile, context, globeShape, appearance);
    //                //pointShapefile.DepthWrite = false;
    //                //_shapefileGraphics = pointShapefile;
    //                break;
    //            case ShapeType.Polyline:
    //                PolylineShapefile polylineShapefile = new PolylineShapefile(shapefile, context, globeShape, appearance);
    //                //polylineShapefile.DepthWrite = false;
    //                _shapefileGraphics = polylineShapefile;
    //                break;
    //            case ShapeType.Polygon:
    //                PolygonShapefile polygonShapefile = new PolygonShapefile(shapefile, context, globeShape, appearance);
    //                //polygonShapefile.DepthWrite = false;
    //                _shapefileGraphics = polygonShapefile;
    //                break;
    //            default:
    //                throw new NotSupportedException("Rendering is not supported for " + shapefile.ShapeType.ToString() + " shapefiles.");
    //        }
    //    }

    //    #region IRenderable Members

    //    public void Render(Context context, SceneState sceneState)
    //    {
    //        if(_shapefileGraphics != null)
    //        _shapefileGraphics.Render(context, sceneState);
    //    }

    //    #endregion


    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        if (_shapefileGraphics != null)
    //        {
    //            _shapefileGraphics.Dispose();
    //        }
    //    }

    //    #endregion

    //    public bool Wireframe
    //    {
    //        get { return _shapefileGraphics.Wireframe; }
    //        set { _shapefileGraphics.Wireframe = value; }
    //    }

    //    private readonly ShapefileGraphics _shapefileGraphics;
    //}

}
