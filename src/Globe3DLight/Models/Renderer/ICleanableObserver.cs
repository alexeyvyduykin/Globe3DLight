using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer
{
    public interface ICleanableObserver
    {
        void NotifyDirty(ICleanable value);
    }
}
