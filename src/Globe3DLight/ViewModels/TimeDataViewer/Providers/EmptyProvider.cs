using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class EmptyProvider : BaseTimeSchedulerProvider
    {
        private readonly string _name = "None";
        private readonly CommonSchedulerProjection _projection = CommonSchedulerProjection.Instance;
        private static readonly EmptyProvider s_instance;

        static EmptyProvider()
        {
            s_instance = new EmptyProvider();
        }

        public EmptyProvider()
        {
            MaxZoom = null;
        }

        public static EmptyProvider Instance => s_instance;

        public override Guid Id => Guid.Empty;

        public override string Name => _name;

        public override BaseProjection Projection => _projection;
    }
}
