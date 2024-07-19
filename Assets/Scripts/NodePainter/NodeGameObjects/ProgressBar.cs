using Mizuvt.Common;
using UniRx;
using UnityEngine;


namespace Download {
    public class ProgressBar : MonoBehaviour {
        public Transform progressBar; // 프로그레스바 스프라이트 렌더러
        public GameObject SpriteParent;

        public SpriteRenderer BackgroundSquareRenderer;
        public SpriteRenderer ForegroundSquareRenderer;

        private readonly ReactiveProperty<ProgressBarTheme> theme = new(ProgressBarTheme.White);

        private void Start() {
            theme.Subscribe(theme => {
                switch (theme) {
                    case ProgressBarTheme.White:
                        BackgroundSquareRenderer.color = Utils.GetColorFromHex("#000000");
                        ForegroundSquareRenderer.color = Utils.GetColorFromHex("#FFFFFF");
                        break;
                    case ProgressBarTheme.Blue:
                        BackgroundSquareRenderer.color = Utils.GetColorFromHex("#08003F");
                        ForegroundSquareRenderer.color = Utils.GetColorFromHex("#BFC2FF");
                        break;
                }
            }
            );
        }

        // 진행도를 설정하는 함수 (0.0f ~ 1.0f 사이 값)
        public void SetProgress(float progress) {
            Vector3 newScale = progressBar.transform.localScale;
            newScale.x = progress;
            progressBar.transform.localScale = newScale;
        }

        public void SetVisible(bool isVisible) {
            SpriteParent.SetActive(isVisible);
        }

        public enum ProgressBarTheme { White, Blue }
        public void SetTheme(ProgressBarTheme theme) {
            this.theme.Value = theme;
        }

    }
}