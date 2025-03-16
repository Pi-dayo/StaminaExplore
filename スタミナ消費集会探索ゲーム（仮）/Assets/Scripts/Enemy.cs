using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] int hp;//現在のHP
    [SerializeField] int maxHp;//MAXHP
    [SerializeField] GameObject damageText;
    [SerializeField] List<Item> items;
    GetItem getItem;
    bool isDead = false;//死亡判定
    bool isHpDisplayed=false;//HPBarが一回のみ表示される用
    [SerializeField]int drop;
    static System.Random rand=new System.Random();

DamageUI damageUI;

    public bool IsDead { get => isDead; }
    public int Hp { get => hp; }
    public int MaxHp { get => maxHp;}

    private void Awake()
    {
        isDead = false;
        hp = maxHp;
        damageUI=GameObject.Find("WorldCanvas").GetComponent<DamageUI>();
    }

    //ダメージを受けた時の処理
    public void Damage(int damage)
    {
        damageUI.DamageDisplay(transform.position);//ダメージの文字を出す
        if (!isHpDisplayed)//初めて攻撃を受けたらHPBarを出す
        {
            damageUI.HpDisplay(this.gameObject);
        }
        isHpDisplayed = true;
        hp -= damage;//ダメージ計算
        if (hp <= 0)//HPが0以下になったら
        {
            getItem = GameObject.Find("Player").GetComponent<GetItem>();
            rand.Next(1, drop + 1);
            for (int i = 0; i < drop; i++)
            {
                getItem.AddItem(items[rand.Next(0,items.Count)]);
            }
            isDead = true;//死亡判定
            Destroy(this.gameObject);//ゲームオブジェクトを消す
        }
    }

}
