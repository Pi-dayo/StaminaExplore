using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̌����Ă�������̐���
public class PlayerDirection : MonoBehaviour
{
    [SerializeField] Camera cam;//���C���J����
    [SerializeField] GameObject ToolPos;
    [SerializeField] GameObject attackCollision;
    [SerializeField] PlayerController playerController;
    Vector2 mousePos;//�}�E�X�̈ʒu
    Vector2 direction;//����
    public Direction playerDirection;//���݌��Ă������
    // Start is called before the first frame update

    //���䂷������̗�
    public enum Direction
    {
        Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);//�}�E�X���ʒu�����[���h���W����X�N���[�����W�ɕϊ�
        direction = (mousePos - (Vector2)transform.position);//�v���C���[�ƃ}�E�X�̈ʒu����������v�Z
        int angle = (int)(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;//�p�x�̌v�Z�@�@+90�͎����̖��c
        //�����̏�������
        if (angle < 23 && angle > -23)
        {
            playerDirection = Direction.Down;//��
        }
        else if (angle > 23 && angle < 68)
        {
            playerDirection = Direction.DownRight;//�E��
        }
        else if (angle > 68 && angle < 113)
        {
            playerDirection = Direction.Right;//�E
        }
        else if (angle > 113 && angle < 158)
        {
            playerDirection = Direction.UpRight;//�E��
        }
        else if (angle > -68 && angle < -23)
        {
            playerDirection = Direction.DownLeft;//����
        }
        else if (angle > 258 || angle < -68)
        {
            playerDirection = Direction.Left;//��
        }
        else if (angle > 203 && angle < 258)
        {
            playerDirection = Direction.UpLeft;//����
        }
        else if (angle > 158 && angle < 203)
        {
            playerDirection = Direction.Up;//��
        }
   
        //���݂̕����ŕ���
        switch (playerDirection)
        {
            //�c�[���ƍU���̓����蔻��̈ʒu�ƌ����̒���

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
