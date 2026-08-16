using UnityEngine;

namespace FlexibleUI {
    
    [CreateAssetMenu(
        fileName = "UIAnimationProfile",
        menuName = "Flexible UI/Animation Profile")]
    public class UIAnimationProfile : ScriptableObject {
        
        [Header("Show")]
        [Min(0f)]
        public float showDuration = 0.2f;

        public AnimationCurve showCurve = AnimationCurve.EaseInOut(
            0f,
            0f,
            1f,
            1f);

        [Header("Hide")]
        [Min(0f)]
        public float hideDuration = 0.15f;

        public AnimationCurve hideCurve = AnimationCurve.EaseInOut(
            0f,
            0f,
            1f,
            1f);

        [Header("Visual")]
        public float hiddenScale = 0.9f;

        public bool animateAlpha = true;
        public bool animateScale = true;
    }
}