using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Ring Item",menuName = "Inventory System/Items/Ring")]
public class Ring : ItemBase
{
    public int bonusAtack = 1;
    public int bonusDefense = 1;

    public void Awake()
    {
        type = ITEMTYPE.Ring;
    }
}
