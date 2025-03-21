using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavingTool : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] List<Sprite> sprits;
    [SerializeField] PlayerController playerController;
    SpriteRenderer spriteRenderer;
    int toolNum;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        toolNum = playerController.HavingtoolNum;
        spriteRenderer.sprite = sprits[playerController.HavingtoolNum];
    }
    void Update()
    {
        if (toolNum != playerController.HavingtoolNum)
        {
            spriteRenderer.sprite = sprits[playerController.HavingtoolNum];
        }
        //ツールの細かい動き
        Vector2 direction = ((Vector2)cam.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position)).normalized / 10;
        transform.localPosition = direction;
        toolNum = playerController.HavingtoolNum;
    }

}