using Mizuvt.Common;
using Download;
using Download.NodeSystem;
using UniRx;
using System.Linq;


public class GameManager : PersistentSingleton<GameManager> {
    public NodeSystem nodeSystem;
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
