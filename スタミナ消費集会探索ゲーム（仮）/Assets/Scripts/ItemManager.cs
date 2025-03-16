using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField]List<Item>allItemList = new List<Item>();
    public List<int>itemCount = new List<int>();

    List<Item> items=new List<Item>();

    private void Start()
    {
        for (int i = 0; i < allItemList.Count; i++)
        {
            itemCount.Add(0);
        }
    }

    public void addItem(Item item)
    {
        itemCount[item.iD]++;
    }

}
