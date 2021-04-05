#nullable enable
using Globe3DLight.Models.Image;
using A = Pfim;

namespace Globe3DLight.ImageLoader.Pfim
{
    internal class PfimFactory
    {
        public IDdsImage? CreateDdsImage(A.IImage image)
        {
            if (image is A.Dds ddsImage)
            {
                return new PfimDdsImage(ddsImage);
            }

            return null;
        }
    }
}
