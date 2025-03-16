using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの向いている方向の制御
public class PlayerDirection : MonoBehaviour
{
    [SerializeField] Camera cam;//メインカメラ
    [SerializeField] GameObject ToolPos;
    [SerializeField] GameObject attackCollision;
    [SerializeField] PlayerController playerController;
    Vector2 mousePos;//マウスの位置
    Vector2 direction;//方向
    public Direction playerDirection;//現在見ている方向
    // Start is called before the first frame update

    //制御する向きの列挙
    public enum Direction
    {
        Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);//マウスも位置をワールド座標からスクリーン座標に変換
        direction = (mousePos - (Vector2)transform.position);//プレイヤーとマウスの位置から方向を計算
        int angle = (int)(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;//角度の計算　　+90は実験の名残
        //向きの条件分け
        if (angle < 23 && angle > -23)
        {
            playerDirection = Direction.Down;//下
        }
        else if (angle > 23 && angle < 68)
        {
            playerDirection = Direction.DownRight;//右下
        }
        else if (angle > 68 && angle < 113)
        {
            playerDirection = Direction.Right;//右
        }
        else if (angle > 113 && angle < 158)
        {
            playerDirection = Direction.UpRight;//右上
        }
        else if (angle > -68 && angle < -23)
        {
            playerDirection = Direction.DownLeft;//左下
        }
        else if (angle > 258 || angle < -68)
        {
            playerDirection = Direction.Left;//左
        }
        else if (angle > 203 && angle < 258)
        {
            playerDirection = Direction.UpLeft;//左上
        }
        else if (angle > 158 && angle < 203)
        {
            playerDirection = Direction.Up;//上
        }
   
        //現在の方向で分岐
        switch (playerDirection)
        {
            //ツールと攻撃の当たり判定の位置と向きの調整

            case Direction.Up:
                ToolPos.transform.localPosition = new Vector3(0.45f, 0.24f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 175);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(0, 0.8f, 0);
                }
                break;
            case Direction.UpRight:
                ToolPos.transform.localPosition = new Vector3(0.6f, -0.1f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 135);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(0.8f, 0.8f, 0);
                }
                break;
            case Direction.Right:
                ToolPos.transform.localPosition = new Vector3(0.5f, -0.45f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 90);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(0.8f, 0, 0);
                }
                break;
            case Direction.DownRight:
                ToolPos.transform.localPosition = new Vector3(-0.1f, -0.6f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 30);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(0.8f, -0.8f, 0);
                }
                break;
            case Direction.Down:
                ToolPos.transform.localPosition = new Vector3(-0.24f, -0.55f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 0);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(0, -0.8f, 0);
                }
                break;
            case Direction.DownLeft:
                ToolPos.transform.localPosition = new Vector3(-0.3f, -0.5f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, -35);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(-0.8f, -0.8f, 0);
                }
                break;
            case Direction.Left:
                ToolPos.transform.localPosition = new Vector3(-0.4f, -0.15f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, 270);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(-0.8f, 0, 0);
                }
                break;
            case Direction.UpLeft:
                ToolPos.transform.localPosition = new Vector3(-0.2f, 0.36f, 0);
                ToolPos.transform.localEulerAngles = new Vector3(0, 0, -150);
                if (playerController.CanHit)
                {
                    attackCollision.transform.localPosition = new Vector3(-0.8f, 0.8f, 0);
                }
                break;
        }
    }

}
