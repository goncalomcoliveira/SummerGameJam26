using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlexibleUI {
    
    [RequireComponent(typeof(CanvasGroupAnimator))]
    public class NotificationView : MonoBehaviour {
        
        [Header("References")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image accentImage;

        private CanvasGroupAnimator animator;
        private Coroutine lifetimeRoutine;

        public event Action<NotificationView> Expired;

        private void Awake() {
            animator = GetComponent<CanvasGroupAnimator>();
        }

        public void Setup(NotificationRequest request, UIVisualTheme theme) {
            
            if (titleText != null) {
                titleText.text = request.title ?? string.Empty;
                titleText.gameObject.SetActive(
                    !string.IsNullOrWhiteSpace(request.title));
            }

            if (messageText != null)
                messageText.text = request.message ?? string.Empty;

            var statusColor = GetStatusColor(
                request.type,
                theme);

            if (accentImage != null)
                accentImage.color = statusColor;

            if (iconImage != null) {
                
                var selectedIcon = request.icon != null
                    ? request.icon
                    : theme != null
                        ? theme.defaultIcon
                        : null;

                iconImage.sprite = selectedIcon;
                iconImage.gameObject.SetActive(selectedIcon != null);
            }
        }

        public void Show(NotificationRequest request) {
            
            if (lifetimeRoutine != null)
                StopCoroutine(lifetimeRoutine);

            gameObject.SetActive(true);
            animator.Show();

            if (request.duration > 0f) {
                lifetimeRoutine =
                    StartCoroutine(
                        LifetimeRoutine(request.duration));
            }
        }

        public void Hide() {
            
            if (lifetimeRoutine != null)
                StopCoroutine(lifetimeRoutine);

            animator.Hide(false, () => {
                Expired?.Invoke(this);
            });
        }

        public void ForceHide() {
            if (lifetimeRoutine != null)
                StopCoroutine(lifetimeRoutine);

            animator.Hide(true);
            Expired?.Invoke(this);
        }

        private IEnumerator LifetimeRoutine(float duration) {
            yield return new WaitForSecondsRealtime(duration);
            Hide();
        }

        private static Color GetStatusColor(NotificationType type, UIVisualTheme theme) {
            
            if (theme == null)
                return Color.white;

            return type switch {
                NotificationType.Success => theme.successColor,
                NotificationType.Info => theme.infoColor,
                NotificationType.Warning => theme.warningColor,
                NotificationType.Error => theme.errorColor,
                _ => theme.primaryColor
            };
        }
    }
}