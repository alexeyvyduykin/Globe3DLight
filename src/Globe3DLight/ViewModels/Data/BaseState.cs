using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight.ViewModels.Data
{
    public class BaseState : ViewModelBase
    {
        private dmat4 _modelMatrix;

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => RaiseAndSetIfChanged(ref _modelMatrix, value);
        }

        public dmat4 AbsoluteModelMatrix => GetAbsoluteModelMatrix();

        protected dmat4 GetAbsoluteModelMatrix()
        {
            var parent = Owner;
            var modelMatrix = _modelMatrix;

            while (parent is not null)
            {
                modelMatrix = ((BaseState)parent).ModelMatrix * modelMatrix;

                parent = parent.Owner;
            }

            return modelMatrix;
        }
    }
}
