using System.Collections.Generic;
using UnityEngine;

namespace FlexibleUI {
    
    public class PopupManager : MonoBehaviour {
        [SerializeField] private Transform popupContainer;

        private readonly Stack<PopupView> popupStack = new();

        public PopupView CurrentPopup =>
            popupStack.Count > 0 ? popupStack.Peek() : null;

        public void Show(PopupView popup) {
            if (popup == null)
                return;

            if (popupContainer != null && popup.transform.parent != popupContainer) {
                popup.transform.SetParent(
                    popupContainer,
                    false);
            }

            if (popupStack.Contains(popup))
                return;

            popupStack.Push(popup);

            popup.Closed -= HandlePopupClosed;
            popup.Closed += HandlePopupClosed;

            popup.Show();
        }

        public void Hide(PopupView popup) {
            if (popup == null)
                return;

            popup.Hide();
        }

        public void HideCurrent() {
            if (popupStack.Count == 0)
                return;

            popupStack.Peek().Hide();
        }

        public void HideAll(bool instant = false) {
            
            var activePopups = popupStack.ToArray();

            popupStack.Clear();

            foreach (var popup in activePopups) {
                
                if (popup == null)
                    continue;

                popup.Closed -= HandlePopupClosed;

                if (instant)
                    popup.ForceHide();
                else
                    popup.Hide();
            }
        }

        private void HandlePopupClosed(PopupView popup) {
            
            RemovePopup(popup);

            if (popup is not null)
                popup.Closed -= HandlePopupClosed;
        }

        private void RemovePopup(PopupView popup) {
            
            if (!popupStack.Contains(popup))
                return;

            List<PopupView> temporary = new();

            while (popupStack.Count > 0) {
                
                var current = popupStack.Pop();

                if (current == popup)
                    break;

                temporary.Add(current);
            }

            for (var i = temporary.Count - 1; i >= 0; i--)
                popupStack.Push(temporary[i]);
        }
    }
}