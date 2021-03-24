using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Image;

namespace Globe3DLight.Models
{
    public interface IImageLoader 
    {           
        IDdsImage LoadDdsImageFromFile(string path);
   
        IEnumerable<IDdsImage> LoadDdsImageFromFiles(IEnumerable<string> paths);
    }
}
