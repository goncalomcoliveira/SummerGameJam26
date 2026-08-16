using UnityEngine;
using UnityEngine.EventSystems;

namespace FlexibleUI {
    
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        
        [Header("Content")]
        [SerializeField] private string title;

        [TextArea(2, 6)]
        [SerializeField] private string description;

        [SerializeField] private Sprite icon;

        [Header("Position")]
        [SerializeField] private RectTransform target;
        [SerializeField] private Vector2 offset =
            new(0f, 12f);

        [SerializeField] private bool followTarget = true;

        public void OnPointerEnter(PointerEventData eventData) {
            var tooltipTarget =
                target != null
                    ? target
                    : transform as RectTransform;

            UIManager.Instance?.ShowTooltip(
                new TooltipRequest(
                    title,
                    description,
                    tooltipTarget,
                    icon,
                    offset,
                    followTarget));
        }

        public void OnPointerExit(PointerEventData eventData) {
            UIManager.Instance?.HideTooltip();
        }

        private void OnDisable() {
            UIManager.Instance?.HideTooltip();
        }
    }
}