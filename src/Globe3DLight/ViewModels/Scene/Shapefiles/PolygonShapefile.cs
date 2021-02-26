using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using Globe3DLight.Renderer;
using GlmSharp;
//using System.Drawing;


namespace Globe3DLight.Scene
{
    //internal class PolygonShapefile : ShapefileGraphics
    //{
    //    public PolygonShapefile(
    //        Shapefile shapefile,
    //        Context context,
    //        Ellipsoid globeShape,
    //        ShapefileAppearance appearance)
    //    {
    //        //Verify.ThrowIfNull(shapefile);
    //        //Verify.ThrowIfNull(context);
    //        //Verify.ThrowIfNull(globeShape);

    //        polyline = new OutlinedPolyline();
    //        polygons = new List<Polygon>();

    //        VertexAttributeDoubleVector3 positionAttribute = new VertexAttributeDoubleVector3("POSITION");
    //        VertexAttributeRGBA colorAttribute = new VertexAttributeRGBA("COLOR");
    //        VertexAttributeRGBA outlineColorAttribute = new VertexAttributeRGBA("OUTLINECOLOR");
    //        IndicesUnsignedInt indices = new IndicesUnsignedInt();

    //        Random r = new Random(3);
    //        IList<dvec3> positions = new List<dvec3>();

    //        foreach (Shape shape in shapefile)
    //        {
    //            if (shape.ShapeType != ShapeType.Polygon)
    //            {
    //                throw new NotSupportedException("The type of an individual shape does not match the Shapefile type.");
    //            }

    //            PolygonShape polygonShape = (PolygonShape)shape;

    //            for (int j = 0; j < polygonShape.Count; ++j)
    //            {
    //                Color color = Color.FromArgb(127, r.Next(256), r.Next(256), r.Next(256));

    //                positions.Clear();

    //                ShapePart part = polygonShape[j];

    //                for (int i = 0; i < part.Count; ++i)
    //                {
    //                    dvec2 point = part[i];

    //                    positions.Add(globeShape.ToVector3D(ExtraMath.ToRadians(new Geodetic3D(point.x, point.y))));

    //                    //
    //                    // For polyline
    //                    //
    //                    positionAttribute.Values.Add(globeShape.ToVector3D(ExtraMath.ToRadians(new Geodetic3D(point.x, point.y))));
    //                    colorAttribute.AddColor(color);
    //                    outlineColorAttribute.AddColor(Color.Black);

    //                    if (i != 0)
    //                    {
    //                        indices.Values.Add((uint)positionAttribute.Values.Count - 2);
    //                        indices.Values.Add((uint)positionAttribute.Values.Count - 1);
    //                    }
    //                }
                    
    //                try
    //                {
    //                    Polygon p = new Polygon(context, globeShape, positions);
    //                    p.Color = color;
    //                    polygons.Add(p);
    //                }
    //                catch (ArgumentOutOfRangeException) // Not enough positions after cleaning
    //                {
    //                    int hghg = 0;
    //                }
    //            }
    //        }

    //        Mesh mesh = new Mesh();
    //        mesh.PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
    //        mesh.Attributes.Add(positionAttribute);
    //        mesh.Attributes.Add(colorAttribute);
    //        mesh.Attributes.Add(outlineColorAttribute);
    //        mesh.Indices = indices;
    //        polyline.Set(context, mesh);
    //    }

    //    #region ShapefileGraphics Members

    //    public override void Render(Context context, SceneState sceneState)
    //    {
    //        polyline.Render(context, sceneState);

    //        foreach (Polygon polygon in polygons)
    //        {
    //            polygon.Render(context, sceneState);
    //        }
    //    }

    //    public override void Dispose()
    //    {
    //        if (polyline != null)
    //        {
    //            polyline.Dispose();
    //        }

    //        foreach (Polygon polygon in polygons)
    //        {
    //            polygon.Dispose();
    //        }
    //    }

    //    public override bool Wireframe
    //    {
    //        get { return polyline.Wireframe; }
    //        set
    //        {
    //            polyline.Wireframe = value;

    //            foreach (Polygon polygon in polygons)
    //            {
    //                polygon.Wireframe = value;
    //            }
    //        }
    //    }

    //    #endregion

    //    /*
    //            public double Width
    //            {
    //                get { return _polyline.Width; }
    //                set { _polyline.Width = value; }
    //            }

    //            public double OutlineWidth
    //            {
    //                get { return _polyline.OutlineWidth; }
    //                set { _polyline.OutlineWidth = value; }
    //            }
    //    */

    //    //public bool DepthWrite
    //    //{
    //    //    get { return polyline.DepthWrite; }
    //    //    set
    //    //    {
    //    //        polyline.DepthWrite = value;

    //    //        foreach (Polygon polygon in polygons)
    //    //        {
    //    //            polygon.DepthWrite = value;
    //    //        }
    //    //    }
    //    //}

    //    private readonly OutlinedPolyline polyline;
    //    private readonly IList<Polygon> polygons;
    //}

}
