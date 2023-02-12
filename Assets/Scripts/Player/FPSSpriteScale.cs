using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSpriteScale : MonoBehaviour
{

    private void Start()
    {
        var cam = GameObject.FindGameObjectWithTag("SpriteCamera").GetComponent<Camera>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        transform.position = cam.transform.position + Vector3.forward * this.transform.position.z;

        transform.localScale = new Vector3(1, 1, 1);
        Vector3 lossyScale = transform.lossyScale;

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = cam.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        //transform.localScale.x = worldScreenWidth / width;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;

        Vector3 newLocalScale = new Vector3(
            transform.localScale.x / lossyScale.x,
            transform.localScale.y / lossyScale.y,
            transform.localScale.z / lossyScale.z
        );

        transform.localScale = newLocalScale;

        // once thats all done, scale the camera size down just a tad
        cam.orthographicSize = 3.5f;
    }

}
