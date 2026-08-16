using UnityEngine;
using System;
using System.Collections.Generic;
using GoncaloMCOliveira.Singleton;

namespace FlexibleUI {
    
    public sealed class UIManager : PersistentSingleton<UIManager> {
        
        [Header("Core Systems")]
        [SerializeField] private PopupManager popupManager;
        [SerializeField] private NotificationManager notificationManager;
        [SerializeField] private TooltipManager tooltipManager;
        [SerializeField] private ConfirmationDialog confirmationDialog;

        private readonly Dictionary<Type, MonoBehaviour> systems = new();

        public PopupManager Popups => popupManager;
        public NotificationManager Notifications => notificationManager;
        public TooltipManager Tooltips => tooltipManager;
        public ConfirmationDialog Confirmation => confirmationDialog;

        public event Action<bool> ApplicationFocusChanged;
        public event Action ApplicationPaused;

        protected override void Awake() {
            base.Awake();
            
            RegisterSystem(popupManager);
            RegisterSystem(notificationManager);
            RegisterSystem(tooltipManager);
            RegisterSystem(confirmationDialog);
        }

        private void OnApplicationFocus(bool hasFocus) {
            ApplicationFocusChanged?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus)
                ApplicationPaused?.Invoke();
        }

        private void RegisterSystem<T>(T system) where T : MonoBehaviour {
            if (system == null)
                return;

            systems[typeof(T)] = system;
        }

        public T GetSystem<T>() where T : MonoBehaviour {
            return systems.TryGetValue(typeof(T), out MonoBehaviour system)
                ? system as T
                : null;
        }

        public void ShowPopup(PopupView popup) {
            popupManager?.Show(popup);
        }

        public void HidePopup(PopupView popup) {
            popupManager?.Hide(popup);
        }

        public void Notify(NotificationRequest request) {
            notificationManager?.Show(request);
        }

        public void ShowTooltip(TooltipRequest request) {
            tooltipManager?.Show(request);
        }

        public void HideTooltip() {
            tooltipManager?.Hide();
        }

        public void Confirm(
            string title,
            string message,
            Action onConfirm = null,
            Action onCancel = null,
            string confirmText = "Confirm",
            string cancelText = "Cancel")
        {
            confirmationDialog?.Show(
                new ConfirmationRequest(
                    title,
                    message,
                    confirmText,
                    cancelText,
                    onConfirm,
                    onCancel));
        }
    }
}