using System;

namespace FlexibleUI {
    public class ConfirmationRequest {
        
        public string title;
        public string message;
        public string confirmText;
        public string cancelText;
        public Action onConfirm;
        public Action onCancel;

        public ConfirmationRequest(
            string title,
            string message,
            string confirmText = "Confirm",
            string cancelText = "Cancel",
            Action onConfirm = null,
            Action onCancel = null)
        {
            this.title = title;
            this.message = message;
            this.confirmText = confirmText;
            this.cancelText = cancelText;
            this.onConfirm = onConfirm;
            this.onCancel = onCancel;
        }
    }
}