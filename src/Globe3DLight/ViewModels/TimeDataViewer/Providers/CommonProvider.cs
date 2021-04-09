#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class TimeSchedulerProviderBase : BaseTimeSchedulerProvider
    {
        public TimeSchedulerProviderBase()
        {
            MaxZoom = null;
        }

        public override Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override BaseProjection Projection
        {
            get
            {
                return CommonSchedulerProjection.Instance;
            }
        }

        static bool init = false;

        public override void OnInitialized()
        {
            if (init == false)
            {
                try
                {
                    init = true; // try it only once
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Initialize failed: " + ex.ToString());
                }
            }
        }
    }

    // GoogleMap provider   
    public class CommonTimeSchedulerProvider : TimeSchedulerProviderBase
    {
        private readonly string _name = "GoogleMap";
        public static readonly CommonTimeSchedulerProvider Instance;

        public CommonTimeSchedulerProvider() { }

        static CommonTimeSchedulerProvider()
        {
            Instance = new CommonTimeSchedulerProvider();
        }

        private readonly Guid id = new Guid("D7287DA0-A7FF-405F-8166-B7BAF26D066C");

        public override Guid Id => id;

        public override string Name => _name;
    }
}
