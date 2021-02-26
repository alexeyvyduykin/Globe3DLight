using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Geometry;

namespace Globe3DLight
{
   // [CLSCompliant(false)]
    public static class TriangleMeshSubdivision
    {
        public static TriangleMeshSubdivisionResult Compute(IEnumerable<dvec3> positions, IndicesUnsignedInt indices, double granularity)
        {
            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }

            if (indices == null)
            {
                throw new ArgumentNullException("positions");
            }

            if (indices.Values.Count < 3)
            {
                throw new ArgumentOutOfRangeException("indices", "At least three indices are required.");
            }

            if (indices.Values.Count % 3 != 0)
            {
                throw new ArgumentException("indices", "The number of indices must be divisable by three.");
            }

            if (granularity <= 0.0)
            {
                throw new ArgumentOutOfRangeException("granularity", "Granularity must be greater than zero.");
            }

            //
            // Use two queues:  one for triangles that need (or might need) to be 
            // subdivided and other for triangles that are fully subdivided.
            //
            Queue<TriangleIndicesUnsignedInt> triangles = new Queue<TriangleIndicesUnsignedInt>(indices.Values.Count / 3);
            Queue<TriangleIndicesUnsignedInt> done = new Queue<TriangleIndicesUnsignedInt>(indices.Values.Count / 3);

            IList<uint> indicesValues = indices.Values;
            for (int i = 0; i < indicesValues.Count; i += 3)
            {
                triangles.Enqueue(new TriangleIndicesUnsignedInt(indicesValues[i], indicesValues[i + 1], indicesValues[i + 2]));
            }

            //
            // New positions due to edge splits are appended to the positions list.
            //
            IList<dvec3> subdividedPositions = CollectionAlgorithms.CopyEnumerableToList(positions);

            //
            // Used to make sure shared edges are not split more than once.
            //
            Dictionary<Edge, int> edges = new Dictionary<Edge, int>();

            //
            // Subdivide triangles until we run out
            //
            while (triangles.Count != 0)
            {
                TriangleIndicesUnsignedInt triangle = triangles.Dequeue();

                dvec3 v0 = subdividedPositions[triangle.I0];
                dvec3 v1 = subdividedPositions[triangle.I1];
                dvec3 v2 = subdividedPositions[triangle.I2];

                double g0 = ExtraMath.AngleBetween(v0, v1); //v0.AngleBetween(v1);
                double g1 = ExtraMath.AngleBetween(v1, v2); //v1.AngleBetween(v2);
                double g2 = ExtraMath.AngleBetween(v2, v0); //v2.AngleBetween(v0);

                double max = Math.Max(g0, Math.Max(g1, g2));

                if (max > granularity)
                {
                    if (g0 == max)
                    {
                        Edge edge = new Edge(Math.Min(triangle.I0, triangle.I1), Math.Max(triangle.I0, triangle.I1));
                        int i;
                        if (!edges.TryGetValue(edge, out i))
                        {
                            subdividedPositions.Add((v0 + v1) * 0.5);
                            i = subdividedPositions.Count - 1;
                            edges.Add(edge, i);
                        }

                        triangles.Enqueue(new TriangleIndicesUnsignedInt(triangle.I0, i, triangle.I2));
                        triangles.Enqueue(new TriangleIndicesUnsignedInt(i, triangle.I1, triangle.I2));
                    }
                    else if (g1 == max)
                    {
                        Edge edge = new Edge(Math.Min(triangle.I1, triangle.I2), Math.Max(triangle.I1, triangle.I2));
                        int i;
                        if (!edges.TryGetValue(edge, out i))
                        {
                            subdividedPositions.Add((v1 + v2) * 0.5);
                            i = subdividedPositions.Count - 1;
                            edges.Add(edge, i);
                        }

                        triangles.Enqueue(new TriangleIndicesUnsignedInt(triangle.I1, i, triangle.I0));
                        triangles.Enqueue(new TriangleIndicesUnsignedInt(i, triangle.I2, triangle.I0));
                    }
                    else if (g2 == max)
                    {
                        Edge edge = new Edge(Math.Min(triangle.I2, triangle.I0), Math.Max(triangle.I2, triangle.I0));
                        int i;
                        if (!edges.TryGetValue(edge, out i))
                        {
                            subdividedPositions.Add((v2 + v0) * 0.5);
                            i = subdividedPositions.Count - 1;
                            edges.Add(edge, i);
                        }

                        triangles.Enqueue(new TriangleIndicesUnsignedInt(triangle.I2, i, triangle.I1));
                        triangles.Enqueue(new TriangleIndicesUnsignedInt(i, triangle.I0, triangle.I1));
                    }
                }
                else
                {
                    done.Enqueue(triangle);
                }
            }

            //
            // New indices
            //
            IndicesUnsignedInt subdividedIndices = new IndicesUnsignedInt(done.Count * 3);
            foreach (TriangleIndicesUnsignedInt t in done)
            {
                subdividedIndices.AddTriangle(t);
            }

            return new TriangleMeshSubdivisionResult(subdividedPositions, subdividedIndices);
        }
    }

}
