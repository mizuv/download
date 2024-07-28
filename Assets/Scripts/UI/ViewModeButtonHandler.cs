using UnityEngine;

namespace Download {
    public class ButtonClickHandler : MonoBehaviour {
        public void OnListButtonClick() {
            GameManager.Instance.View.Value = GameManager.ViewMode.List;
        }

        public void OnTreeButtonClick() {
            GameManager.Instance.View.Value = GameManager.ViewMode.Tree;
        }
    }
}