using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;


namespace Globe3DLight
{
    public interface IImageLoader 
    {           
        IDdsImage LoadDdsImageFromFile(string path);
   
        IEnumerable<IDdsImage> LoadDdsImageFromFiles(IEnumerable<string> paths);
    }
}
