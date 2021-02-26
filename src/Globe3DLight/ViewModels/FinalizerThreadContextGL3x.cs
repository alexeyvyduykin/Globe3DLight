using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using OpenTK;
//using OpenTK.Graphics;

namespace Globe3DLight.Renderer
{
    //public static class FinalizerThreadContextGL3x
    //{
    //    static FinalizerThreadContextGL3x()
    //    {
    //        _window = new NativeWindow();
    //        _context = new GraphicsContext(new GraphicsMode(32, 24, 8), _window.WindowInfo, 3, 2, GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug);
    //    }

    //    public static void Initialize()
    //    {
    //    }

    //    public delegate void DisposeCallback(bool disposing);

    //    public static void RunFinalizer(DisposeCallback callback)
    //    {
    //        try
    //        {
    //            if (!_context.IsDisposed)
    //            {
    //                _context.MakeCurrent(_window.WindowInfo);
    //                try
    //                {
    //                    callback(false);
    //                }
    //                finally
    //                {
    //                    _context.MakeCurrent(null);
    //                }
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private static NativeWindow _window;
    //    private static GraphicsContext _context;
    //}
}
