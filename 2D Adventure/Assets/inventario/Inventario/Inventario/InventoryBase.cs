using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : ScriptableObject
{

    public int invSize = 5;
    public List<ItemBase> Container = new List<ItemBase>();

    public void AddItem(ItemBase _item)  //Metodo para adicionar novos Items a mochila
    {
        for (int i = 0; i < Container.Count; i++)                       //Laço para buscar os items do container
        {
            if (Container[i] == _item && _item.canStack == true)        //Se o item ja existir
            {
                Container[i].amount += _item.amount;                    //Sera adicionado apenas a quantidade
                return;                                                 //Dessa forma estacando os items
            }
        }
        if (Container.Count <= invSize)                                 //caso nao tenha item igual e ainda houver espaço
        {
            Container.Add(_item);                                       //adicionar o item e quantidade ao container
            return;
        }
        else
        {
            Debug.Log("Seu Inventario esta Cheio");     //Se passou por todos testes ate aqui significa q nao ha espaço na mochila.
        }
    }
}
