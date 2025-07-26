using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KingCat.Base.UI
{
    public class UILoadingManager : MonoSingleton<UILoadingManager>
    {
        [SerializeField] private CanvasGroup fadeCanvasGroup; // Assign your CanvasGroup in the inspector
        public float fadeDuration = 0.5f; // Duration of the fade effect

        public void LoadScene(string sceneName)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 0f;

            // Start the fade and load process
            FadeAndLoadScene(sceneName).Forget(); // Using UniTask or a wrapper for async operations
        }

        private async UniTaskVoid FadeAndLoadScene(string sceneName)
        {
            // Fade out
            await fadeCanvasGroup.DOFade(1, fadeDuration).AsyncWaitForCompletion();

            // Load the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            // Wait until the scene is loaded
            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    // Activate the scene when it's ready
                    asyncLoad.allowSceneActivation = true;
                }
                await UniTask.Yield();
            }

            // Fade back in after loading the scene
            await fadeCanvasGroup.DOFade(0, fadeDuration).AsyncWaitForCompletion();

            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }
}

