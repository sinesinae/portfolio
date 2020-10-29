using System.Collections;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // camera will follow this object
    public Transform Target;
    //camera transform
    public Transform camTransform;
    // offset between camera and target
    public Vector3 Offset;
    public Vector3 ZoomOffset;
    // change this value to get desired smoothness
    public float SmoothTime = 0f;

    // This value will change at the runtime depending on target movement. Initialize with zero vector.
    private Vector3 velocity = Vector3.zero;

    private bool isCameraZoom;

    private Vector3 targetPosition;
    private Vector3 zoomTargetPosition;

    private bool CamZoomisRunning;
    private bool CamZoomoutRunning;

    private void Start()
    {
        Offset = new Vector3(0, 3, -3);
        targetPosition = Target.position + Offset;
        CamZoomisRunning = false;
        CamZoomoutRunning = false;


    }

    private void LateUpdate()
    {


        if (Input.GetKeyDown(KeyCode.T) && !CamZoomisRunning && !CamZoomoutRunning)
        {
            ZoomOffset = new Vector3(0, 1.2f, -1.8f);
            zoomTargetPosition = Target.position + ZoomOffset;
            CamZoomisRunning = true;
            StartCoroutine(ZoomCam());
        }


        if (!CamZoomisRunning && !CamZoomoutRunning)
        {
            SmoothTime = 0f;
            //Offset = new Vector3(0, 3, -3);
            // update position
            targetPosition = Target.position + Offset;
            camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

            // update rotation
            transform.LookAt(Target);
        }


    }

    IEnumerator ZoomCam()
    {
        Debug.Log("AA");
        while (Vector3.SqrMagnitude(transform.position - zoomTargetPosition) > 0.0001f && CamZoomisRunning)
        {   // 종료지점에 거의 근접할때까지 이동을 진행
            transform.position = Vector3.Lerp(transform.position, zoomTargetPosition, 0.1f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, EndCamPos.rotation, Time.deltaTime);
            yield return null;
        }
        CamZoomisRunning = false;
        if (!CamZoomoutRunning)
        {
            Offset = new Vector3(0, 3, -3);
            targetPosition = Target.position + Offset;
            StartCoroutine(ZoomOut());
        }
    }
    IEnumerator ZoomOut()
    {
        while (Vector3.SqrMagnitude(transform.position - targetPosition) > 0.0001f)
        {   // 종료지점에 거의 근접할때까지 이t동을 진행
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.3f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetPosition, Time.deltaTime);

            CamZoomoutRunning = true;
            yield return null;
        }
        CamZoomoutRunning = false;
    }
}