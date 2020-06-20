using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Epic Item", menuName = "Inventory System/Items/Epic Item")]
public class Epic : ItemBase
{
    public string Name;

    private void Awake()
    {
        type = ITEMTYPE.Epic;

    }
}
