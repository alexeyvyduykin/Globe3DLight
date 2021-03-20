using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Entities;

namespace Globe3DLight
{
    public static class ScenarioObjectExtensions
    {
        public static void AddChild(this IChildren obj, IEntity child)
        {
            if (child != null)
            {
                var builder = obj.Children.ToBuilder();

                child.Owner = obj;

                builder.Add(child);

                obj.Children = builder.ToImmutable();
            }
        }

        public static void AddChildren(this IChildren obj, IEnumerable<IEntity> children)
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

        public static void AddAssets(this IAssetable assetable, IEnumerable<IEntity> objects)
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
