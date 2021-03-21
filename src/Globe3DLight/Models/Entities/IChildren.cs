using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Entities
{
    public interface IChildren //: IObservableObject
    {
        ImmutableArray<BaseEntity> Children { get; set; }
    }
}
