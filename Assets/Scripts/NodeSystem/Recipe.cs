using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public partial class Recipe {
        public readonly ImmutableList<IStaticNode> From;
        public readonly ImmutableList<IStaticNode> To;
        public readonly int MergeTime;

        private Recipe(IEnumerable<IStaticNode> from, IEnumerable<IStaticNode> to, int mergeTime) {
            From = OrderByType(from).ToImmutableList();
            To = OrderByType(to).ToImmutableList();
            MergeTime = mergeTime;
        }
    }
    public partial class Recipe {
        public readonly static ImmutableHashSet<Recipe> Recipes;

        static Recipe() {
            var immutableHashSetBuilder = ImmutableHashSet.CreateBuilder<Recipe>();

            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Wood.StaticNode, Wood.StaticNode },
                new IStaticNode[] { WoodPlatter.StaticNode },
                2400
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Wood.StaticNode, WoodPlatter.StaticNode },
                new IStaticNode[] { Folder.StaticNode },
                1000
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Wood.StaticNode, Wood.StaticNode, Wood.StaticNode, Wood.StaticNode },
                new IStaticNode[] { Tree.StaticNode },
                3400
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Stone.StaticNode, Stone.StaticNode, Stone.StaticNode, Stone.StaticNode },
                new IStaticNode[] { Cave.StaticNode },
                4300
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Stone.StaticNode, Stick.StaticNode },
                new IStaticNode[] { AxeStone.StaticNode },
                4300
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Cave.StaticNode, IronOre.StaticNode, },
                new IStaticNode[] { IronIngot.StaticNode },
                4300
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new IStaticNode[] { Cave.StaticNode, CopperOre.StaticNode, },
                new IStaticNode[] { CopperIngot.StaticNode },
                4300
            ));

            Recipes = immutableHashSetBuilder.ToImmutable();
        }

        static private IEnumerable<IStaticNode> OrderByType(IEnumerable<IStaticNode> types) {
            return types.OrderBy(t => { var type = t.GetType(); return $"{type.Namespace}#{type.Name}"; });
        }

        static public Recipe? GetRecipe(IEnumerable<IStaticNode> from) {
            var recipe = Recipes.FirstOrDefault(r => r.From.SequenceEqual(OrderByType(from)));
            return recipe;
        }
    }
}