using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKTextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class TextureUnit : ICleanable
    {
        public TextureUnit(int index, ICleanableObserver observer)
        {
            this.textureUnitIndex = index;
            this.textureUnit = OpenTKTextureUnit.Texture0 + index;
            this.observer = observer;
        }

        public Texture2D Texture
        {
            get { return texture; }

            set
            {
                if (texture != value)
                {
                    if (dirtyFlags == DirtyFlags.None)
                    {
                        observer.NotifyDirty(this);
                    }

                    dirtyFlags |= DirtyFlags.Texture;
                    texture = value;
                }
            }
        }

        //public TextureSamplerGL3x TextureSampler
        //{
        //    get { return textureSampler; }

        //    set
        //    {
        //        if (textureSampler != value)
        //        {
        //            if (dirtyFlags == DirtyFlags.None)
        //            {
        //                observer.NotifyDirty(this);
        //            }

        //            dirtyFlags |= DirtyFlags.TextureSampler;
        //            textureSampler = value;
        //        }
        //    }
        //}

        //internal void CleanLastTextureUnit()
        //{
        //    //
        //    // If the last texture unit has a texture attached, it
        //    // is cleaned even if it isn't dirty because the last
        //    // texture unit is used for texture uploads and downloads in
        //    // Texture2DGL3x, the texture unit could be dirty without
        //    // knowing it.
        //    //
        //    if (texture != null)
        //    {
        //        dirtyFlags |= DirtyFlags.Texture;
        //    }

        //    Clean();
        //}

        #region ICleanable Members

        public void Clean()
        {
            if (dirtyFlags != DirtyFlags.None)
            {
                //Validate();

                A.GL.ActiveTexture(textureUnit);

                if ((dirtyFlags & DirtyFlags.Texture) == DirtyFlags.Texture)
                {
                    if (texture != null)
                    {
                        texture.Bind();
                    }
                    else
                    {
                        Texture2D.UnBind(A.TextureTarget.Texture2D);
                        Texture2D.UnBind(A.TextureTarget.TextureRectangle);
                    }
                }

                if ((dirtyFlags & DirtyFlags.TextureSampler) == DirtyFlags.TextureSampler)
                {
                    //if (textureSampler != null)
                    //{
                    //    //textureSampler.Bind(textureUnitIndex);
                    //}
                    //else
                    //{
                    //    TextureSamplerGL3x.UnBind(textureUnitIndex);
                    //}
                }

                dirtyFlags = DirtyFlags.None;
            }
        }

        //public void Clean()
        //{
        //    if (dirtyFlags != DirtyFlags.None)
        //    {
        //        GL.ActiveTexture(textureUnit);
        //        texture.Bind();
        //        textureSampler.Bind(textureUnitIndex);

        //        dirtyFlags = DirtyFlags.None;
        //    }      
        //}

        #endregion

        //private void Validate()
        //{
        //    if (texture != null)
        //    {
        //        if (textureSampler == null)
        //        {
        //            throw new InvalidOperationException("A texture sampler must be assigned to a texture unit with one or more bound textures.");
        //        }

        //        if (texture.Target == TextureTarget.TextureRectangle)
        //        {
        //            if (textureSampler.MinificationFilter != TextureMinFilter.Linear &&
        //                textureSampler.MinificationFilter != TextureMinFilter.Nearest)
        //            {
        //                throw new InvalidOperationException("The texture sampler is incompatible with the rectangle texture bound to the same texture unit.  Rectangle textures only support linear and nearest minification filters.");
        //            }

        //            if (textureSampler.WrapS == TextureWrapMode.Repeat ||
        //                textureSampler.WrapS == TextureWrapMode.MirroredRepeat ||
        //                textureSampler.WrapT == TextureWrapMode.Repeat ||
        //                textureSampler.WrapT == TextureWrapMode.MirroredRepeat)
        //            {
        //                throw new InvalidOperationException("The texture sampler is incompatible with the rectangle texture bound to the same texture unit.  Rectangle textures do not support repeat or mirrored repeat wrap modes.");
        //            }
        //        }
        //    }
        //}

        [Flags]
        private enum DirtyFlags
        {
            None = 0,
            Texture = 1,
            TextureSampler = 2,
            All = Texture | TextureSampler
        }

        private readonly int textureUnitIndex;
        private readonly OpenTKTextureUnit textureUnit;
        private readonly ICleanableObserver observer;
        private Texture2D texture;
       // private TextureSamplerGL3x textureSampler;
        private DirtyFlags dirtyFlags;
    }

}
