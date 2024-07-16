using UnityEngine;


namespace Download {
    public class ProgressBar : MonoBehaviour {
        public Transform progressBar; // 프로그레스바 스프라이트 렌더러
        public GameObject SpriteParent;

        // 진행도를 설정하는 함수 (0.0f ~ 1.0f 사이 값)
        public void SetProgress(float progress) {
            Vector3 newScale = progressBar.transform.localScale;
            newScale.x = progress;
            progressBar.transform.localScale = newScale;
        }

        public void SetVisible(bool isVisible) {
            SpriteParent.SetActive(isVisible);
        }
    }
}