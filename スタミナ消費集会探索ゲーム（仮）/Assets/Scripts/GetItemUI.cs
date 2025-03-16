using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemUI : MonoBehaviour
{
    public List<Item> getItems = new List<Item>();
    [SerializeField] List<Text> texts = new List<Text>();
    int addY;
    int maxDisplay = 5;
    int listCount;
    float removeTime = 4f;
    float removeTimer;

    // Start is called before the first frame update
    void Start()
    {
        removeTimer = 0;
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DisplayItemText());
        listCount =getItems.Count;
    }

    public void AddItem(Item item)
    {
        getItems.Add(item);
    }
    void RemoveItem()
    {
        getItems.Remove(getItems[0]);
    }

    IEnumerator DisplayItemText()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            if (getItems.Count - 1 < i)
            {
                texts[i].text = "";

            }
            else
            {
                if (getItems[i] != null)
                {
                    texts[i].text = getItems[i].name + "‚ð“üŽè";
                }
                else
                {
                    texts[i].text = "";
                }
                yield return new WaitForSeconds(0.1f);
            }
            if (getItems.Count > 0)
            {
                if (listCount == 0)
                {
                    removeTime = 12;
                }
                removeTimer += Time.deltaTime;
                if (removeTime < removeTimer)
                {
                    removeTime = 4;
                    RemoveItem();
                    removeTimer = 0;
                }
            }
            else
            {
                removeTimer = 0;
            }
        }
    }
}
