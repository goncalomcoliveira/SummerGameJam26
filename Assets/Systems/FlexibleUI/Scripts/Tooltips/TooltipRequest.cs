using System;
using UnityEngine;

namespace FlexibleUI {
    
    [Serializable]
    public class TooltipRequest {
        
        public string title;
        public string description;
        public Sprite icon;
        public RectTransform target;
        public Vector2 offset;
        public bool followTarget;

        public TooltipRequest(
            string title,
            string description,
            RectTransform target = null,
            Sprite icon = null,
            Vector2? offset = null,
            bool followTarget = true)
        {
            this.title = title;
            this.description = description;
            this.target = target;
            this.icon = icon;
            this.offset = offset ?? new Vector2(0f, 12f);
            this.followTarget = followTarget;
        }
    }
}