using Mizuvt.Common;
using Download;
using Download.NodeSystem;
using UniRx;
using System.Linq;
using UnityEngine;


public class GameManager : PersistentSingleton<GameManager> {
    public NodeSystem nodeSystem;
    private NodeTreePainter nodeTreePainter;
    private NodeListPainter nodeListPainter;

    public enum ViewMode { Tree, List }
    public ReactiveProperty<ViewMode> View = new(ViewMode.Tree);

    public ReactiveProperty<ImmutableOrderedSet<NodeGameObject>> SelectedNode = new(ImmutableOrderedSet<NodeGameObject>.Empty);

    protected override void Awake() {
        base.Awake();
        nodeSystem = new NodeSystem();
        nodeTreePainter = new GameObject("NodeTreePainter").AddComponent<NodeTreePainter>();
        nodeListPainter = new GameObject("NodeListPainter").AddComponent<NodeListPainter>();
    }

    protected void Start() {
        nodeTreePainter.Initialize(nodeSystem);

        View.Subscribe(view => {
            nodeTreePainter.gameObject.SetActive(view == ViewMode.Tree);
            nodeListPainter.gameObject.SetActive(view == ViewMode.List);
        }).AddTo(this);

        // 객체에 select 이벤트 전달하기
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
