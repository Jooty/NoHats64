using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteMask : MonoBehaviour
{

    // locals
    private SpriteRenderer Renderer;
    private SpriteMask mask;

    private void Awake()
    {
        this.Renderer = this.GetComponent<SpriteRenderer>();
        this.mask     = this.GetComponent<SpriteMask>();
    }

    private void LateUpdate()
    {
        mask.sprite = Renderer.sprite;
    }

}
