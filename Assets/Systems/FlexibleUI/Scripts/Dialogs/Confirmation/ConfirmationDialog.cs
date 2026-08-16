using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlexibleUI {
    
    [RequireComponent(typeof(CanvasGroupAnimator))]
    public class ConfirmationDialog : MonoBehaviour {
        
        [Header("Text")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;

        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private TMP_Text confirmButtonText;
        [SerializeField] private TMP_Text cancelButtonText;

        private CanvasGroupAnimator animator;
        private ConfirmationRequest currentRequest;
        private bool isVisible;

        private void Awake() {
            animator = GetComponent<CanvasGroupAnimator>();

            confirmButton.onClick.AddListener(Confirm);
            cancelButton.onClick.AddListener(Cancel);
        }

        private void Update() {
            if (!isVisible)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }

        public void Show(ConfirmationRequest request) {
            
            if (request == null)
                return;

            currentRequest = request;
            isVisible = true;

            if (titleText != null) {
                titleText.text = request.title ?? string.Empty;

                titleText.gameObject.SetActive(
                    !string.IsNullOrWhiteSpace(request.title));
            }

            if (messageText != null)
                messageText.text = request.message ?? string.Empty;

            if (confirmButtonText != null)
                confirmButtonText.text =
                    string.IsNullOrWhiteSpace(request.confirmText)
                        ? "Confirm"
                        : request.confirmText;

            if (cancelButtonText != null)
                cancelButtonText.text =
                    string.IsNullOrWhiteSpace(request.cancelText)
                        ? "Cancel"
                        : request.cancelText;

            gameObject.SetActive(true);
            animator.Show();
        }

        public void Confirm() {
            
            if (!isVisible)
                return;

            var callback = currentRequest?.onConfirm;

            Close();
            callback?.Invoke();
        }

        public void Cancel() {
            
            if (!isVisible)
                return;

            var callback = currentRequest?.onCancel;

            Close();
            callback?.Invoke();
        }

        public void Close() {
            
            if (!isVisible)
                return;

            isVisible = false;
            currentRequest = null;

            animator.Hide();
        }

        private void OnDestroy() {
            if (confirmButton != null)
                confirmButton.onClick.RemoveListener(Confirm);

            if (cancelButton != null)
                cancelButton.onClick.RemoveListener(Cancel);
        }
    }
}