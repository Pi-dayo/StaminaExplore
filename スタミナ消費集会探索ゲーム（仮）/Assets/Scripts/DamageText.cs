using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    Vector2 pos;
    new Camera camera;
    float floating=0;//文字を浮かせるための数値蓄積用
    float floatSpeed = 1;//文字が浮く速さ
    [SerializeField] float floatTime;//文字の表示時間
    float timer;//文字表示用タイマー
    static System.Random random = new System.Random();//文字が出る位置のランダム性用
    float x, y;//上に同じ
    private void Start()
    {
        camera =GameObject.Find("Camera").GetComponent<Camera>();
        x = random.Next(-1, 2)*0.2f;//文字のランダム位置X
        y = random.Next(-1, 2)*0.2f; //文字のランダム位置Y
       pos = new Vector2(transform.position.x+x,transform.position.y+y) ;//文字表示の初期位置
    }
    private void Update()
    {
        FloatText();
    }

    //文字を浮かせる
    void FloatText()
    {
        timer += Time.deltaTime;//タイマー
        if (timer > floatTime)
        {
            Destroy(this.gameObject);//表示時間を超えたらゲームオブジェを破壊
        }
        floating += floatSpeed * Time.deltaTime;//y軸に浮かせるための蓄積
        Vector3 newPos = camera.WorldToScreenPoint(camera.ScreenToWorldPoint(pos));//文字を表示開始位置に固定するためのもの
        this.transform.position = new Vector3(newPos.x, newPos.y + floating, 0);//文字を浮かせる
    }
}
