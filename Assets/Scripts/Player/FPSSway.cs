using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSway : MonoBehaviour
{

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initPos;

    private void Start()
    {
        initPos = transform.localPosition;
    }

    private void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") - Input.GetAxis("Horizontal") * amount;
        float movementY = -Input.GetAxis("Mouse Y") - Input.GetAxis("Vertical") * amount;
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initPos, Time.deltaTime * smoothAmount);
    }

}
