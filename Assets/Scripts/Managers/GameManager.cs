using System.Collections.Generic;
using Mizuvt.Common;
using UnityEngine;
using System;
using Download;
using Download.NodeSystem;
using UniRx;
using System.Collections.Immutable;
using System.Linq;


public class GameManager : PersistentSingleton<GameManager> {
    private NodeSystem nodeSystem;
    private NodePainter nodePainter;

    public ReactiveProperty<ImmutableOrderedSet<NodeGameObject>> SelectedNode = new(ImmutableOrderedSet<NodeGameObject>.Empty);

    protected override void Awake() {
        base.Awake();
        nodeSystem = new NodeSystem();
        nodePainter = this.gameObject.AddComponent<NodePainter>();
    }

    protected void Start() {
        nodePainter.Initialize(nodeSystem);

    }

    protected void OnEnable() {
        SelectedNode.DistinctUntilChanged().Pairwise().Subscribe(pair => {
            var prev = pair.Previous;
            var curr = pair.Current;

            // ROOM FOR OPTIMIZATION
            var SelectedList = curr.Except(prev);
            var UnselectedList = prev.Except(curr);
            SelectedList.ForEach(node => node.OnSelect());
            UnselectedList.ForEach(node => node.OnUnselect());
        }).AddTo(this);
    }



}
