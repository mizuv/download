using System.Collections;
using System.Collections.Generic;
using Download.NodeSystem;
using UniRx;

namespace Download {
    public interface IDragEventListener {
        public void OnDrop(DragContext context);
        public void OnHoverAtDrag(DragContext context);

        public bool IsDestoryed { get; }
    }

    public class DragContext {
        public readonly IEnumerable<Node> SelectedNodes;

        public DragContext(IEnumerable<Node> selectedNodes) {
            this.SelectedNodes = selectedNodes;
        }
    }
}