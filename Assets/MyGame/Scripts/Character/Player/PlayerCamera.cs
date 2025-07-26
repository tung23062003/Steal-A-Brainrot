using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class PlayerCamera : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image imageCameraControlArea;
    public CinemachineFreeLook cinemachineFreeLook;
    public float sensitiveX, sensitiveY;

    private bool isDragging = false;
    private Vector2 lastDragPosition;

    private void Start()
    {
        imageCameraControlArea = GetComponent<Image>();
    }

    private void Update()
    {
        // N?u ?ang không drag thì reset giá tr? input
        if (!isDragging)
        {
            cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0;
            cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0;
        }
#if UNITY_ANDROID || UNITY_IOS
        else if (Input.touchCount == 0)
        {
            // N?u không còn touch nào thì coi nh? ?ã k?t thúc drag
            isDragging = false;
        }
#endif
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cinemachineFreeLook == null) return;

        isDragging = true;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageCameraControlArea.rectTransform,
            eventData.position,
            eventData.enterEventCamera,
            out Vector2 posOut))
        {
            cinemachineFreeLook.m_XAxis.m_InputAxisValue = eventData.delta.x * sensitiveX * Time.deltaTime;
            cinemachineFreeLook.m_YAxis.m_InputAxisValue = eventData.delta.y * sensitiveY * Time.deltaTime;

            lastDragPosition = eventData.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cinemachineFreeLook == null) return;
        isDragging = true;
        lastDragPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (cinemachineFreeLook == null) return;
        isDragging = false;
        cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0;
    }
}
