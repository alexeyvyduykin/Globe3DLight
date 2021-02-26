using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using OpenTK;
using OpenTK.Graphics;

namespace Globe3DLight.AvaloniaUI.OpenTK__
{
    //public interface IOpenGlBitmapImpl : IBitmapImpl
    //{
    //    IOpenGlBitmapAttachment CreateFramebufferAttachment(IGraphicsContext/*IGlContext*/ context, Action presentCallback);

    // //   bool SupportsContext(IGraphicsContext/*IGlContext*/ context);
    //}

    //public interface IOpenGlBitmapAttachment : IDisposable
    //{
    //    void Present();
    //}

    //public interface IOpenGlAwarePlatformRenderInterface
    //{
    //    IOpenGlBitmapImpl CreateOpenGlBitmap(PixelSize size, Vector dpi);
    //}

    //public class OpenGlBitmap : Avalonia.Media.Imaging.Bitmap, IAffectsRender
    //{
    //    private IBitmap /*IOpenGlBitmapImpl*/ _impl;

    //    public OpenGlBitmap(PixelSize size, Vector dpi) : base(size, dpi)
    //       // : base(CreateOrThrow(size, dpi))
    //    {
    //        _impl = (IOpenGlBitmapImpl)PlatformImpl.Item;
    //    }

    //    static IOpenGlBitmapImpl CreateOrThrow(PixelSize size, Vector dpi)
    //    {


    //        if (!(AvaloniaLocator.Current.GetService<IPlatformRenderInterface>() is IOpenGlAwarePlatformRenderInterface
    //            glAware))
    //            throw new PlatformNotSupportedException("Rendering platform does not support OpenGL integration");
    //        return glAware.CreateOpenGlBitmap(size, dpi);
    //    }

    //    public IOpenGlBitmapAttachment CreateFramebufferAttachment(IGraphicsContext/*IGlContext*/ context) =>
    //        _impl.CreateFramebufferAttachment(context, SetIsDirty);

    // //   public bool SupportsContext(IGraphicsContext/*IGlContext*/ context) => _impl.SupportsContext(context);

    //    void SetIsDirty()
    //    {
    //        if (Dispatcher.UIThread.CheckAccess())
    //            CallInvalidated();
    //        else
    //            Dispatcher.UIThread.Post(CallInvalidated);
    //    }

    //    private void CallInvalidated() => Invalidated?.Invoke(this, EventArgs.Empty);

    //    public event EventHandler Invalidated;
    //}
}
