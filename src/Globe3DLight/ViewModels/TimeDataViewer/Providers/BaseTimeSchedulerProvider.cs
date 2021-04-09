using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class BaseTimeSchedulerProvider
    {
        private static readonly List<BaseTimeSchedulerProvider> s_schedulerProviders;
        private bool _isInitialized = false;

        static BaseTimeSchedulerProvider()
        {
            s_schedulerProviders = new List<BaseTimeSchedulerProvider>();
        }

        protected BaseTimeSchedulerProvider()
        {
            if (s_schedulerProviders.Exists(p => p.Id == Id))
            {
                throw new Exception("such provider id already exsists, try regenerate your provider guid...");
            }

            s_schedulerProviders.Add(this);
        }
     
        public abstract Guid Id { get; }

        public abstract string Name { get; }

        public abstract BaseProjection Projection { get; }
        
        public bool IsInitialized
        {
            get => _isInitialized;            
            internal set => _isInitialized = value;            
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

        public override bool Equals(object? obj)
        {
            if (obj is BaseTimeSchedulerProvider baseTimeSchedulerProvider)
            {
                return Id.Equals(baseTimeSchedulerProvider.Id);
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
