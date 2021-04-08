#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class TimeSchedulerProviderBase : SCTimeSchedulerProvider
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

        public override SCProjection Projection
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
        public static readonly CommonTimeSchedulerProvider Instance;

        CommonTimeSchedulerProvider() { }

        static CommonTimeSchedulerProvider()
        {
            Instance = new CommonTimeSchedulerProvider();
        }

        readonly Guid id = new Guid("D7287DA0-A7FF-405F-8166-B7BAF26D066C");
        public override Guid Id
        {
            get
            {
                return id;
            }
        }

        readonly string name = "GoogleMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }
    }
}
