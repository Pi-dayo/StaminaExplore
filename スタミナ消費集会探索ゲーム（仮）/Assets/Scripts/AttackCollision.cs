using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//攻撃の当たり判定制御
public class AttackCollision : MonoBehaviour
{
    BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float canHitTime;
    [SerializeField] int hitNum;
    bool isHit=false;//多重ヒット防止用（できてない）

    static System.Random rand = new System.Random();
    public bool IsHit { get => isHit; set => isHit = value; }

    // Start is called before the first frame update
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer= GetComponent<SpriteRenderer>();//攻撃エフェクト
    }
    private void Update()
    {
        Debug.Log(isHit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null&&!isHit)//攻撃範囲に敵スクリプトをもつオブジェクトがいたら
        {
            int hits= rand.Next(1,hitNum+1);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(HitsNum(hits,enemy));
        }
    }

    //攻撃があたる時間
    public void HitTime(bool canHit)
    {
        if (canHit)//攻撃可能時間なら
        {
            //あたり判定と攻撃エフェクトをつける
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
            transform.localPosition = Vector3.zero;//場所が同じだとなぜか攻撃が当たらないため
        }
    }

    IEnumerator HitsNum(int hits,Enemy enemy)
    {
        for (int i = 0; i < hits; i++)
        {
            if (enemy != null)
            {
                enemy.Damage(1);//敵にダメージ
            }
            yield return null;
        }
    }
}
