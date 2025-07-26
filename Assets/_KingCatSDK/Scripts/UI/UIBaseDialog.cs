using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCat.Base.UI
{
    public abstract class UIBaseDialog : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Transform panel;
        public UIBaseButton closeBtn;
        public const float ANIM_DURATION = 0.5f;

        public UnityEvent onClosed = new UnityEvent();

        /*        protected virtual void Awake()
                {}*/
        protected virtual void Awake()
        {
            UIDialogManager.Instance.RegisterDialog(this);
            closeBtn.onClick.AddListener(Hide);
        }


        protected virtual void OnDestroy()
        {
            closeBtn.onClick.RemoveAllListeners();
            DOTween.KillAll();
        }

        public virtual void Show()
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            panel.gameObject.SetActive(true);
            panel.localScale = Vector3.zero;
            Sequence seq = DOTween.Sequence();
            seq.Join(panel.DOScale(Vector3.one, ANIM_DURATION).SetEase(Ease.OutBack));
            seq.Join(canvasGroup.DOFade(1, ANIM_DURATION).SetEase(Ease.Linear));
            seq.OnComplete(ShowCompleted);

            GameEvent.OnFreezePlayer?.Invoke();

            SFXManager.Instance.PlaySFX("PopupOpen", Vector2.zero, 1, 0, false, false);
            //  SoundManager.Instance.PlaySound("PopupOpen");
        }

        protected virtual void ShowCompleted()
        {}

        public virtual void Hide()
        {
            if (canvasGroup == null) return;
            Debug.Log("Hide dialog");
            Sequence seq = DOTween.Sequence();
            seq.Join(panel.DOScale(Vector3.zero, ANIM_DURATION).SetEase(Ease.InBack));
            seq.Join(canvasGroup.DOFade(0, ANIM_DURATION).SetEase(Ease.Linear));
            seq.OnComplete(() => {
                gameObject.SetActive(false);
                HideCompleted();

                GameEvent.OnUnFreezePlayer?.Invoke();
            });

            //   SoundManager.Instance.PlaySound("PopupClose");
        }

        protected virtual void HideCompleted()
        {
            onClosed?.Invoke();
        }
    }
}

