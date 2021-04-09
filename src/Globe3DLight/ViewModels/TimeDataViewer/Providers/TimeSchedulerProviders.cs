#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class TimeSchedulerProviders
    {
        private static readonly CommonTimeSchedulerProvider s_commonTimeScheduler;
        private static readonly List<BaseTimeSchedulerProvider> s_list;
        private static readonly Dictionary<Guid, BaseTimeSchedulerProvider> s_hash;
        private static readonly EmptyProvider s_emptyProvider;

        static TimeSchedulerProviders()
        {
            s_list = new List<BaseTimeSchedulerProvider>();

            var type = typeof(TimeSchedulerProviders);
            foreach (var p in type.GetFields())
            {
                var v = p.GetValue(null) as BaseTimeSchedulerProvider; // static classes cannot be instanced, so use null...
                if (v is not null)
                {
                    s_list.Add(v);
                }
            }

            s_hash = new Dictionary<Guid, BaseTimeSchedulerProvider>();
            foreach (var p in s_list)
            {
                s_hash.Add(p.Id, p);
            }

            s_commonTimeScheduler = CommonTimeSchedulerProvider.Instance;
            s_emptyProvider = EmptyProvider.Instance;
        }

        public TimeSchedulerProviders() 
        {
        
        }

        public static EmptyProvider EmptyProvider => s_emptyProvider;

        public static List<BaseTimeSchedulerProvider> List => s_list;

        public static BaseTimeSchedulerProvider? TryGetProvider(Guid id)
        {      
            if (s_hash.TryGetValue(id, out var ret))
            {
                return ret;
            }

            return null;
        }
    }






}
