using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class NetworkJoyStickMovement : MonoBehaviourPun
{
    public static NetworkJoyStickMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkJoyStickMovement>();
                if (instance == null)
                {
                    var instaceContainer = new GameObject("NetworkJoyStickMovement");
                    instance = instaceContainer.AddComponent<NetworkJoyStickMovement>();
                }
            }
            return instance;
        }
    }

    private static NetworkJoyStickMovement instance;

    public GameObject smallStick;
    public GameObject bGStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joyStickFirstPosition;
    float stickRadius;

    // Start is called before the first frame update
    
    void Start()
    {
        stickRadius = bGStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joyStickFirstPosition = bGStick.transform.position;
    }

    // Update is called once per frame
    
    [PunRPC]
    public void Drag(BaseEventData baseEventData)
    {

        
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;
        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    [PunRPC]
    public void PointDown()
    {
        
        bGStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;
    }

    [PunRPC]
    public void Drop()
    {
        Debug.Log("Drop");
        joyVec = Vector3.zero;
        bGStick.transform.position = joyStickFirstPosition;
        smallStick.transform.position = joyStickFirstPosition;
    }
}
