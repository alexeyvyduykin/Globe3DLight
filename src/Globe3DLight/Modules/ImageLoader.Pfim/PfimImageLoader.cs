﻿#nullable enable
using System;
using System.Collections.Generic;
using Globe3DLight.Models;
using Globe3DLight.Models.Image;
using A = Pfim;

namespace Globe3DLight.ImageLoader.Pfim
{
    public class PfimImageLoader : IImageLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PfimFactory PfimFactory;

        public PfimImageLoader(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            PfimFactory = new PfimFactory();
        }

        public IDdsImage? LoadDdsImageFromFile(string path)
        {
            using var image = A.Pfim.FromFile(path);

            return PfimFactory.CreateDdsImage(image);
        }

        public IEnumerable<IDdsImage?> LoadDdsImageFromFiles(IEnumerable<string> paths)
        {
            var list = new List<IDdsImage?>();

            foreach (var path in paths)
            {
                using var image = A.Pfim.FromFile(path);

                list.Add(PfimFactory.CreateDdsImage(image));
            }

            return list;
        }
    }
}
