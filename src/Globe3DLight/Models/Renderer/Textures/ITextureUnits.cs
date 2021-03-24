using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Globe3DLight.Models.Renderer
{
    public interface ITextureUnits : ICleanableObserver, IEnumerable
    {
        ITextureUnit this[int index] { get; }

        int Count { get; }

        void Clean();
    }

}
