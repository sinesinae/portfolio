using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Color white = Color.white;
    Color halftrans = new Color(1, 1, 1, 0.5f);
    Color qtrans = new Color(1, 1, 1, 0.25f);
    RawImage[] rimage;

 
    public void OnBeginDrag(PointerEventData eventData)
    {
        rimage = GetComponentsInChildren<RawImage>();
        GetComponent<Image>().color = qtrans;
        foreach (RawImage ri in rimage)
            ri.color = halftrans;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mpos = Input.mousePosition;
        GetComponent<RectTransform>().position = mpos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = halftrans;
        foreach (RawImage ri in rimage)
            ri.color = white;
    }

}
