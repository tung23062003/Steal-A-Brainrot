using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PreventMultipleClick : MonoBehaviour
{
    [SerializeField] private float buttonReactivateDelay = 1f;
    [SerializeField] private bool activeWhenEnable = true;

    private Button TheButton;
    private WaitForSeconds waitSeconds;

    private void Awake()
    {
        if (GetComponent<Button>() != null)
        {
            TheButton = GetComponent<Button>();
            TheButton.onClick.AddListener(WhenClicked);
        }
        waitSeconds = new WaitForSeconds(buttonReactivateDelay);
    }

    private void OnEnable()
    {
        if (TheButton != null && activeWhenEnable)
        {
            TheButton.interactable = true;
        }
    }

    // Assign this as your OnClick listener from the inspector
    public void WhenClicked()
    {
        if (TheButton != null)
        {
            TheButton.interactable = false;
        }

        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        StartCoroutine(EnableButtonAfterDelay(TheButton));

        // Do whatever else your button is supposed to do.
    }

    IEnumerator EnableButtonAfterDelay(Button button)
    {
        yield return waitSeconds;

        if (button != null)
        {
            button.interactable = true;
        }
    }

}
