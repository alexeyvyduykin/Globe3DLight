using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;
using A = Pfim;


namespace Globe3DLight.ImageLoader.Pfim
{
    internal class PfimFactory
    {
        public IDdsImage CreateDdsImage(A.IImage image) 
        {
            if (image is A.Dds ddsImage)
            {
                return new PfimDdsImage(ddsImage);
            }

            return null;
        }
    }
}
