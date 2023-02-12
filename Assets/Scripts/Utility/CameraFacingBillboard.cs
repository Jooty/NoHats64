using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{

    public Camera mainCam;

    private void Update()
    {
        // search for until found
        if (!mainCam)
            mainCam = Camera.main;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        if (!mainCam) return;
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
            mainCam.transform.rotation * Vector3.up);
    }

}