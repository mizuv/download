using System.Collections.Generic;
using Mizuvt.Common;
using UnityEngine;
using System;
using Download;
using Download.NodeSystem;


public class GameManager : PersistentSingleton<GameManager> {
    private NodeSystem nodeSystem;
    private NodePainter nodePainter;
    protected override void Awake() {
        base.Awake();
        nodeSystem = new NodeSystem();
        nodePainter = new NodePainter();
    }

    protected void Start() {
        nodePainter.Initialize(nodeSystem);

    }

}
