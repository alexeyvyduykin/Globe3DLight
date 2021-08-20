using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeDataViewer.Core
{
    public abstract partial class Model
    {
        private readonly object _syncRoot = new();

        protected Model()
        {

        }

        public object SyncRoot
        {
            get { return _syncRoot; }
        }

        public IEnumerable<HitTestResult> HitTest(HitTestArguments args)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in GetElements().Reverse())
            {
                var result = element.HitTest(args);
                if (result != null)
                {
                    yield return result;
                }
            }
        }

        public abstract IEnumerable<UIElement> GetElements();
    }
}
