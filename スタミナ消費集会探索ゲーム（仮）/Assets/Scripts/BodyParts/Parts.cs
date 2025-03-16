using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̊e�p�[�c�̐���
public class Parts : MonoBehaviour
{
    [SerializeField] PlayerBodyParts playerBodyParts;//�p�[�c�A�Z�b�g
    SpriteRenderer spriteRenderer;//�p�[�c�̉摜
    [SerializeField] PlayerDirection pD;//�v���C���[�̕���
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

    //�p�[�c�̑S�����̉摜�؂�ւ�
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
