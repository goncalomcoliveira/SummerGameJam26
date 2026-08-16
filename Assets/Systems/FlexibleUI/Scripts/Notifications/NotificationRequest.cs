using System;
using UnityEngine;

namespace FlexibleUI {
    
    [Serializable]
    public class NotificationRequest {
        
        public string title;
        public string message;
        public NotificationType type;
        public Sprite icon;
        public float duration;
        public bool allowStacking;

        public NotificationRequest(
            string title,
            string message,
            NotificationType type = NotificationType.Default,
            float duration = 3f,
            Sprite icon = null,
            bool allowStacking = true)
        {
            this.title = title;
            this.message = message;
            this.type = type;
            this.duration = duration;
            this.icon = icon;
            this.allowStacking = allowStacking;
        }
    }
}