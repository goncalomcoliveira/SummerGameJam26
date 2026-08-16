using System;
using UnityEngine;

namespace FlexibleUI {
    
    public static class UIServiceExtensions {
        
        public static void ShowSuccess(
            this NotificationManager manager,
            string message,
            string title = "Success",
            float duration = 3f)
        {
            manager?.Show(
                new NotificationRequest(
                    title,
                    message,
                    NotificationType.Success,
                    duration));
        }

        public static void ShowInfo(
            this NotificationManager manager,
            string message,
            string title = "Information",
            float duration = 3f)
        {
            manager?.Show(
                new NotificationRequest(
                    title,
                    message,
                    NotificationType.Info,
                    duration));
        }

        public static void ShowWarning(
            this NotificationManager manager,
            string message,
            string title = "Warning",
            float duration = 4f)
        {
            manager?.Show(
                new NotificationRequest(
                    title,
                    message,
                    NotificationType.Warning,
                    duration));
        }

        public static void ShowError(
            this NotificationManager manager,
            string message,
            string title = "Error",
            float duration = 5f)
        {
            manager?.Show(
                new NotificationRequest(
                    title,
                    message,
                    NotificationType.Error,
                    duration));
        }

        public static void ShowConfirmation(
            this ConfirmationDialog dialog,
            string title,
            string message,
            Action onConfirm,
            Action onCancel = null)
        {
            dialog?.Show(
                new ConfirmationRequest(
                    title,
                    message,
                    "Confirm",
                    "Cancel",
                    onConfirm,
                    onCancel));
        }

        public static void ShowTooltip(
            this TooltipManager manager,
            string title,
            string description,
            RectTransform target = null,
            Sprite icon = null)
        {
            manager?.Show(
                new TooltipRequest(
                    title,
                    description,
                    target,
                    icon));
        }
    }
}