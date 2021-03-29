using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.ViewModels.Data
{
    public class BaseState : LogicalViewModel
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
                if(parent is LogicalCollectionViewModel)
                {
                    parent = parent.Owner;
                    continue;
                }

                modelMatrix = ((BaseState)parent).ModelMatrix * modelMatrix;

                parent = parent.Owner;
            }

            return modelMatrix;
        }
    }
}
