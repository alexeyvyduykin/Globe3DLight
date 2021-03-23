using System;
using Avalonia;
//using Avalonia.Controls.ApplicationLifetimes;
//using Avalonia.Logging.Serilog;
//using Avalonia.ReactiveUI;

namespace Globe3DLight.Desktop
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        //public static void Main(string[] args) => BuildAvaloniaApp()
        //    .StartWithClassicDesktopLifetime(args);

        [STAThread]
        internal static void Main(string[] args)
        {
            var builder = BuildAvaloniaApp();

            try
            {
                builder.StartWithClassicDesktopLifetime(args);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<Globe3DLight.App>()
                .UsePlatformDetect()
                .LogToTrace();         
    }

}
