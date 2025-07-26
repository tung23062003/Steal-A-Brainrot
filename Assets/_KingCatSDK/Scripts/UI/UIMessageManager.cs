using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCat.Base.UI
{
    [System.Serializable]
    public class UINoti
    {
        public CanvasGroup canvas;
        public TextMeshProUGUI notiText;
    }

    [System.Serializable]
    public class UIMessage
    {
        public CanvasGroup canvas;
        public Transform transform;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI messageText;
        public Image iconImage;
        public UIBaseButton yesButton;
        public UIBaseButton okeButton;
        public UIBaseButton noButton;
    }

    public class UIMessageManager : MonoSingleton<UIMessageManager>
    {
        public UINoti noti;
        public UIMessage message;
        private const float ANIM_DURATION = 0.5f;
        private const float NOTI_DURATION = 2.0f;

        private void Start()
        {
            HideMessage();
        }

        private void ClearMessage()
        {
            message.canvas.alpha = 0;
            message.canvas.gameObject.SetActive(false);

            message.yesButton.onClick.RemoveAllListeners();
            message.noButton.onClick.RemoveAllListeners();
        }

        private void ClearNoti()
        {
            noti.canvas.alpha = 0;
            noti.canvas.gameObject.SetActive(false);
        }

        public void ShowNoti(string text, string location)
        {
            Debug.Log($"Show noti {location}: {text}");

            DOTween.Kill(transform.GetInstanceID());

            noti.canvas.gameObject.SetActive(true);
            noti.notiText.text = text;
            noti.canvas.alpha = 0;
            

            // Get the RectTransform component
            RectTransform rectTransform = noti.canvas.GetComponent<RectTransform>();
            Vector2 nextPosition = Vector2.zero;
            Vector2 startPosition = Vector2.zero;

            // Determine starting position based on location
            switch (location.ToLower())
            {
                case "top":
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    rectTransform.pivot = new Vector2(0.5f, 1f);
                    startPosition = new Vector2(0, 200); 
                    nextPosition = new Vector2(0, -45); 
                    break;
                case "bottom":
                    rectTransform.anchorMin = new Vector2(0.5f, 0f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0f);
                    rectTransform.pivot = new Vector2(0.5f, 0f);
                    startPosition = new Vector2(0, - 200);
                    nextPosition = new Vector2(0, 45);
                    break;
                case "center":
                    rectTransform.localScale = Vector3.zero; // Start with scale 0
                     // Scale up to full size
                    break;
                default:
                    break;
            }

            rectTransform.anchoredPosition = startPosition;

            Sequence seq = DOTween.Sequence();
            seq.Append(noti.canvas.DOFade(1, ANIM_DURATION).SetEase(Ease.Linear)); // Fade in
            seq.Join(rectTransform.DOAnchorPos(nextPosition, ANIM_DURATION).SetEase(Ease.Linear));
            seq.AppendInterval(NOTI_DURATION);
            seq.Append(noti.canvas.DOFade(0, ANIM_DURATION).SetEase(Ease.Linear));
            seq.Join(rectTransform.DOAnchorPos(startPosition, ANIM_DURATION).SetEase(Ease.Linear));
            seq.AppendCallback(ClearNoti);
            seq.SetId(transform.GetInstanceID());

            SoundManager.Instance.PlaySound("sound_noti");
            SoundManager.Instance.PlayVibrateNope();
        }

        public void ShowMessage(string text, UnityAction yesCallback, UnityAction noCallback, string title="MESSAGE", Sprite icon=null)
        {
            ClearMessage(); // clear message

            message.titleText.text = title;
            message.messageText.text = text;
            if (icon != null)
            {
                message.iconImage.gameObject.SetActive(true);
                message.iconImage.sprite = icon;
            }
            else { message.iconImage.gameObject.SetActive(false);}
            

            // Reset the canvas group and scale for the animation
            message.canvas.alpha = 0;
            message.transform.localScale = Vector3.zero;
            message.canvas.gameObject.SetActive(true);

            // Add the show animation
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(message.canvas.DOFade(1, ANIM_DURATION).SetEase(Ease.Linear)); // Fade in over 0.5 seconds
            showSequence.Join(message.transform.DOScale(Vector3.one, ANIM_DURATION).SetEase(Ease.OutBack)); // Scale up from 0 to full size
            message.yesButton.onClick.RemoveAllListeners();
            message.noButton.onClick.RemoveAllListeners();

            if (noCallback != null)
            {
                message.noButton.gameObject.SetActive(true);
                message.noButton.onClick.AddListener(noCallback);
                message.okeButton.gameObject.SetActive(false);
                message.yesButton.gameObject.SetActive(true);
            }
            else
            {
                message.noButton.gameObject.SetActive(false);
                message.okeButton.gameObject.SetActive(true);
                message.yesButton.gameObject.SetActive(false);
            }

            if (yesCallback != null)
            {
                message.yesButton.onClick.AddListener(yesCallback);
                message.okeButton.onClick.AddListener(yesCallback);
            }
            else
            {
                message.okeButton.gameObject.SetActive(false);
                message.yesButton.gameObject.SetActive(false);
            }

            message.yesButton.onClick.AddListener(HideMessage);
            message.okeButton.onClick.AddListener(HideMessage);
            message.noButton.onClick.AddListener(HideMessage);
        }

        public void HideMessage()
        {
            // Fade out the message box and then deactivate it
            Sequence seq = DOTween.Sequence();
            seq.Append(message.canvas.DOFade(0, ANIM_DURATION).SetEase(Ease.Linear)); // Fade out over 0.5 seconds
            seq.Join(message.transform.DOScale(0, ANIM_DURATION).SetEase(Ease.InBack));
            seq.AppendCallback(ClearMessage);
            seq.SetId(transform.GetInstanceID());
        }

    }
}
