using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] PlayerController playerController;
    [SerializeField] Text toolText;
    int toolNum;
    // Start is called before the first frame update
    void Start()
    {
        toolNum=playerController.HavingtoolNum;
        image.sprite= sprites[playerController.HavingtoolNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (toolNum != playerController.HavingtoolNum)
        {
            image.sprite = sprites[playerController.HavingtoolNum];
        }
        if (toolNum == 0)
        {
            toolText.text = "��͂�";
        }
        else if (toolNum == 1)
        {
            toolText.text = "��";
        }
        toolNum =playerController.HavingtoolNum;
    }
}
