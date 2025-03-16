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
    [SerializeField] List<Color> colors;//HPの割合で変わる色
    private void Start()
    {
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        pos = new Vector2(transform.position.x, transform.position.y);//HPBar表示の初期位置
    }
    private void Update()
    {
        if (enemy != null)//Enemyスクリプトがついてたら
        {
            FloatText(enemy);//HPBarの表示
            NowHP(enemy.MaxHp, enemy.Hp);//現在のHPのUI
        }
        if (enemy.IsDead)
        {
            Destroy(this.gameObject);//HPが0になったら消す
        }
    }

    public void GetEnemyPos(Enemy em)
    {
        enemy=em.GetComponent<Enemy>();
    }
    //文字を浮かせる
    void FloatText(Enemy em)
    {
        Vector3 newPos = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(pos));//文字を表示開始位置に固定するためのもの
        this.transform.position = new Vector3(em.transform.position.x, em.transform.position.y+0.5f, 0);
    }
    //HPの割合で色が変化
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
