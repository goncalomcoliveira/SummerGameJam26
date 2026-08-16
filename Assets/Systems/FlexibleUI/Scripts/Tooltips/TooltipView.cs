using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlexibleUI {
    
    [RequireComponent(typeof(CanvasGroupAnimator))]
    public class TooltipView : MonoBehaviour {
        
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;

        private CanvasGroupAnimator animator;
        private RectTransform rectTransform;

        private void Awake() {
            animator = GetComponent<CanvasGroupAnimator>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Start() {
            Hide();
        }

        public void Setup(TooltipRequest request) {
            
            if (titleText != null) {
                titleText.text = request.title ?? string.Empty;

                titleText.gameObject.SetActive(
                    !string.IsNullOrWhiteSpace(request.title));
            }

            if (descriptionText != null) {
                descriptionText.text =
                    request.description ?? string.Empty;
            }

            if (iconImage != null) {
                iconImage.sprite = request.icon;

                iconImage.gameObject.SetActive(
                    request.icon != null);
            }
        }

        public void Show() {
            gameObject.SetActive(true);
            animator.Show();
        }

        public void Hide() {
            animator.Hide();
        }

        public void SetLocalPosition(Vector2 position) {
            rectTransform.anchoredPosition = position;
        }
    }
}