using System.Collections.Generic;
using UnityEngine;

namespace FlexibleUI {
    
    public class NotificationManager : MonoBehaviour {
        
        [Header("References")]
        [SerializeField] private NotificationView notificationPrefab;
        [SerializeField] private Transform notificationContainer;
        [SerializeField] private UIVisualTheme theme;

        [Header("Pooling")]
        [SerializeField] private int initialPoolSize = 5;

        private readonly Queue<NotificationView> available = new();
        private readonly List<NotificationView> active = new();

        private void Awake() {
            CreateInitialPool();
        }

        public void Show(NotificationRequest request) {
            if (request == null)
                return;

            if (!request.allowStacking)
                RemoveMatchingNotification(request);

            var notification = GetNotification();

            notification.Setup(request, theme);
            notification.Show(request);

            active.Add(notification);
        }

        public void ClearAll() {
            var notifications = active.ToArray();

            foreach (var notification in notifications)
                notification.Hide();
        }

        private void CreateInitialPool() {
            for (var i = 0; i < initialPoolSize; i++) {
                var notification = CreateNotification();
                ReturnToPool(notification);
            }
        }

        private NotificationView GetNotification() {
            var notification =
                available.Count > 0
                    ? available.Dequeue()
                    : CreateNotification();

            notification.gameObject.SetActive(true);
            return notification;
        }

        private NotificationView CreateNotification() {
            var notification = Instantiate(
                notificationPrefab,
                notificationContainer);

            notification.Expired += HandleExpired;
            notification.gameObject.SetActive(false);

            return notification;
        }

        private void HandleExpired(NotificationView notification) {
            active.Remove(notification);
            ReturnToPool(notification);
        }

        private void ReturnToPool(NotificationView notification) {
            if (notification is null)
                return;

            notification.gameObject.SetActive(false);
            available.Enqueue(notification);
        }

        private void RemoveMatchingNotification(NotificationRequest request) {
            var current = active.ToArray();

            foreach (var notification in current) {
                if (notification == null)
                    continue;
                notification.Hide();
            }
        }
    }
}