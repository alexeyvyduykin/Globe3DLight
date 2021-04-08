#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    // providers that are already build in  
    public class SCTimeSchedulerProviders
    {
        static SCTimeSchedulerProviders()
        {
            list = new List<SCTimeSchedulerProvider>();

            Type type = typeof(SCTimeSchedulerProviders);
            foreach (var p in type.GetFields())
            {
                var v = p.GetValue(null) as SCTimeSchedulerProvider; // static classes cannot be instanced, so use null...
                if (v != null)
                {
                    list.Add(v);
                }
            }

            Hash = new Dictionary<Guid, SCTimeSchedulerProvider>();
            foreach (var p in list)
            {
                Hash.Add(p.Id, p);
            }
        }

        SCTimeSchedulerProviders() { }

        public static readonly EmptyProvider EmptyProvider = EmptyProvider.Instance;

        public static readonly CommonTimeSchedulerProvider CommonTimeScheduler = CommonTimeSchedulerProvider.Instance;

        static List<SCTimeSchedulerProvider> list;

        // get all instances of the supported providers     
        public static List<SCTimeSchedulerProvider> List
        {
            get
            {
                return list;
            }
        }

        static Dictionary<Guid, SCTimeSchedulerProvider> Hash;

        public static SCTimeSchedulerProvider TryGetProvider(Guid id)
        {
            SCTimeSchedulerProvider ret;
            if (Hash.TryGetValue(id, out ret))
            {
                return ret;
            }
            return null;
        }
    }



    // base class for each scheduler provider
    public abstract class SCTimeSchedulerProvider
    {
        // unique provider id     
        public abstract Guid Id { get; }

        // provider name     
        public abstract string Name { get; }

        // provider projection    
        public abstract BaseProjection Projection { get; }

        static readonly List<SCTimeSchedulerProvider> SchedulerProviders = new List<SCTimeSchedulerProvider>();

        protected SCTimeSchedulerProvider()
        {
            if (SchedulerProviders.Exists(p => p.Id == Id))
            {
                throw new Exception("such provider id already exsists, try regenerate your provider guid...");
            }

            SchedulerProviders.Add(this);
        }

        static SCTimeSchedulerProvider() { }

        bool isInitialized = false;

        // was provider initialized   
        public bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
            internal set
            {
                isInitialized = value;
            }
        }

        // called before first use       
        public virtual void OnInitialized()
        {
            // nice place to detect current provider version
        }

        // area of scheduler       
        public RectD? Area;

        // minimum level of zoom     
        public int MinZoom;

        // maximum level of zoom    
        public int? MaxZoom = 17;

        // true if axis origin at BottomLeft 
        public bool InvertedAxisY = false;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SCTimeSchedulerProvider)
            {
                return Id.Equals((obj as SCTimeSchedulerProvider).Id);
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    // represents empty provider 
    public class EmptyProvider : SCTimeSchedulerProvider
    {
        public static readonly EmptyProvider Instance;

        EmptyProvider()
        {
            MaxZoom = null;
        }

        static EmptyProvider()
        {
            Instance = new EmptyProvider();
        }

        public override Guid Id
        {
            get
            {
                return Guid.Empty;
            }
        }

        readonly string name = "None";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        readonly CommonSchedulerProjection projection = CommonSchedulerProjection.Instance;
        public override BaseProjection Projection
        {
            get
            {
                return projection;
            }
        }

    }
}
