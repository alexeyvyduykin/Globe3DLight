using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.Models.Entities
{
    public interface IChildren
    {
        ImmutableArray<BaseEntity> Children { get; set; }
    }
}
