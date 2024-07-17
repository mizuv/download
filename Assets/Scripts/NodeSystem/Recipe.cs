
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public partial class Recipe {
        public readonly ImmutableList<Type> From;
        public readonly ImmutableList<Type> To;

        private Recipe(IEnumerable<Type> from, IEnumerable<Type> to) {
            From = OrderType(from).ToImmutableList();
            To = OrderType(to).ToImmutableList();
        }
    }
    public partial class Recipe {
        public readonly static ImmutableHashSet<Recipe> Recipes;

        static Recipe() {
            var immutableHashSetBuilder = ImmutableHashSet.CreateBuilder<Recipe>();

            immutableHashSetBuilder.Add(new Recipe(
                new Type[] { typeof(Tree), typeof(Tree), typeof(Forest) },
                new Type[] { typeof(Folder) }
            ));

            Recipes = immutableHashSetBuilder.ToImmutable();
        }

        static private IEnumerable<Type> OrderType(IEnumerable<Type> types) {
            return types.OrderBy(t => t.Namespace).ThenBy(t => t.Name);
        }

        static public ImmutableList<Type> GetRecipe(IEnumerable<Type> from) {
            var recipe = Recipes.FirstOrDefault(r => r.From.SequenceEqual(OrderType(from)));
            return recipe.To;
        }
    }
}