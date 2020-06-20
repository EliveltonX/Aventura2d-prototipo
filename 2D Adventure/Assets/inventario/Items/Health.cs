using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Health Item",menuName ="Inventory System/Items/Health Item")]
public class Health : ItemBase
{

    public int lifeRegen = 30;

    private void Awake()
    {
        type = ITEMTYPE.Health;
        
    }
}
