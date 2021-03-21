using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class VertexBufferAttribute
    {

        public VertexBufferAttribute(VertexBuffer vertexBuffer, A.VertexAttribPointerType componentDatatype, int numberOfComponents)
            : this(vertexBuffer, componentDatatype, numberOfComponents, false, 0, 0)
        {
        }

        public VertexBufferAttribute(
            VertexBuffer vertexBuffer,
            A.VertexAttribPointerType componentDatatype,
            int numberOfComponents,
            bool normalize,
            int offsetInBytes,
            int strideInBytes)
        {
            if (numberOfComponents <= 0)
            {
                throw new ArgumentOutOfRangeException("numberOfComponents", "numberOfComponents must be greater than zero.");
            }

            if (offsetInBytes < 0)
            {
                throw new ArgumentOutOfRangeException("offsetInBytes", "offsetInBytes must be greater than or equal to zero.");
            }

            if (strideInBytes < 0)
            {
                throw new ArgumentOutOfRangeException("stride", "stride must be greater than or equal to zero.");
            }

            this.vertexBuffer = vertexBuffer;
            this.componentDatatype = componentDatatype;
            this.numberOfComponents = numberOfComponents;
            this.normalize = normalize;
            this.offsetInBytes = offsetInBytes;

            if (this.strideInBytes == 0)
            {
                //
                // Tightly packed
                //
                
                this.strideInBytes = numberOfComponents * VertexArraySizes.SizeOf(componentDatatype);
            }
            else
            {
                this.strideInBytes = strideInBytes;
            }

        }

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public A.VertexAttribPointerType ComponentDatatype
        {
            get { return componentDatatype; }
        }

        public int NumberOfComponents
        {
            get { return numberOfComponents; }
        }

        public bool Normalize
        {
            get { return normalize; }
        }

        public int OffsetInBytes
        {
            get { return offsetInBytes; }
        }

        public int StrideInBytes
        {
            get { return strideInBytes; }
        }
        
        private VertexBuffer vertexBuffer;
        private A.VertexAttribPointerType componentDatatype;
        private int numberOfComponents;
        private bool normalize;
        private int offsetInBytes;
        private int strideInBytes;
    }
}
