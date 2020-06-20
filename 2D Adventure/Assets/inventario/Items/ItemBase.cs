using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEMTYPE
{
    Ring,
    Health,
    Epic
}

public abstract class ItemBase : ScriptableObject
{

    public int amount = 1;
    public bool canStack = true;
    public ITEMTYPE type;
    [TextArea(5,10)]
    public string Description = "(Descrição) Item Desconhecido !";
    public GameObject iconPrefab;
    public GameObject gamePrefab;

}
