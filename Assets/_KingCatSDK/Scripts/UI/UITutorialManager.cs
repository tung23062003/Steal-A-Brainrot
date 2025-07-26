using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCat.Base.UI
{
    public class UITutorialManager : MonoSingleton<UITutorialManager>
    {
//#if TUTORIAL
        [SerializeField] private Image bg;
        [SerializeField] private GameObject hand;

        [SerializeField] private GameObject interacable;

        private GameObject tempButton;

        private UnityAction callback;

        public bool isShowing;

        protected override void Awake()
        {
            base.Awake();
            if (interacable != null)
            {
                var button = interacable.GetComponent<UIBaseButton>();
                if (button != null)
                {
                    button.onClick.AddListener(() => Hide());
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (interacable != null)
            {
                var button = interacable.GetComponent<UIBaseButton>();
                if (button != null)
                {
                    button.onClick.RemoveListener(() => Hide());
                }
            }
        }

        public Vector3 GetCenterPosition(RectTransform rectTransform)
        {
            // L?y kích th??c th?c t? c?a UI (k? c? scale)
            Vector2 size = rectTransform.rect.size;

            // Tính offset t? pivot hi?n t?i ??n trung tâm
            // (Ví d?: N?u pivot ? góc trái d??i (0,0), offset s? là (width/2, height/2))
            Vector2 centerOffset = new Vector2(
                size.x * (0.5f - rectTransform.pivot.x),
                size.y * (0.5f - rectTransform.pivot.y)
            );

            // Chuy?n t? local space sang world space
            Vector3 worldCenter = rectTransform.TransformPoint(centerOffset);

            return worldCenter;
        }

        public void ShowInPosition(UIBaseButton button, UnityAction cb, float bgFade = 0.8f, bool isShowBG = true, Action onComplete = null)
        {
            Transform oldParent = transform;
            int index = new();
            if (isShowBG)
            {
                oldParent = button.transform.parent;
                index = button.transform.GetSiblingIndex();
                button.transform.SetParent(bg.transform);
            }
            var seq = Show();

            if (isShowBG)
            {
                bg.color = new Vector4(0, 0, 0, 0f);
                seq.Join(bg.DOFade(bgFade, GameConfigs.TUTORIAL_TIME_ANIMATION));
            }else
                bg.gameObject.SetActive(false);

                

            RectTransform myButtonRect = button.GetComponent<RectTransform>();
            Vector3 centerPosition = GetCenterPosition(myButtonRect);
            hand.transform.position = centerPosition;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => Hide(() => {
                if (isShowBG)
                {
                    button.transform.SetParent(oldParent);
                    button.transform.SetSiblingIndex(index);
                }


                onComplete?.Invoke();
            }));

            callback = cb;
        }

        //[Obsolete("ATTENTION: Not right when parrent include auto layout component")]
        public void ShowByButton(UIBaseButton button, UnityAction cb, float bgFade=0.8f, Action onComplete = null)
        {
            var seq = Show();
            bg.color = new Vector4(0, 0, 0, 0f);
            seq.Join(bg.DOFade(bgFade, GameConfigs.TUTORIAL_TIME_ANIMATION));

            tempButton = Instantiate(button.gameObject, bg.transform);
            tempButton.transform.position = button.transform.position;
            hand.transform.position = tempButton.transform.position;

            tempButton?.GetComponent<UIBaseButton>()?.onClick.RemoveAllListeners();
            tempButton?.GetComponent<UIBaseButton>()?.onClick.AddListener(() => Hide());

            callback = cb;
            onComplete?.Invoke();
        }


        public void ShowByBoxCollider(GameObject go, UnityAction cb, float bgFade = 0.8f)
        {
            Show();

            bg.color = new Vector4(0, 0, 0, bgFade);
            interacable.SetActive(true);

            var boxCollider = go.GetComponent<BoxCollider2D>();
            Vector3 viewPos = Camera.main.WorldToViewportPoint(boxCollider.transform.position);
            Vector3 screenPos = new Vector2(viewPos.x * Screen.width, viewPos.y * Screen.height);
            
            Vector3 boxColliderSize = boxCollider.bounds.size;
            Vector3 screenSize = Camera.main.WorldToScreenPoint(boxCollider.transform.position + boxColliderSize)
                                 - Camera.main.WorldToScreenPoint(boxCollider.transform.position);
            RectTransform rectTransform = interacable.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(screenSize.x, screenSize.y);

            var rectTemp = interacable.GetComponent<RectTransform>();
            hand.transform.position = screenPos;

            interacable.transform.localScale = Vector3.one;
            interacable.GetComponent<RectTransform>().position = screenPos;

            callback = cb;
        }

        public void ShowByWorldPosition(Vector3 position, Vector2 size, UnityAction cb, float bgFade = 0.8f)
        {
            Show();

            bg.color = new Vector4(0, 0, 0, bgFade);
            interacable.SetActive(true);
            interacable.transform.localScale = Vector3.one;

            Vector3 viewPos = Camera.main.WorldToViewportPoint(position);
            Vector3 screenPos = new Vector2(viewPos.x * Screen.width, viewPos.y * Screen.height);

            Vector3 boxColliderSize = size;
            Vector3 screenSize = Camera.main.WorldToScreenPoint(position + boxColliderSize)
                                 - Camera.main.WorldToScreenPoint(position);
            RectTransform rectTransform = interacable.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(screenSize.x, screenSize.y);
            rectTransform.position = screenPos;

            hand.transform.position = screenPos;
            
            callback = cb;
        }

        public void ShowDrag(GameObject from, GameObject to)
        {
            Show();

            //Vector3 fromViewPos = Camera.main.WorldToViewportPoint(from.transform.position);
            //Vector3 toViewPos = Camera.main.WorldToViewportPoint(to.transform.position);

            //Vector3 fromScreenPos = new Vector3(fromViewPos.x * Screen.width, fromViewPos.y * Screen.height);
            //Vector3 toScreenPos = new Vector3(toViewPos.x * Screen.width, toViewPos.y * Screen.height);

            hand.transform.position = from.transform.position;
            hand.transform.localScale = Vector3.zero;

            DOTween.Kill(transform.GetInstanceID());

            var seq = DOTween.Sequence();
            seq.Append(hand.transform.DOScale(1, GameConfigs.TUTORIAL_TIME_ANIMATION)); 
            seq.Append(hand.transform.DOMove(to.transform.position, GameConfigs.TUTORIAL_TIME_ANIMATION * 2f)); 
            seq.SetLoops(-1, LoopType.Restart);
            seq.SetId(transform.GetInstanceID());

            interacable.gameObject.SetActive(false);
        }

        public void Show2GameObject(GameObject from, GameObject to)
        {
            Show();

            Vector3 fromViewPos = Camera.main.WorldToViewportPoint(from.transform.position);
            Vector3 toViewPos = Camera.main.WorldToViewportPoint(to.transform.position);

            Vector3 fromScreenPos = new Vector3(fromViewPos.x * Screen.width, fromViewPos.y * Screen.height);
            Vector3 toScreenPos = new Vector3(toViewPos.x * Screen.width, toViewPos.y * Screen.height);

            hand.transform.position = fromScreenPos;
            hand.transform.localScale = Vector3.zero;

            DOTween.Kill(transform.GetInstanceID());

            var seq = DOTween.Sequence();
            seq.Append(hand.transform.DOScale(0, GameConfigs.TUTORIAL_TIME_ANIMATION));
            seq.AppendCallback(() => hand.transform.position = fromScreenPos);
            seq.Append(hand.transform.DOScale(1, GameConfigs.TUTORIAL_TIME_ANIMATION));
            seq.AppendInterval(GameConfigs.TUTORIAL_TIME_ANIMATION * 2f);
            seq.Append(hand.transform.DOScale(0, GameConfigs.TUTORIAL_TIME_ANIMATION));
            seq.AppendCallback(() => hand.transform.position = toScreenPos);
            seq.Append(hand.transform.DOScale(1, GameConfigs.TUTORIAL_TIME_ANIMATION));
            seq.AppendInterval(GameConfigs.TUTORIAL_TIME_ANIMATION * 2f);
            seq.Append(hand.transform.DOScale(0, GameConfigs.TUTORIAL_TIME_ANIMATION));
            seq.SetLoops(-1, LoopType.Restart);
            seq.SetId(transform.GetInstanceID());

            interacable.SetActive(false);
        }

        private Sequence Show()
        {
            isShowing = true;
            bg.gameObject.SetActive(true);

            hand.SetActive(true);
            hand.transform.localScale = Vector3.zero;


            interacable.gameObject.SetActive(false);
            interacable.transform.localScale = Vector3.zero;
            
            DOTween.Kill(transform.GetInstanceID());

            var seq = DOTween.Sequence();
            seq.Append(hand.transform.DOScale(1, GameConfigs.TUTORIAL_TIME_ANIMATION).SetEase(Ease.OutBack));
            seq.SetId(transform.GetInstanceID());

            return seq;
        }

        public void Hide(Action onComplete = null)
        {
            isShowing = false;

            try
            {
                callback?.Invoke();
                callback = null;

                if (tempButton) Destroy(tempButton.gameObject);
                tempButton = null;

                DOTween.Kill(transform.GetInstanceID());

                var seq = DOTween.Sequence();
                seq.Append(hand.transform.DOScale(0, GameConfigs.TUTORIAL_TIME_ANIMATION).SetEase(Ease.Linear));
                if (bg.gameObject.activeSelf) seq.Join(bg.DOFade(0, GameConfigs.TUTORIAL_TIME_ANIMATION).SetEase(Ease.Linear));
                seq.AppendCallback(() =>
                {
                    bg.gameObject.SetActive(false);
                    interacable.SetActive(false);
                    hand.SetActive(false);
                });
                seq.SetId(transform.GetInstanceID());


                onComplete?.Invoke();
            }
            catch (System.Exception e) 
            { 
                Debug.LogError(e);
            }
        }

        public void TrackStep(string step)
        {
#if ANALYTICS
            AnalyticsManager.Instance.LogEvent($"tutorial_step_{step}");
#endif  
        }

        public void TrackBegin()
        {
#if ANALYTICS
            AnalyticsManager.Instance.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialBegin);
#endif
        }

        public void TrackComplete()
        {
#if ANALYTICS
            AnalyticsManager.Instance.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialComplete);
#endif
        }

//#endif
    }
}

