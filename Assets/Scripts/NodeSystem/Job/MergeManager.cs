using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using System.Linq;


namespace Download.NodeSystem {
    public class MergeManager : AsyncJobManager {
        public readonly Recipe Recipe;

        public MergeManager(IEnumerable<IMergeable> mergeables, Recipe recipe) : base(mergeables.Select(m => m.GetDisposable()), new(recipe.MergeTime)) {
            Recipe = recipe;
        }
        public void StartRun() {
            base.Run();
        }


    }
}