using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniRx;

namespace Download.NodeSystem {
    public partial class Recipe {
        public readonly ImmutableList<Type> From;
        public readonly ImmutableList<Type> To;
        public readonly int MergeTime;

        private Recipe(IEnumerable<Type> from, IEnumerable<Type> to, int mergeTime) {
            From = OrderType(from).ToImmutableList();
            To = OrderType(to).ToImmutableList();
            MergeTime = mergeTime;
        }
    }
    public partial class Recipe {
        public readonly static ImmutableHashSet<Recipe> Recipes;

        static Recipe() {
            var immutableHashSetBuilder = ImmutableHashSet.CreateBuilder<Recipe>();

            immutableHashSetBuilder.Add(new Recipe(
                new Type[] { typeof(Wood), typeof(WoodPlatter), typeof(Forest) },
                new Type[] { typeof(Folder) },
                2400
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new Type[] { typeof(Wood), typeof(Wood) },
                new Type[] { typeof(WoodPlatter) },
                1000
            ));
            immutableHashSetBuilder.Add(new Recipe(
                new Type[] { typeof(Wood), typeof(Wood), typeof(Wood), typeof(Wood) },
                new Type[] { typeof(Forest) },
                3400
            ));

            Recipes = immutableHashSetBuilder.ToImmutable();
        }

        static private IEnumerable<Type> OrderType(IEnumerable<Type> types) {
            return types.OrderBy(t => t.Namespace).ThenBy(t => t.Name);
        }

        static public Recipe? GetRecipe(IEnumerable<Type> from) {
            var recipe = Recipes.FirstOrDefault(r => r.From.SequenceEqual(OrderType(from)));
            return recipe;
        }
    }
}