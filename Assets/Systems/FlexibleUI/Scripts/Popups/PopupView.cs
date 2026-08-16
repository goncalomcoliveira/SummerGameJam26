using System;
using UnityEngine;

namespace FlexibleUI {
    
    [RequireComponent(typeof(CanvasGroupAnimator))]
    public abstract class PopupView : MonoBehaviour {
        
        [SerializeField] private bool closeOnEscape = true;
        [SerializeField] private bool closeOnBackgroundClick;

        protected CanvasGroupAnimator Animator { get; private set; }

        public bool IsVisible { get; private set; }
        public bool CloseOnEscape => closeOnEscape;
        public bool CloseOnBackgroundClick => closeOnBackgroundClick;

        public event Action<PopupView> Opened;
        public event Action<PopupView> Closed;

        protected virtual void Awake() {
            Animator = GetComponent<CanvasGroupAnimator>();
        }

        protected virtual void Update() {
            
            if (!IsVisible || !closeOnEscape)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                Hide();
        }

        public virtual void Show() {
            
            if (IsVisible)
                return;

            IsVisible = true;

            gameObject.SetActive(true);
            Animator.Show();

            OnShown();
            Opened?.Invoke(this);
        }

        public virtual void Hide() {
            
            if (!IsVisible)
                return;

            IsVisible = false;

            Animator.Hide(false, () => {
                OnHidden();
                Closed?.Invoke(this);
            });
        }

        public virtual void ForceHide() {
            IsVisible = false;
            Animator.Hide(true);

            OnHidden();
            Closed?.Invoke(this);
        }

        public void OnBackgroundClicked() {
            if (closeOnBackgroundClick)
                Hide();
        }

        protected virtual void OnShown() { }

        protected virtual void OnHidden() { }
    }
}