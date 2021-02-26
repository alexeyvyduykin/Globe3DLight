using System;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Media;
//using Avalonia.OpenGL.Imaging;
//using static Avalonia.OpenGL.GlConsts;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;
using Avalonia;
using Avalonia.Media.Imaging;

namespace Globe3DLight.AvaloniaUI.OpenTK__
{


    public class OpenGlControlCustom : UserControl//Control
    {
        private IGraphicsContext _context;
      //  private IGlContext _context;
        private int _fb, _depthBuffer;
        private WriteableBitmap/* OpenGlBitmap*/ _bitmap;
     //   private IOpenGlBitmapAttachment _attachment;
        private PixelSize _depthBufferSize;
        private bool _glFailed;
        private bool _initialized;
        //protected GlVersion GlVersion { get; private set; }

        private GameWindow _gameWindow;


        public override void Render(DrawingContext context)
        {        
            if (EnsureInitialized() == false)
            {
                return;
            }

            //_context.MakeCurrent((OpenTK.Platform.IWindowInfo)_gameWindow);
            //using (_context.MakeCurrent())
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fb);
                //_context.GlInterface.BindFramebuffer(GL_FRAMEBUFFER, _fb);
                EnsureTextureAttachment();
                
                EnsureDepthBufferAttachment(_context/*.GlInterface*/);
                
                if (CheckFramebufferStatus(_context/*.GlInterface*/) == false)
                {
                    return;
                }

                OnOpenGlRender(_context/*.GlInterface*/, _fb);

            //    _attachment.Present();
            }

