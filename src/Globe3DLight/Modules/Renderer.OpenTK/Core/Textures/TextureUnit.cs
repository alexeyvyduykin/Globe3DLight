#nullable enable
using System;
using A = OpenTK.Graphics.OpenGL;
using OpenTKTextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class TextureUnit : ICleanable
    {
        private readonly int _textureUnitIndex;
        private readonly OpenTKTextureUnit _textureUnit;
        private readonly ICleanableObserver _observer;
        private Texture2D? _texture;       
        private DirtyFlags _dirtyFlags;

        [Flags]
        private enum DirtyFlags
        {
            None = 0,
            Texture = 1,
            TextureSampler = 2,
            All = Texture | TextureSampler
        }

        public TextureUnit(int index, ICleanableObserver observer)
        {
            _textureUnitIndex = index;
            _textureUnit = OpenTKTextureUnit.Texture0 + index;
            _observer = observer;
        }

        public Texture2D? Texture
        {
            get { return _texture; }
            set
            {
                if (_texture != value)
                {
                    if (_dirtyFlags == DirtyFlags.None)
                    {
                        _observer.NotifyDirty(this);
                    }

                    _dirtyFlags |= DirtyFlags.Texture;
                    _texture = value;
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

        public void Clean()
        {
            if (_dirtyFlags != DirtyFlags.None)
            {
                //Validate();

                A.GL.ActiveTexture(_textureUnit);

                if ((_dirtyFlags & DirtyFlags.Texture) == DirtyFlags.Texture)
                {
                    if (_texture != null)
                    {
                        _texture.Bind();
                    }
                    else
                    {
                        Texture2D.UnBind(A.TextureTarget.Texture2D);
                        Texture2D.UnBind(A.TextureTarget.TextureRectangle);
                    }
                }

                if ((_dirtyFlags & DirtyFlags.TextureSampler) == DirtyFlags.TextureSampler)
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

                _dirtyFlags = DirtyFlags.None;
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
    }
}
