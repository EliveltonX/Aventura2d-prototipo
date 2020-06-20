using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public bool infiniteParX;
    public bool infiniteParY;
    public float parallaxFactorX, parallaxFactorY;
    public bool clampY = false;
    public float clampYValueMax, clampYValueMin;

    private Transform mainCameraTrans;
    private Vector3 lastCameraPos;

    private float textureUnitSizeX;
    private float textureUnitSizeY;

    void Start()
    {
        mainCameraTrans = Camera.main.transform;
        lastCameraPos = mainCameraTrans.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

   
    void LateUpdate()
    {
        Vector2 deltaMovement = mainCameraTrans.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x*parallaxFactorX,deltaMovement.y*parallaxFactorY,transform.position.z);
        if (clampY)
        {
            if (transform.position.y > clampYValueMax) { transform.position = new Vector2(transform.position.x, clampYValueMax); }
            if (transform.position.y < clampYValueMin) { transform.position = new Vector2(transform.position.x, clampYValueMin); }
        }
        lastCameraPos = mainCameraTrans.position;

        if (infiniteParX) {

            if (Mathf.Abs(mainCameraTrans.position.x - transform.position.x) >= textureUnitSizeX) 
            {
                float offsetPosX = (mainCameraTrans.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(mainCameraTrans.position.x + offsetPosX, transform.position.y);
            }

        }
        if (infiniteParY)
        {

            if (Mathf.Abs(mainCameraTrans.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offsetPosY = (mainCameraTrans.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, mainCameraTrans.position.y + offsetPosY);
            }

        }


    }
}