            context.DrawImage(_bitmap, new Rect(_bitmap.Size), Bounds);
            base.Render(context);
        }

        private void CheckError(IGraphicsContext/*GlInterface*/ gl)
        {
            ErrorCode err;
            while ((err = GL.GetError()) != ErrorCode.NoError)
                Console.WriteLine(err);
        }

        void EnsureTextureAttachment()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fb);
            //_context.BindFramebuffer(GL_FRAMEBUFFER, _fb);
            if (_bitmap == null || /*_attachment == null ||*/ _bitmap.PixelSize != GetPixelSize())
            {
           //     _attachment?.Dispose();
          //      _attachment = null;
                _bitmap?.Dispose();
                _bitmap = null;
                _bitmap = new WriteableBitmap(GetPixelSize(), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
             //   _bitmap = new OpenGlBitmap(GetPixelSize(), new Vector(96, 96));
            //    _attachment = _bitmap.CreateFramebufferAttachment(_context);
            }
        }

        void EnsureDepthBufferAttachment(IGraphicsContext/*GlInterface*/ gl)
        {
            var size = GetPixelSize();
            if (size == _depthBufferSize && _depthBuffer != 0)
                return;

            GL.GetInteger/*v*/(GetPName.RenderbufferBinding, out var oldRenderBuffer);
            if (_depthBuffer != 0)
            {
                GL.DeleteRenderbuffers(1, new[] { _depthBuffer });
            }

            var oneArr = new int[1];
            GL.GenRenderbuffers(1, oneArr);
            _depthBuffer = oneArr[0];
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _depthBuffer);

            // if OpenTK.Graphics.OpenGL
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, size.Width, size.Height);
            // if OpenTK.Graphics.OpenGL4
            //GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent16, size.Width, size.Height);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, _depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, oldRenderBuffer);
        }

        void DoCleanup()
        {
            if (_context != null)
            {
                //_context.MakeCurrent((OpenTK.Platform.IWindowInfo)_gameWindow);
                //using (_context.MakeCurrent())
                {
                    var gl = _context;//.GlInterface;
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                    GL.DeleteFramebuffers(1, new[] { _fb });
                    GL.DeleteRenderbuffers(1, new[] { _depthBuffer });
             //       _attachment?.Dispose();
             //       _attachment = null;
                    _bitmap?.Dispose();
                    _bitmap = null;

                    try
                    {
                        if (_initialized)
                        {
                            _initialized = false;
                            OnOpenGlDeinit(_context/*.GlInterface*/, _fb);
                        }
                    }
                    finally
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            DoCleanup();
            base.OnDetachedFromVisualTree(e);
        }

        private bool EnsureInitializedCore()
        {
            if (_context != null)
                return true;

            if (_glFailed)
                return false;

            //var feature = AvaloniaLocator.Current.GetService<IPlatformOpenGlInterface>();
            //if (feature == null)
            //    return false;
            //if (!feature.CanShareContexts)
            //{
            //    Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
            //        "Unable to initialize OpenGL: current platform does not support multithreaded context sharing");
            //    return false;
            //}
            try
            {
                _gameWindow = new GameWindow(100, 100);

                _gameWindow.Visible = false;
                _gameWindow.MakeCurrent();

                _context = _gameWindow.Context;// feature.CreateSharedContext();
            }
            catch (Exception e)
            {
                Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
                    "Unable to initialize OpenGL: unable to create additional OpenGL context: {exception}", e);
                return false;
            }

            //GlVersion = _context.Version;
            try
            {

                _bitmap = new WriteableBitmap(GetPixelSize(), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);
            //    _bitmap =  new OpenGlBitmap(GetPixelSize(), new Vector(96, 96));
             //   if (_bitmap.SupportsContext(_context) == false)
             //   {
             //       Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
             //           "Unable to initialize OpenGL: unable to create OpenGlBitmap: OpenGL context is not compatible");
             //       return false;
            //    }
            }
            catch (Exception e)
            {
                _context.Dispose();
                _context = null;
                Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
                    "Unable to initialize OpenGL: unable to create OpenGlBitmap: {exception}", e);
                return false;
            }

            //_context.MakeCurrent((OpenTK.Platform.IWindowInfo)_gameWindow);
            //using (_context.MakeCurrent())
            {
                try
                {
                    _depthBufferSize = GetPixelSize();
                    var gl = _context;//.GlInterface;
                    var oneArr = new int[1];
                    GL.GenFramebuffers(1, oneArr);
                    _fb = oneArr[0];
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fb);

                    EnsureDepthBufferAttachment(gl);
                    EnsureTextureAttachment();

                    return CheckFramebufferStatus(gl);
                }
                catch (Exception e)
                {
                    Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
                        "Unable to initialize OpenGL FBO: {exception}", e);
                    return false;
                }
            }
        }

        private bool CheckFramebufferStatus(IGraphicsContext/*GlInterface*/ gl)
        {
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                ErrorCode code;
                while ((code = GL.GetError()) != 0)
                    Logger.TryGet(LogEventLevel.Error, "OpenGL")?.Log("OpenGlControlBase",
                        "Unable to initialize OpenGL FBO: {code}", code);
                return false;
            }

            return true;
        }

        private bool EnsureInitialized()
        {
            if (_initialized)
            {
                return true;
            }

            _glFailed = !(_initialized = EnsureInitializedCore());
           
            if (_glFailed)
            {
                return false;
            }

            //_context.MakeCurrent((OpenTK.Platform.IWindowInfo)_gameWindow);
            //using (_context.MakeCurrent())
            {
                OnOpenGlInit(_context/*.GlInterface*/, _fb);
            }

            return true;
        }

        private PixelSize GetPixelSize()
        {
            var scaling = VisualRoot.RenderScaling;
            return new PixelSize(Math.Max(1, (int)(Bounds.Width * scaling)),
                Math.Max(1, (int)(Bounds.Height * scaling)));
        }


        protected virtual void OnOpenGlInit(IGraphicsContext/*GlInterface*/ gl, int fb)
        {

        }

        protected virtual void OnOpenGlDeinit(IGraphicsContext/*GlInterface*/ gl, int fb)
        {

        }

        protected /*abstract*/ void OnOpenGlRender(IGraphicsContext/*GlInterface*/ gl, int fb)
        {
            GL.ClearColor(0.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);       
        }
    }



}
