using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Vector2 pos;
    new Camera camera;
    Enemy enemy;
    [SerializeField] GameObject hb;
    [SerializeField] List<Color> colors;//HP�̊����ŕς��F
    private void Start()
    {
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        pos = new Vector2(transform.position.x, transform.position.y);//HPBar�\���̏����ʒu
    }
    private void Update()
    {
        if (enemy != null)//Enemy�X�N���v�g�����Ă���
        {
            FloatText(enemy);//HPBar�̕\��
            NowHP(enemy.MaxHp, enemy.Hp);//���݂�HP��UI
        }
        if (enemy.IsDead)
        {
            Destroy(this.gameObject);//HP��0�ɂȂ��������
        }
    }

    public void GetEnemyPos(Enemy em)
    {
        enemy=em.GetComponent<Enemy>();
    }
    //�����𕂂�����
    void FloatText(Enemy em)
    {
        Vector3 newPos = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(pos));//������\���J�n�ʒu�ɌŒ肷�邽�߂̂���
        this.transform.position = new Vector3(em.transform.position.x, em.transform.position.y+0.5f, 0);
    }
    //HP�̊����ŐF���ω�
    void NowHP(int maxHp, int hp)
    {
        float hpPercent = hp*1.0f / maxHp*1.0f;
        hb.transform.localScale= new Vector2(hpPercent,1);
        Image hpImg=hb.GetComponent<Image>();
        if (hpPercent > 0.6f)
        {
            hpImg.color = colors[0];
        }
        else if (hpPercent < 0.6f && hpPercent > 0.3f)
        {
            hpImg.color = colors[1];
        }
        else if(hpPercent <0.3f)
        {
            hpImg.color = colors[2];
        }
    }
}
