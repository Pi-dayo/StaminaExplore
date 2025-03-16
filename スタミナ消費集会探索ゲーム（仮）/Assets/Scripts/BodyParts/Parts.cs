using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの各パーツの制御
public class Parts : MonoBehaviour
{
    [SerializeField] PlayerBodyParts playerBodyParts;//パーツアセット
    SpriteRenderer spriteRenderer;//パーツの画像
    [SerializeField] PlayerDirection pD;//プレイヤーの方向
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerBodyParts.sprite[4];
    }

    // Update is called once per frame
    void Update()
    {
        PartsDirection();
    }

    //パーツの全方向の画像切り替え
    void PartsDirection()
    {
        switch (pD.playerDirection)
        {
            case PlayerDirection.Direction.Up:
                spriteRenderer.sprite = playerBodyParts.sprite[0];
                break;
            case PlayerDirection.Direction.UpRight:
                spriteRenderer.sprite = playerBodyParts.sprite[1];
                break;
            case PlayerDirection.Direction.Right:
                spriteRenderer.sprite = playerBodyParts.sprite[2];
                break;
            case PlayerDirection.Direction.DownRight:
                spriteRenderer.sprite = playerBodyParts.sprite[3];
                break;
            case PlayerDirection.Direction.Down:
                spriteRenderer.sprite = playerBodyParts.sprite[4];
                break;
            case PlayerDirection.Direction.DownLeft:
                spriteRenderer.sprite = playerBodyParts.sprite[5];
                break;
            case PlayerDirection.Direction.Left:
                spriteRenderer.sprite = playerBodyParts.sprite[6];
                break;
            case PlayerDirection.Direction.UpLeft:
                spriteRenderer.sprite = playerBodyParts.sprite[7];
                break;
        }
    }
}
