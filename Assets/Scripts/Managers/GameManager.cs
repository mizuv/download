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
        nodePainter = new NodePainter();
    }

    protected void Start() {
        nodePainter.Initialize(nodeSystem);

    }

    protected void OnEnable() {
        SelectedNode.DistinctUntilChanged().Pairwise().Subscribe(pair => {
            pair.Previous?.ShowSelectedSprite(false);
            pair.Current?.ShowSelectedSprite(true);
        }).AddTo(this);
    }



}
