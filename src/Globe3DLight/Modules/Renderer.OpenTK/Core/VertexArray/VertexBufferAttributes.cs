using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal struct VertexBufferAttributeGL3x
    {
        public VertexBufferAttribute VertexBufferAttribute { get; set; }
        public bool Dirty { get; set; }
    }


    internal class VertexBufferAttributes
    {
        private Device _device;
        public VertexBufferAttributes()
        {
            _device = new Device();

        attributes = new VertexBufferAttributeGL3x[_device.MaximumNumberOfVertexAttributes];
        }
   
        public virtual VertexBufferAttribute this[int index]
        {
            get { return attributes[index].VertexBufferAttribute; }

            set
            {
                if (attributes[index].VertexBufferAttribute != value)
                {
                    if (value != null)
                    {
                        if (value.NumberOfComponents < 1 || value.NumberOfComponents > 4)
                        {
                            throw new ArgumentException(
                                "NumberOfComponents must be between one and four.");
                        }

                        if (value.Normalize)
                        {
                            if ((value.ComponentDatatype != A.VertexAttribPointerType.Byte) &&
                                (value.ComponentDatatype != A.VertexAttribPointerType.UnsignedByte) &&
                                (value.ComponentDatatype != A.VertexAttribPointerType.Short) &&
                                (value.ComponentDatatype != A.VertexAttribPointerType.UnsignedShort) &&
                                (value.ComponentDatatype != A.VertexAttribPointerType.Int) &&
                                (value.ComponentDatatype != A.VertexAttribPointerType.UnsignedInt))
                            {
                                throw new ArgumentException(
                                    "When Normalize is true, ComponentDatatype must be Byte, UnsignedByte, Short, UnsignedShort, Int, or UnsignedInt.");
                            }
                        }
                    }

                    if ((attributes[index].VertexBufferAttribute != null) && (value == null))
                    {
                        --count;
                    }
                    else if ((attributes[index].VertexBufferAttribute == null) && (value != null))
                    {
                        ++count;
                    }

                    attributes[index].VertexBufferAttribute = value;
                    attributes[index].Dirty = true;
                    dirty = true;
                }
            }
        }

        public virtual int Count
        {
            get { return count; }
        }

        public virtual int MaximumCount
        {
            get { return attributes.Length; }
        }

        public virtual IEnumerator GetEnumerator()
        {
            foreach (VertexBufferAttributeGL3x attribute in attributes)
            {
                if (attribute.VertexBufferAttribute != null)
                {
                    yield return attribute.VertexBufferAttribute;
                }
            }
        }

        internal void Clean()
        {
            if (dirty)
            {
                int maximumArrayIndex = 0;

                for (int i = 0; i < attributes.Length; ++i)
                {
                    VertexBufferAttribute attribute = attributes[i].VertexBufferAttribute;

                    if (attributes[i].Dirty)
                    {
                        if (attribute != null)
                        {
                            Attach(i);
                        }
                        else
                        {
                            Detach(i);
                        }

                        attributes[i].Dirty = false;
                    }

                    if (attribute != null)
                    {
                        maximumArrayIndex = Math.Max(NumberOfVertices(attribute) - 1, maximumArrayIndex);
                    }
                }

                dirty = false;
                this.maximumArrayIndex = maximumArrayIndex;
            }
        }

        private void Attach(int index)
        {
            A.GL.EnableVertexAttribArray(index);

            VertexBufferAttribute attribute = attributes[index].VertexBufferAttribute;
            VertexBuffer bufferObjectGL = attribute.VertexBuffer as VertexBuffer;

            bufferObjectGL.Bind();
            A.GL.VertexAttribPointer(index,
                attribute.NumberOfComponents,
                attribute.ComponentDatatype,
                attribute.Normalize,
                attribute.StrideInBytes,
                attribute.OffsetInBytes);
        }

        private static void Detach(int index)
        {
            A.GL.DisableVertexAttribArray(index);
        }

        internal int MaximumArrayIndex
        {
            get
            {
                if (dirty)
                {
                    throw new InvalidOperationException("MaximumArrayIndex is not valid until Clean() is called.");
                }

                return maximumArrayIndex;
            }
        }

        private static int NumberOfVertices(VertexBufferAttribute attribute)
        {
            return attribute.VertexBuffer.SizeInBytes / attribute.StrideInBytes;
        }

        private VertexBufferAttributeGL3x[] attributes;
        private int count;
        private int maximumArrayIndex;
        private bool dirty;

    }
}
