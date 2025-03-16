using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダメージ文字を表示
public class DamageUI : MonoBehaviour
{
    [SerializeField] GameObject damageText;//表示するテキスト
    [SerializeField] GameObject hpBar;

    //テキストの表示
    public void DamageDisplay(Vector2 HitPos)
    {
        GameObject dmText= Instantiate(damageText,transform);//文字の表示
        dmText.transform.position= new Vector2(HitPos.x, HitPos.y+1f);//文字の場所を変更
    }
    public void HpDisplay(GameObject enemy)
    {
        GameObject hpB = Instantiate(hpBar, transform);//HPBarの表示
        hpB.transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1f);//HPBarの場所を変更
        HPBar hp = hpB.GetComponent<HPBar>();
        hp.GetEnemyPos(enemy.GetComponent<Enemy>());
    }

}
