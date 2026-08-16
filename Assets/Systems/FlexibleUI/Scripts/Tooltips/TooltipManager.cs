using UnityEngine;

namespace FlexibleUI {
    
    public class TooltipManager : MonoBehaviour {
        [Header("References")]
        [SerializeField] private TooltipView tooltipView;
        [SerializeField] private Canvas rootCanvas;

        private TooltipRequest currentRequest;

        private RectTransform CanvasRectTransform =>
            rootCanvas != null
                ? rootCanvas.transform as RectTransform
                : null;

        private Camera UICamera {
            get {
                if (rootCanvas == null)
                    return null;

                return rootCanvas.renderMode ==
                       RenderMode.ScreenSpaceOverlay
                    ? null
                    : rootCanvas.worldCamera;
            }
        }

        private void Update() {
            if (currentRequest == null)
                return;

            if (!currentRequest.followTarget)
                return;

            UpdatePosition();
        }

        public void Show(TooltipRequest request) {
            if (request == null || tooltipView == null)
                return;

            currentRequest = request;

            tooltipView.Setup(request);
            tooltipView.Show();

            UpdatePosition();
        }

        public void Hide() {
            currentRequest = null;
            tooltipView?.Hide();
        }

        private void UpdatePosition()
        {
            if (currentRequest == null ||
                tooltipView == null ||
                CanvasRectTransform == null)
            {
                return;
            }

            Vector2 screenPosition;

            if (currentRequest.target != null)
            {
                Vector3[] corners = new Vector3[4];

                currentRequest.target.GetWorldCorners(corners);

                // Top center of the target.
                Vector3 worldPosition =
                    (corners[1] + corners[2]) * 0.5f;

                screenPosition =
                    RectTransformUtility.WorldToScreenPoint(
                        UICamera,
                        worldPosition);
            }
            else
            {
                screenPosition = Input.mousePosition;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                CanvasRectTransform,
                screenPosition,
                UICamera,
                out Vector2 localPosition);

            localPosition += currentRequest.offset;

            tooltipView.SetLocalPosition(localPosition);
        }
    }
}