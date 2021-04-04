#nullable enable
using System;
using System.Collections;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal struct VertexBufferAttributeGL3x
    {
        public VertexBufferAttribute? VertexBufferAttribute { get; set; }

        public bool Dirty { get; set; }
    }

    internal class VertexBufferAttributes
    {
        private readonly Device _device;
        private readonly VertexBufferAttributeGL3x[] _attributes;
        private int _count;
        private int _maximumArrayIndex;
        private bool _dirty;

        public VertexBufferAttributes()
        {
            _device = new Device();
            _attributes = new VertexBufferAttributeGL3x[_device.MaximumNumberOfVertexAttributes];
        }

        public virtual VertexBufferAttribute? this[int index]
        {
            get 
            {
                return _attributes[index].VertexBufferAttribute; 
            }
            set
            {
                if (_attributes[index].VertexBufferAttribute != value)
                {
                    if (value != null)
                    {
                        if (value.NumberOfComponents < 1 || value.NumberOfComponents > 4)
                        {
                            throw new ArgumentException("NumberOfComponents must be between one and four.");
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
                                throw new ArgumentException("When Normalize is true, ComponentDatatype must be Byte, UnsignedByte, Short, UnsignedShort, Int, or UnsignedInt.");
                            }
                        }
                    }

                    if ((_attributes[index].VertexBufferAttribute != null) && (value == null))
                    {
                        --_count;
                    }
                    else if ((_attributes[index].VertexBufferAttribute == null) && (value != null))
                    {
                        ++_count;
                    }

                    _attributes[index].VertexBufferAttribute = value;
                    _attributes[index].Dirty = true;
                    _dirty = true;
                }
            }
        }

        public virtual int Count => _count;         

        public virtual int MaximumCount => _attributes.Length;         

        public virtual IEnumerator GetEnumerator()
        {
            foreach (var attribute in _attributes)
            {
                if (attribute.VertexBufferAttribute != null)
                {
                    yield return attribute.VertexBufferAttribute;
                }
            }
        }

        internal void Clean()
        {
            if (_dirty)
            {
                int maximumArrayIndex = 0;

                for (int i = 0; i < _attributes.Length; ++i)
                {
                    var attribute = _attributes[i].VertexBufferAttribute;

                    if (_attributes[i].Dirty)
                    {
                        if (attribute != null)
                        {
                            Attach(i);
                        }
                        else
                        {
                            Detach(i);
                        }

                        _attributes[i].Dirty = false;
                    }

                    if (attribute != null)
                    {
                        maximumArrayIndex = Math.Max(NumberOfVertices(attribute) - 1, maximumArrayIndex);
                    }
                }

                _dirty = false;
                _maximumArrayIndex = maximumArrayIndex;
            }
        }

        private void Attach(int index)
        {
            A.GL.EnableVertexAttribArray(index);

            var attribute = _attributes[index].VertexBufferAttribute;

            if (attribute != null)
            {
                var bufferObjectGL = attribute.VertexBuffer;

                bufferObjectGL.Bind();

                A.GL.VertexAttribPointer(index,
                    attribute.NumberOfComponents,
                    attribute.ComponentDatatype,
                    attribute.Normalize,
                    attribute.StrideInBytes,
                    attribute.OffsetInBytes);
            }
        }

        private static void Detach(int index)
        {
            A.GL.DisableVertexAttribArray(index);
        }

        internal int MaximumArrayIndex
        {
            get
            {
                if (_dirty)
                {
                    throw new InvalidOperationException("MaximumArrayIndex is not valid until Clean() is called.");
                }

                return _maximumArrayIndex;
            }
        }

        private static int NumberOfVertices(VertexBufferAttribute attribute)
        {
            return attribute.VertexBuffer.SizeInBytes / attribute.StrideInBytes;
        }
    }
}
