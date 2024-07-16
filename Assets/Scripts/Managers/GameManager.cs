using System.Collections.Generic;
using Mizuvt.Common;
using UnityEngine;
using System;
using Download;
using Download.NodeSystem;
using UniRx;


public class GameManager : PersistentSingleton<GameManager> {
    private NodeSystem nodeSystem;
    private NodePainter nodePainter;

    public ReactiveProperty<NodeGameObject?> SelectedNode = new(null);

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
            if (prev != null) prev.OnUnselect();
            if (curr != null) curr.OnSelect();
        }).AddTo(this);
    }



}
