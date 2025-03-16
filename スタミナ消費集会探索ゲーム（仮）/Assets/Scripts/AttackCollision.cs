using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//�U���̓����蔻�萧��
public class AttackCollision : MonoBehaviour
{
    BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float canHitTime;
    [SerializeField] int hitNum;
    bool isHit=false;//���d�q�b�g�h�~�p�i�ł��ĂȂ��j

    static System.Random rand = new System.Random();
    public bool IsHit { get => isHit; set => isHit = value; }

    // Start is called before the first frame update
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer= GetComponent<SpriteRenderer>();//�U���G�t�F�N�g
    }
    private void Update()
    {
        Debug.Log(isHit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null&&!isHit)//�U���͈͂ɓG�X�N���v�g�����I�u�W�F�N�g��������
        {
            int hits= rand.Next(1,hitNum+1);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            StartCoroutine(HitsNum(hits,enemy));
        }
    }

    //�U���������鎞��
    public void HitTime(bool canHit)
    {
        if (canHit)//�U���\���ԂȂ�
        {
            //�����蔻��ƍU���G�t�F�N�g������
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
            transform.localPosition = Vector3.zero;//�ꏊ���������ƂȂ����U����������Ȃ�����
        }
    }

    IEnumerator HitsNum(int hits,Enemy enemy)
    {
        for (int i = 0; i < hits; i++)
        {
            if (enemy != null)
            {
                enemy.Damage(1);//�G�Ƀ_���[�W
            }
            yield return null;
        }
    }
}
