using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    Vector2 pos;
    new Camera camera;
    float floating=0;//�����𕂂����邽�߂̐��l�~�ϗp
    float floatSpeed = 1;//��������������
    [SerializeField] float floatTime;//�����̕\������
    float timer;//�����\���p�^�C�}�[
    static System.Random random = new System.Random();//�������o��ʒu�̃����_�����p
    float x, y;//��ɓ���
    private void Start()
    {
        camera =GameObject.Find("Camera").GetComponent<Camera>();
        x = random.Next(-1, 2)*0.2f;//�����̃����_���ʒuX
        y = random.Next(-1, 2)*0.2f; //�����̃����_���ʒuY
       pos = new Vector2(transform.position.x+x,transform.position.y+y) ;//�����\���̏����ʒu
    }
    private void Update()
    {
        FloatText();
    }

    //�����𕂂�����
    void FloatText()
    {
        timer += Time.deltaTime;//�^�C�}�[
        if (timer > floatTime)
        {
            Destroy(this.gameObject);//�\�����Ԃ𒴂�����Q�[���I�u�W�F��j��
        }
        floating += floatSpeed * Time.deltaTime;//y���ɕ������邽�߂̒~��
        Vector3 newPos = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(pos));//������\���J�n�ʒu�ɌŒ肷�邽�߂̂���
        this.transform.position = new Vector3(newPos.x, newPos.y + floating, 0);//�����𕂂�����
    }
}
