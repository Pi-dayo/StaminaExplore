using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] int hp;//���݂�HP
    [SerializeField] int maxHp;//MAXHP
    [SerializeField] GameObject damageText;
    [SerializeField] List<Item> items;
    GetItem getItem;
    bool isDead = false;//���S����
    bool isHpDisplayed=false;//HPBar�����̂ݕ\�������p
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

    //�_���[�W���󂯂����̏���
    public void Damage(int damage)
    {
        damageUI.DamageDisplay(transform.position);//�_���[�W�̕������o��
        if (!isHpDisplayed)//���߂čU�����󂯂���HPBar���o��
        {
            damageUI.HpDisplay(this.gameObject);
        }
        isHpDisplayed = true;
        hp -= damage;//�_���[�W�v�Z
        if (hp <= 0)//HP��0�ȉ��ɂȂ�����
        {
            getItem = GameObject.Find("Player").GetComponent<GetItem>();
            rand.Next(1, drop + 1);
            for (int i = 0; i < drop; i++)
            {
                getItem.AddItem(items[rand.Next(0,items.Count)]);
            }
            isDead = true;//���S����
            Destroy(this.gameObject);//�Q�[���I�u�W�F�N�g������
        }
    }

}
