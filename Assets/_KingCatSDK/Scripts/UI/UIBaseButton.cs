using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KingCat.Base.UI
{
    [RequireComponent(typeof(Button))]
    public class UIBaseButton : MonoBehaviour, IPointerEnterHandler
    {
        [HideInInspector]
        public UnityEvent onClick = new UnityEvent();
        [HideInInspector]
        public UnityEvent onPointerEnter = new UnityEvent();

        private Button button;
        private float animationScale = 1.1f;
        private float animationDuration = 0.1f;

        private bool isPressed = false;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        protected virtual void OnButtonClick()
        {
            if (!isPressed)
            {
                isPressed = true;
                AnimateButton();

                SFXManager.Instance.PlaySFX("Click", Vector2.zero, 1, 0);
                //SoundManager.Instance.PlaySound("Button", 0.3f);1
                //SoundManager.Instance.PlayVibratePop();
            }
        }

        private void AnimateButton()
        {
            transform.DOScale(Vector3.one * animationScale, animationDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // Trigger the onClick action
                InvokeCallback();

                // Scale back to original size
                transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.InOutSine);

                isPressed = false;
            });
        }

        protected virtual void InvokeCallback()
        {
            onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();
        }

        
    }
}
