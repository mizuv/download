using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using System.Linq;


namespace Download.NodeSystem {
    public class MergeManager : AsyncJobManager {
        public readonly IReadOnlyReactiveProperty<bool> IsActive;
        public readonly Recipe Recipe;

        public MergeManager(IEnumerable<IMergeable> mergeables, Recipe recipe) : base(mergeables.Select(m => m.GetDisposable()), new(recipe.MergeTime)) {
            Recipe = recipe;
            IsActive = mergeables
                .Select(m => m.IsMergeActive)
                .CombineLatestEvenEmitOnEmpty()
                .Select(values => values.Count == 0 ? false : values.All(active => active))
                .ToReactiveProperty();

            var subscription = IsActive
                .Subscribe(isActive => {
                    if (isActive) return;
                    StopRun();
                });
            _disposablesList.ForEach(disposables => disposables.Add(subscription));
        }
        public void StartRun() {
            base.Run();
        }


    }
}