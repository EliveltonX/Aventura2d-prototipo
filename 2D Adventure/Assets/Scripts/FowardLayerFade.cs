using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowardLayerFade : MonoBehaviour
{
    public Vector3 size;
    public LayerMask Activelayer;
    private SpriteRenderer spRender;
    private Color cor;


    private void Awake()
    {
        spRender = GetComponent<SpriteRenderer>();
        cor = spRender.color;
    }


    void Update()
    {

        Collider2D[] coliders = Physics2D.OverlapBoxAll(transform.position, size, 0f, Activelayer);

        if (coliders.Length>0)
        {

            spRender.color = new Color(spRender.color.r, spRender.color.g, spRender.color.b, 0.8f);

        }
        else
        {
            spRender.color = new Color(spRender.color.r, spRender.color.g, spRender.color.b, 255f);
        }

        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
