using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_���[�W������\��
public class DamageUI : MonoBehaviour
{
    [SerializeField] GameObject damageText;//�\������e�L�X�g
    [SerializeField] GameObject hpBar;

    //�e�L�X�g�̕\��
    public void DamageDisplay(Vector2 HitPos)
    {
        GameObject dmText= Instantiate(damageText,transform);//�����̕\��
        dmText.transform.position= new Vector2(HitPos.x, HitPos.y+1f);//�����̏ꏊ��ύX
    }
    public void HpDisplay(GameObject enemy)
    {
        GameObject hpB = Instantiate(hpBar, transform);//HPBar�̕\��
        hpB.transform.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1f);//HPBar�̏ꏊ��ύX
        HPBar hp = hpB.GetComponent<HPBar>();
        hp.GetEnemyPos(enemy.GetComponent<Enemy>());
    }

}
