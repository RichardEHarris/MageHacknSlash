using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallaxController : MonoBehaviour
{
    public Transform background;
    public Transform backLayer;
    public Transform middleLayer;
    public Transform frontLayer;
    public Transform foreground;

    float backMult = 0.9f;
    float middleMult = 0.8f;
    float frontMult = 0.7f;
    float foregroundMult = 0.55f;

    public float offset = -1.36f;
    public float spriteHeight = 1.6f;
    public float spriteWidth = 2.72f;
    Vector3 scale;
    SpriteRenderer bRender;
    SpriteRenderer mRender;
    SpriteRenderer fRender;
    SpriteRenderer foreRender;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
        bRender = backLayer.gameObject.GetComponent<SpriteRenderer>();
        mRender = middleLayer.gameObject.GetComponent<SpriteRenderer>();
        fRender = frontLayer.gameObject.GetComponent<SpriteRenderer>();
        foreRender = foreground.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 rootPos = background.position;
        Vector3 offsetPos = new Vector3(offset, 0, 0);
        offsetPos.x = offsetPos.x * scale.x;
        offsetPos.y = offsetPos.y * scale.y;

        backLayer.position = (rootPos * backMult) + offsetPos;
        middleLayer.position = (rootPos * middleMult) + offsetPos;
        frontLayer.position = (rootPos * frontMult) + offsetPos;
        foreground.position = (rootPos * foregroundMult) + offsetPos;

        bRender.size = new Vector2(Mathf.Abs(backLayer.localPosition.x) + (spriteWidth/2), spriteHeight);
        mRender.size = new Vector2(Mathf.Abs(middleLayer.localPosition.x) + (spriteWidth / 2), spriteHeight);
        fRender.size = new Vector2(Mathf.Abs(frontLayer.localPosition.x) + (spriteWidth / 2), spriteHeight);
        foreRender.size = new Vector2(Mathf.Abs(foreground.localPosition.x) + (spriteWidth / 2), spriteHeight);
    }
}
