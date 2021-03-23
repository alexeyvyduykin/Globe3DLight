using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class VertexArray : Disposable
    {
        public VertexArray()
        {
            name = GL.GenVertexArray();
            attributes = new VertexBufferAttributes();
        }

        public void Bind()
        {
            GL.BindVertexArray(name);
        }

        public void Clean()
        {
            attributes.Clean();

            if (dirtyIndexBuffer)
            {
                if (indexBuffer != null)
                {
                    indexBuffer.Bind();
                }
                else
                {
                    IndexBuffer.UnBind(); 
                }

                dirtyIndexBuffer = false;
            }
        }

        public int MaximumArrayIndex()
        {
            return attributes.MaximumArrayIndex;
        }

        public VertexBufferAttributes Attributes
        {
            get { return attributes; }
        }

        public IndexBuffer IndexBuffer
        {
            get { return indexBuffer; }

            set
            {
                indexBuffer = value;
                dirtyIndexBuffer = true;
            }
        }

        public bool DisposeBuffers { get; set; }

        #region Disposable Members

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
                    HashSet<VertexBuffer> vertexBuffers = new HashSet<VertexBuffer>();

                    foreach (VertexBufferAttribute attribute in attributes)
                    {
                        vertexBuffers.Add(attribute.VertexBuffer);
                    }

                    foreach (VertexBuffer vb in vertexBuffers)
                    {
                        vb.Dispose();
                    }

                    if (indexBuffer != null)
                    {
                        indexBuffer.Dispose();
                    }
                }

                if (name != 0)
                {
                    GL.DeleteVertexArray(name);
                    name = 0;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private int name;
        private VertexBufferAttributes attributes;
        private IndexBuffer indexBuffer;
        private bool dirtyIndexBuffer;
    }

}
