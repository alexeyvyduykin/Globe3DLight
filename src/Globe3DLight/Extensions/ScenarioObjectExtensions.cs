using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.Models.Entities;

namespace Globe3DLight
{
    public static class ScenarioObjectExtensions
    {
        public static void AddChild(this BaseEntity obj, BaseEntity child)
        {
            if (child != null)
            {
                var builder = obj.Children.ToBuilder();

                child.Owner = obj;

                builder.Add(child);

                obj.Children = builder.ToImmutable();
            }
        }

        public static void AddChildren(this BaseEntity obj, IEnumerable<BaseEntity> children)
        {
            if (children != null)
            {
                var builder = obj.Children.ToBuilder();

                foreach (var item in children)
                {
                    if (item != null)
                    {
                        item.Owner = obj;

                        builder.Add(item);
                    }
                }

                obj.Children = builder.ToImmutable();
            }
        }

        public static void AddAssets<T>(this IAssetable<T> assetable, IEnumerable<T> objects)
        {
            if (assetable.Assets != null && objects != null)
            {
                var builder = assetable.Assets.ToBuilder();
                builder.AddRange(objects);
                assetable.Assets = builder.ToImmutable();
            }
        }
    }
}
