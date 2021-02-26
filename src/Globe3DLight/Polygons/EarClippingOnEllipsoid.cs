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
    public static class EarClippingOnEllipsoid
    {
        public static IndicesUnsignedInt Triangulate(IEnumerable<dvec3> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }

            //
            // Doubly linked list.  This would be a tad cleaner if it were also circular.
            //
            LinkedList<IndexedVector<dvec3>> remainingPositions = new LinkedList<IndexedVector<dvec3>>(); ;

            int index = 0;
            foreach (dvec3 position in positions)
            {
                remainingPositions.AddLast(new IndexedVector<dvec3>(position, index++));
            }

            if (remainingPositions.Count < 3)
            {
                throw new ArgumentOutOfRangeException("positions", "At least three positions are required.");
            }

            IndicesUnsignedInt indices = new IndicesUnsignedInt(3 * (remainingPositions.Count - 2));

            ///////////////////////////////////////////////////////////////////

            LinkedListNode<IndexedVector<dvec3>> previousNode = remainingPositions.First;
            LinkedListNode<IndexedVector<dvec3>> node = previousNode.Next;
            LinkedListNode<IndexedVector<dvec3>> nextNode = node.Next;

            int bailCount = remainingPositions.Count * remainingPositions.Count;

            while (remainingPositions.Count > 3)
            {
                dvec3 p0 = previousNode.Value.Vector;
                dvec3 p1 = node.Value.Vector;
                dvec3 p2 = nextNode.Value.Vector;

                if (IsTipConvex(p0, p1, p2))
                {
                    bool isEar = true;
                    for (LinkedListNode<IndexedVector<dvec3>> n = ((nextNode.Next != null) ? nextNode.Next : remainingPositions.First);
                        n != previousNode;
                        n = ((n.Next != null) ? n.Next : remainingPositions.First))
                    {
                        if (ContainmentTests.PointInsideThreeSidedInfinitePyramid(n.Value.Vector, dvec3.Zero, p0, p1, p2))
                        {
                            isEar = false;
                            break;
                        }
                    }

                    if (isEar)
                    {
                        indices.AddTriangle(new TriangleIndicesUnsignedInt(previousNode.Value.Index, node.Value.Index, nextNode.Value.Index));
                        remainingPositions.Remove(node);

                        node = nextNode;
                        nextNode = (nextNode.Next != null) ? nextNode.Next : remainingPositions.First;
                        continue;
                    }
                }

                previousNode = (previousNode.Next != null) ? previousNode.Next : remainingPositions.First;
                node = (node.Next != null) ? node.Next : remainingPositions.First;
                nextNode = (nextNode.Next != null) ? nextNode.Next : remainingPositions.First;

                if (--bailCount == 0)
                {
                    break;
                }
            }

            LinkedListNode<IndexedVector<dvec3>> n0 = remainingPositions.First;
            LinkedListNode<IndexedVector<dvec3>> n1 = n0.Next;
            LinkedListNode<IndexedVector<dvec3>> n2 = n1.Next;
            indices.AddTriangle(new TriangleIndicesUnsignedInt(n0.Value.Index, n1.Value.Index, n2.Value.Index));

            return indices;
        }

        private static bool IsTipConvex(dvec3 p0, dvec3 p1, dvec3 p2)
        {
            dvec3 u = p1 - p0;
            dvec3 v = p2 - p1;

            return dvec3.Dot(dvec3.Cross(u, v), p1) >= 0.0;// u.Cross(v).Dot(p1) >= 0.0;
        }
    }

}
