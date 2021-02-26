﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
//using OpenTK.Graphics.OpenGL;
using Globe3DLight.Geometry;

namespace Globe3DLight
{
    public static class SimplePolygonAlgorithms
    {
        /// <summary>
        /// Cleans up a simple polygon by removing duplicate adjacent positions and making
        /// the first position not equal the last position
        /// </summary>
        public static IList<T> Cleanup<T>(IEnumerable<T> positions)
        {
            IList<T> positionsList = CollectionAlgorithms.EnumerableToList(positions);

            if (positionsList.Count < 3)
            {
                throw new ArgumentOutOfRangeException("positions", "At least three positions are required.");
            }

            List<T> cleanedPositions = new List<T>(positionsList.Count);

            for (int i0 = positionsList.Count - 1, i1 = 0; i1 < positionsList.Count; i0 = i1++)
            {
                T v0 = positionsList[i0];
                T v1 = positionsList[i1];

                if (!v0.Equals(v1))
                {
                    cleanedPositions.Add(v1);
                }
            }

            cleanedPositions.TrimExcess();
            return cleanedPositions;
        }

        public static double ComputeArea(IEnumerable<dvec2> positions)
        {
            IList<dvec2> positionsList = CollectionAlgorithms.EnumerableToList(positions);

            if (positionsList.Count < 3)
            {
                throw new ArgumentOutOfRangeException("positions", "At least three positions are required.");
            }

            double area = 0.0;

            //
            // Compute the area of the polygon.  The sign of the area determines the winding order.
            //
            for (int i0 = positionsList.Count - 1, i1 = 0; i1 < positionsList.Count; i0 = i1++)
            {
                dvec2 v0 = positionsList[i0];
                dvec2 v1 = positionsList[i1];

                area += (v0.x * v1.y) - (v1.x * v0.y);
            }

            return area * 0.5;
        }

        public static FrontFaceDirection ComputeWindingOrder(IEnumerable<dvec2> positions)
        {
            return (ComputeArea(positions) >= 0.0) ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw;
        }
    }

}
