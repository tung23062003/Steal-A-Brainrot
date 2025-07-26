using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject container;
    private CanvasGroup canvas;

    protected override void Awake()
    {
        base.Awake();
        canvas = container.GetComponent<CanvasGroup>();
    }

    public void FadeOut(float time = 0.5f, Action onComplete = null)
    {
        canvas.alpha = 0;
        canvas.DOFade(1, time).OnComplete(() => { onComplete?.Invoke(); });
    }

    public void FadeIn(float time = 0.5f, Action onComplete = null)
    {
        canvas.alpha = 1;
        canvas.DOFade(0, time).OnComplete(() => { onComplete?.Invoke(); });
    }
    public UniTask FadeOutAsync(float time = 0.5f)
    {
        canvas.alpha = 0; // B?t ??u t? trong su?t
        return canvas.DOFade(1, time)
                    .OnComplete(() => canvas.alpha = 1)
                    .AsyncWaitForCompletion()
                    .AsUniTask();
    }

    public UniTask FadeInAsync(float time = 0.5f)
    {
        canvas.alpha = 1;
        return canvas.DOFade(0, time).AsyncWaitForCompletion().AsUniTask();
    }

    public void FadeOutIn(float time = 0.5f, Action onFadeOutComplete = null, Action onFadeInComplete = null)
    {
        canvas.alpha = 0;
        canvas.DOFade(1, time).OnComplete(() =>
        {
            onFadeOutComplete?.Invoke();
            canvas.DOFade(0, time).OnComplete(() =>
            {
                onFadeInComplete?.Invoke();
            });
        });
    }

    public async UniTask FadeOutInAsync(
    float time = 0.5f,
    Func<UniTask> onFadeOutComplete = null,
    Action onFadeInComplete = null)
    {
        // Fade Out
        await canvas.DOFade(1, time).AsyncWaitForCompletion().AsUniTask();

        // Ch? công vi?c load hoàn thành (n?u có)
        if (onFadeOutComplete != null)
            await onFadeOutComplete.Invoke();

        // Fade In
        await canvas.DOFade(0, time).AsyncWaitForCompletion().AsUniTask();
        onFadeInComplete?.Invoke();
    }
}
