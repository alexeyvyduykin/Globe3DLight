#nullable enable
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class VertexArray : Disposable
    {
        private int _name;
        private VertexBufferAttributes _attributes;
        private IndexBuffer? _indexBuffer;
        private bool _dirtyIndexBuffer;

        public VertexArray()
        {
            _name = GL.GenVertexArray();
            _attributes = new VertexBufferAttributes();
        }

        public void Bind()
        {
            GL.BindVertexArray(_name);
        }

        public void Clean()
        {
            _attributes.Clean();

            if (_dirtyIndexBuffer)
            {
                if (_indexBuffer is not null)
                {
                    _indexBuffer.Bind();
                }
                else
                {
                    IndexBuffer.UnBind();
                }

                _dirtyIndexBuffer = false;
            }
        }

        public int MaximumArrayIndex() => _attributes.MaximumArrayIndex;
        
        public VertexBufferAttributes Attributes => _attributes; 
        
        public IndexBuffer? IndexBuffer
        {
            get { return _indexBuffer; }

            set
            {
                _indexBuffer = value;
                _dirtyIndexBuffer = true;
            }
        }

        public bool DisposeBuffers { get; set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DisposeBuffers)
                {
                    //
                    // Multiple components may share the same vertex buffer, so
                    // find the unique set of vertex buffers used by this vertex array.
                    //
                    var vertexBuffers = new HashSet<VertexBuffer>();

                    foreach (VertexBufferAttribute attribute in _attributes)
                    {
                        vertexBuffers.Add(attribute.VertexBuffer);
                    }

                    foreach (VertexBuffer vb in vertexBuffers)
                    {
                        vb.Dispose();
                    }

                    if (_indexBuffer is not null)
                    {
                        _indexBuffer.Dispose();
                    }
                }

                if (_name != 0)
                {
                    GL.DeleteVertexArray(_name);
                    _name = 0;
                }
            }

            base.Dispose(disposing);
        }
    }

}
