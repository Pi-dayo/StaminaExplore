using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [SerializeField] GetItemUI getItemUI;
    [SerializeField] ItemManager itemManager;
    public List<Item> items=new List<Item>();
    public void AddItem(Item item)
    {
        items.Add(item);
        getItemUI.AddItem(item);
        itemManager.addItem(item);
    }

}
