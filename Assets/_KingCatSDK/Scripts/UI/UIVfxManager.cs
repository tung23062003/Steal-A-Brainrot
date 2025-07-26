using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KingCat.Base.UI
{
    public class UIVfxManager : MonoSingleton<UIVfxManager>
    {
        public void ShowCircleEffect(int amount, Sprite icon, Vector3 start, Vector3 end, UnityAction OnProgess = null, UnityAction onComplete = null)
        {
            for (int i = 0; i < amount; i++)
            {
                var index = i;
                GameObject circleIcon = new GameObject($"ExplosionEffect_{index}");
                circleIcon.transform.parent = transform;
                circleIcon.transform.position = start;
                circleIcon.transform.localScale = Vector3.zero;

                Image image = circleIcon.AddComponent<Image>();
                image.sprite = icon;
                //image.SetNativeSize();
                image.transform.localScale *= 2;

                RectTransform rectTransform = circleIcon.GetComponent<RectTransform>();
                rectTransform.position = start;

                float duration = 0.5f + (index * 0.1f);

                // Generate a random explosion direction 
                Vector3 randomDirection = Random.insideUnitCircle.normalized * 110f;

                // Sequence for explosion, smooth move, and scaling down
                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, duration / 2).SetEase(Ease.OutBack)) // Scale up
                        .Join(rectTransform.DOMove(start + randomDirection, duration / 2).SetEase(Ease.OutQuad)) // Explode outward
                        .Append(rectTransform.DOMove(end, duration).SetEase(Ease.InOutQuad)) // Move smoothly to end
                        .Join(rectTransform.DOScale(0.7f, 0.3f).SetEase(Ease.InQuad)) // Scale down to small size
                        .OnComplete(() =>
                        {
                            OnProgess?.Invoke();
                            Destroy(circleIcon);
                            if (index == amount - 1) onComplete?.Invoke();
                        });
            }
        }


        public void ShowLineEffect(int amount, Sprite icon, Vector3 start, Vector3 end, UnityAction callback)
        {
            for (int i = 0; i < amount; i++)
            {
                var index = i;
                GameObject lineIcon = new GameObject($"LineEffect_{index}");
                lineIcon.transform.parent = transform;
                lineIcon.transform.position = start;

                Image image = lineIcon.AddComponent<Image>();
                image.sprite = icon;
                image.SetNativeSize();

                RectTransform rectTransform = lineIcon.GetComponent<RectTransform>();
                rectTransform.position = start;

                float duration = 0.9f + (index * 0.1f);

                lineIcon.transform.DOScale(0.2f, duration * 0.9f).SetEase(Ease.Linear);
                rectTransform.DOMove(end, duration).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    Destroy(lineIcon);
                    if (index == amount - 1) callback?.Invoke();
                });
            }
        }

        public void ShowJustUpBooster(int amount, Sprite icon, Vector3 start, UnityAction callback, float durationTime=0.5f)
        {
            for (int i = 0; i < amount; i++)
            {
                var index = i;
                GameObject boosterIcon = new GameObject($"JustUpBooster_{index}");
                boosterIcon.transform.parent = transform;
                boosterIcon.transform.position = start;
                boosterIcon.transform.localScale = Vector3.zero;

                Image image = boosterIcon.AddComponent<Image>();
                image.sprite = icon;
                image.SetNativeSize();

                RectTransform rectTransform = boosterIcon.GetComponent<RectTransform>();
                rectTransform.position = start;

                float duration = durationTime + (index * 0.1f);

                rectTransform.DOScale(1, duration / 2).SetEase(Ease.OutBounce);
                rectTransform.DOMoveY(start.y + 2, duration).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    rectTransform.DOScale(0, duration / 2).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        Destroy(boosterIcon);
                        if (index == amount - 1) callback?.Invoke();
                    });
                });
            }
        }

        [ContextMenu("Test fx")]
        private void Test()
        {
            ShowCircleEffect(5, LoaderUtility.Instance.GetAsset<Sprite>("Sprites/coin"), new Vector3(0, 0, 0), new Vector3(100, 100, 0), null, () => Debug.Log("Circle Effect Complete"));
            ShowLineEffect(3, LoaderUtility.Instance.GetAsset<Sprite>("Sprites/star"), new Vector3(50, 50, 0), new Vector3(200, 200, 0), () => Debug.Log("Line Effect Complete"));
            ShowJustUpBooster(4, LoaderUtility.Instance.GetAsset<Sprite>("Sprites/hammer"), new Vector3(0, 0, 0), () => Debug.Log("Just Up Booster Complete"));
        }

    }
}

