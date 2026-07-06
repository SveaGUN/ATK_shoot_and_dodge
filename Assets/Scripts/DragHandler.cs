using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action OnBeginDragEvent;
    public event Action OnDragEvent;
    public event Action OnEndDragEvent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        OnBeginDragEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        OnDragEvent?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        OnEndDragEvent?.Invoke();
    }

    private void OnDestroy()
    {
        if (OnBeginDragEvent != null)
        {
            foreach (var d in OnBeginDragEvent.GetInvocationList())
            {
                OnBeginDragEvent -= (Action)d;
            }
        }

        if (OnDragEvent != null)
        {
            foreach (var d in OnDragEvent.GetInvocationList())
            {
                OnDragEvent -= (Action)d;
            }
        }

        if (OnEndDragEvent != null)
        {
            foreach (var d in OnEndDragEvent.GetInvocationList())
            {
                OnEndDragEvent -= (Action)d;
            }
        }
    }
}
