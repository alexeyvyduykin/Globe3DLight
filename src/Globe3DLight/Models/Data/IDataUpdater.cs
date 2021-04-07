using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.Models.Data
{
    public interface IDataUpdater
    {
        void Update(double t, FrameViewModel frame);
    }
}
