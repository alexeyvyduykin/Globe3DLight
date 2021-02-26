using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using Globe3DLight.Renderer;
using GlmSharp;


namespace Globe3DLight.Scene
{
    //public abstract class ShapefileGraphics : IRenderable, IDisposable
    //{
    //    public abstract void Render(Context context, SceneState sceneState);
    //    public abstract void Dispose();

    //    public abstract bool Wireframe { get; set; }
    //}

    //public class PolylineShapefile : ShapefileGraphics
    //{
    //    public PolylineShapefile(
    //        Shapefile shapefile,
    //        Context context,
    //        Ellipsoid globeShape,
    //        ShapefileAppearance appearance)
    //    {
    //        //Verify.ThrowIfNull(shapefile);
    //        //Verify.ThrowIfNull(context);
    //        //Verify.ThrowIfNull(globeShape);
    //        //Verify.ThrowIfNull(appearance);

    //        _polyline = new OutlinedPolyline();
    //        //_polyline = new Polyline();

    //        int positionsCount = 0;
    //        int indicesCount = 0;
    //        PolylineCapacities(shapefile, out positionsCount, out indicesCount);

    //        VertexAttributeDoubleVector3 positionAttribute = new VertexAttributeDoubleVector3("POSITION", positionsCount);
    //        VertexAttributeRGBA colorAttribute = new VertexAttributeRGBA("COLOR", positionsCount);
    //        VertexAttributeRGBA outlineColorAttribute = new VertexAttributeRGBA("OUTLINECOLOR", positionsCount);
    //        IndicesUnsignedInt indices = new IndicesUnsignedInt(indicesCount);

    //        foreach (Shape shape in shapefile)
    //        {
    //            if (shape.ShapeType != ShapeType.Polyline)
    //            {
    //                throw new NotSupportedException("The type of an individual shape does not match the Shapefile type.");
    //            }

    //            PolylineShape polylineShape = (PolylineShape)shape;

    //            for (int j = 0; j < polylineShape.Count; ++j)
    //            {
    //                ShapePart part = polylineShape[j];

    //                for (int i = 0; i < part.Count; ++i)
    //                {
    //                    dvec2 point = part[i];

    //                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //                    //double latitude = point.y;

    //                    //latitude = Math.Atan((1 - 6.69437999014e-3) * Math.Tan(latitude));

    //                    //point.y = latitude;

    //                    positionAttribute.Values.Add(globeShape.ToVector3D(ExtraMath.ToRadians(new Geodetic3D(point.x, point.y))));
    //                    colorAttribute.AddColor(appearance.PolylineColor);
    //                    outlineColorAttribute.AddColor(appearance.PolylineOutlineColor);

    //                    if (i != 0)
    //                    {
    //                        indices.Values.Add((uint)positionAttribute.Values.Count - 2);
    //                        indices.Values.Add((uint)positionAttribute.Values.Count - 1);
    //                    }
    //                }
    //            }
    //        }

    //        Mesh mesh = new Mesh();
    //        mesh.PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;// PrimitiveType.Lines;
    //        mesh.Attributes.Add(positionAttribute);
    //        mesh.Attributes.Add(colorAttribute);
    //        mesh.Attributes.Add(outlineColorAttribute);
    //        mesh.Indices = indices;
    //        _polyline.Set(context, mesh);
    //        _polyline.Width = appearance.PolylineWidth;
    //        _polyline.OutlineWidth = appearance.PolylineOutlineWidth;
    //    }

    //    private static void PolylineCapacities(Shapefile shapefile, out int positionsCount, out int indicesCount)
    //    {
    //        int numberOfPositions = 0;
    //        int numberOfIndices = 0;

    //        foreach (Shape shape in shapefile)
    //        {
    //            if (shape.ShapeType != ShapeType.Polyline)
    //            {
    //                throw new NotSupportedException("The type of an individual shape does not match the Shapefile type.");
    //            }

    //            PolylineShape polylineShape = (PolylineShape)shape;

    //            for (int j = 0; j < polylineShape.Count; ++j)
    //            {
    //                ShapePart part = polylineShape[j];

    //                numberOfPositions += part.Count;
    //                numberOfIndices += (part.Count - 1) * 2;
    //            }
    //        }

    //        positionsCount = numberOfPositions;
    //        indicesCount = numberOfIndices;
    //    }

    //    #region ShapefileGraphics Members

    //    public override void Render(Context context, SceneState sceneState)
    //    {
    //        _polyline.Render(context, sceneState);
    //    }

    //    public override void Dispose()
    //    {
    //        if (_polyline != null)
    //        {
    //            _polyline.Dispose();
    //        }
    //    }

    //    public override bool Wireframe
    //    {
    //        get { return _polyline.Wireframe; }
    //        set { _polyline.Wireframe = value; }
    //    }

    //    #endregion

    //    //public bool DepthWrite
    //    //{
    //    //    get { return _polyline.DepthWrite; }
    //    //    set { _polyline.DepthWrite = value; }
    //    //}

    //    private readonly OutlinedPolyline _polyline;
    //    //private readonly Polyline _polyline;
    //}
}
