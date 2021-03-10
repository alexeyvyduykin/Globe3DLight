using Globe3DLight.Image;
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Globe3DLight.Renderer;

namespace Globe3DLight
{
    public class ImageLibrary : IImageLibrary
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IImageLoader _imageLoader;

        private readonly IDictionary<string, string> _dictionary;

        private Thread _thread;

        private IDdsImage _currentImage;

        private string _targetKey;

        public ImageLibrary(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _imageLoader = _serviceProvider.GetService<IImageLoader>();

            _dictionary = new Dictionary<string, string>();
          
            State = IImageLibrary.ImageLibraryState.None;
        }

        public IImageLibrary.ImageLibraryState State { get; protected set; }
      
        public void AddKey(string key, string path)
        {
            if (File.Exists(path) == true)
            {
                if (_dictionary.ContainsKey(key) == false)
                {
                    _dictionary.Add(key, path);
                }
                else
                {
                    _dictionary[key] = path;
                }

                return;
            }

            throw new Exception();
        }

        public void AddKeys(IEnumerable<(string key, string path)> pairs)
        {
            foreach (var (key, path) in pairs)
            {
                AddKey(key, path);
            }
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }



        public void Pass(IThreadLoadingNode node, ICache<string, int> textureCache)
        { 
            var key = node.WaitKey;

            if (textureCache.Get(key) != default)
            {
                var name = textureCache.Get(key);
                node.SetName(name);
            }            
            else if (State == IImageLibrary.ImageLibraryState.None)
            {
                _targetKey = key;
                _thread = new Thread(LoadImage);
                _thread.Start();
                State = IImageLibrary.ImageLibraryState.Loading;
            }
            else if(State == IImageLibrary.ImageLibraryState.Loading)
            {
                if(_thread.IsAlive == false)
                {
                    State = IImageLibrary.ImageLibraryState.Ready;
                }
            }
            else if (State == IImageLibrary.ImageLibraryState.Ready)
            {
                if (_targetKey == key)
                {              
                    var name = node.SetImage(_currentImage);
                    textureCache.Set(key, name);

                    State = IImageLibrary.ImageLibraryState.None;
                }
            }
        }

        private void LoadImage()
        {         
            if (_dictionary.ContainsKey(_targetKey) == true)
            {
                _currentImage = _imageLoader.LoadDdsImageFromFile(_dictionary[_targetKey]);

                return;
            }

            throw new Exception();
        }

    }
}
