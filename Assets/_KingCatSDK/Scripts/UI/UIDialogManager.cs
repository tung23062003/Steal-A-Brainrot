using KingCat.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;

namespace KingCat.Base.UI
{
    /*  public class UIDialogManager : MonoSingleton<UIDialogManager>
      {
          //[SerializeField] private CanvasGroup background;
          [SerializeField] private List<UIBaseDialog> dialogs = new List<UIBaseDialog>();
          private Dictionary<string, UIBaseDialog> activeDialogs = new Dictionary<string, UIBaseDialog>();

  //#if UNITY_EDITOR
  //        private void OnValidate()
  //        {
  //            //Find all UIBaseDialog prefab
  //            try
  //            {
  //                dialogs.Clear();
  //                string[] guids = AssetDatabase.FindAssets("t:Prefab");
  //                foreach (string guid in guids)
  //                {
  //                    string path = AssetDatabase.GUIDToAssetPath(guid);
  //                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
  //                    if (prefab != null)
  //                    {
  //                        UIBaseDialog dialog = prefab.GetComponent<UIBaseDialog>();
  //                        if (dialog != null)
  //                        {
  //                            dialogs.Add(dialog);
  //                        }
  //                    }
  //                }
  //            }
  //            catch (System.Exception e)
  //            {
  //            }
  //        }
  //#endif

          protected override void Awake()
          {
              base.Awake();
              //background.gameObject.SetActive(false);
              //background.alpha = 0f;
          }
          public T ShowDialog<T>() where T : UIBaseDialog
          {
              //background.gameObject.SetActive(true);
              //background.alpha = 0f;
              //background.DOFade(0.8f, UIBaseDialog.ANIM_DURATION);

              var dialogType= typeof(T).ToString();
              if (!activeDialogs.ContainsKey(dialogType))
              {
                  // find the dialog by its type
                  var prefab = dialogs.Find(x => x is T) as T;
                  var newDialog = Instantiate(prefab, transform);
                  activeDialogs.Add(dialogType, newDialog);
                  newDialog.closeBtn.onClick.AddListener(() => HideDialog<T>());
              }
              if (activeDialogs.TryGetValue(dialogType, out var dialog))
              {
                  dialog.Show();

                  return dialog as T;
              }

              return null;
          }

          public void HideDialog<T>() where T : UIBaseDialog
          {
              //background.DOFade(0f, UIBaseDialog.ANIM_DURATION).OnComplete(() => { background.gameObject.SetActive(false); });

              var dialogType = typeof(T).ToString();
              if (activeDialogs.TryGetValue(dialogType,out var dialog))
              {
                 dialog.Hide();
              }
          }

          public bool IsActive<T>() where T : UIBaseDialog
          {
              var dialogType = typeof(T).ToString();
              if (activeDialogs.TryGetValue(dialogType, out var dialog))
              {
                  return dialog.gameObject.activeInHierarchy;
              }
              return false;
          }

          public T GetActiveDialog<T>() where T : UIBaseDialog
          {
              var dialogType = typeof(T).ToString();
              if (activeDialogs.TryGetValue(dialogType, out var dialog))
              {
                  return dialog as T;
              }
              return null;
          }

          public void HideAllDialogs()
          {
              foreach (var dialog in activeDialogs.Values)
              {
                  if (dialog.gameObject.activeInHierarchy) dialog.Hide();
              }
          }
      }*/

    public class UIDialogManager : MonoSingleton<UIDialogManager>
    {
        public Dictionary<string, UIBaseDialog> activeDialogs = new Dictionary<string, UIBaseDialog>();

        public void RegisterDialog(UIBaseDialog dialog)
        {
            var dialogType = dialog.GetType().ToString();
            if (!activeDialogs.ContainsKey(dialogType))
            {
                activeDialogs.Add(dialogType, dialog);
                dialog.gameObject.SetActive(false); // start hidden
            }
        }

        public T ShowDialog<T>() where T : UIBaseDialog
        {
            var dialogType = typeof(T).ToString();
            if (activeDialogs.TryGetValue(dialogType, out var dialog))
            {
                dialog.Show();
                return dialog as T;
            }
            else
            {
                Debug.LogError($"Dialog of type {dialogType} not found in scene. Did you forget to add it?");   
                return null;
            }
        }

        public void HideDialog<T>() where T : UIBaseDialog
        {
            var dialogType = typeof(T).ToString();
            if (activeDialogs.TryGetValue(dialogType, out var dialog))
            {
                dialog.Hide();
            }
        }

        public bool IsActive<T>() where T : UIBaseDialog
        {
            var dialogType = typeof(T).ToString();
            return activeDialogs.TryGetValue(dialogType, out var dialog) && dialog.gameObject.activeSelf;
        }
    }

}

