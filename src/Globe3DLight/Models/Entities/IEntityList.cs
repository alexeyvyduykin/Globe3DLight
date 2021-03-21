using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Entities
{
    public interface ICollection<T> where T : class
    {
        ImmutableArray<T> Values { get; set; }
    }

    //public interface ICollectionValue
    //{

    //}

    //public interface IEntityList : IEntity, IDrawable, ICollection<IEntity>
    //{   
        
    //}
}
