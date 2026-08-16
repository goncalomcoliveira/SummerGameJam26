using System;
using System.Collections;
using UnityEngine;

namespace FlexibleUI {
    
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupAnimator : MonoBehaviour {
        
        [SerializeField] private UIAnimationProfile animationProfile;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Coroutine animationRoutine;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetProfile(UIAnimationProfile profile) {
            animationProfile = profile;
        }

        public void Show(bool instant = false) {
            Animate(true, instant, null);
        }

        public void Hide(bool instant = false, Action onComplete = null) {
            Animate(false, instant, onComplete);
        }

        private void Animate(bool show, bool instant, Action onComplete) {
            
            if (animationRoutine != null) {
                StopCoroutine(animationRoutine);
                animationRoutine = null;
            }
            
            if (!show && !gameObject.activeInHierarchy) {
                ApplyHiddenState();

                onComplete?.Invoke();
                return;
            }

            if (show && !gameObject.activeSelf) {
                gameObject.SetActive(true);
            }

            if (instant || animationProfile == null) {
                ApplyImmediate(show);

                onComplete?.Invoke();
                return;
            }

            animationRoutine = StartCoroutine(
                AnimateRoutine(show, onComplete));
        }

        private IEnumerator AnimateRoutine(bool show, Action onComplete) {
            
            if (show) {
                gameObject.SetActive(true);

                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }

            var duration = show
                ? animationProfile.showDuration
                : animationProfile.hideDuration;

            var curve = show
                ? animationProfile.showCurve
                : animationProfile.hideCurve;

            var startAlpha = canvasGroup.alpha;

            var targetAlpha = show
                ? 1f
                : 0f;

            var startScale =
                rectTransform.localScale;

            var targetScale = show
                ? Vector3.one
                : Vector3.one *
                  animationProfile.hiddenScale;

            var time = 0f;

            while (time < duration) {
                time += Time.unscaledDeltaTime;

                var normalizedTime =
                    duration <= 0f
                        ? 1f
                        : Mathf.Clamp01(
                            time / duration);

                var evaluated =
                    curve.Evaluate(normalizedTime);

                if (animationProfile.animateAlpha) {
                    canvasGroup.alpha =
                        Mathf.Lerp(
                            startAlpha,
                            targetAlpha,
                            evaluated);
                }

                if (animationProfile.animateScale) {
                    rectTransform.localScale =
                        Vector3.Lerp(
                            startScale,
                            targetScale,
                            evaluated);
                }

                yield return null;
            }

            ApplyImmediate(show);

            animationRoutine = null;

            onComplete?.Invoke();
        }

        private void ApplyImmediate(bool show) {
            
            if (show) {
                gameObject.SetActive(true);

                canvasGroup.alpha = 1f;

                rectTransform.localScale =
                    Vector3.one;

                canvasGroup.blocksRaycasts = true;
                canvasGroup.interactable = true;
            }
            else {
                ApplyHiddenState();

                gameObject.SetActive(false);
            }
        }

        private void ApplyHiddenState() {
            
            canvasGroup.alpha = 0f;

            if (animationProfile != null && animationProfile.animateScale) {
                rectTransform.localScale =
                    Vector3.one *
                    animationProfile.hiddenScale;
            }
            else {
                rectTransform.localScale =
                    Vector3.one;
            }

            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void OnDisable() {
            if (animationRoutine != null) {
                StopCoroutine(animationRoutine);
                animationRoutine = null;
            }
        }
    }
}