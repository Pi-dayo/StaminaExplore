using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearMenue : MonoBehaviour
{
    [SerializeField] List<Text> menueText;
    int selectNum;//0or1で選択制御
    int input;//選択入力

    // Start is called before the first frame update
    void Start()
    {
        selectNum = 0;//初期選択をもう一度遊ぶに
        menueText[selectNum].fontStyle=FontStyle.Bold;//選択しているほうのTextをBoldに
    }

    // Update is called once per frame
    void Update()
    {
        input = (int)Input.GetAxisRaw("Horizontal");//移動キーで選択
        selectNum += input;
        //数値の範囲を制御
        if (selectNum > 1)
        {
            selectNum = 1;
            menueText[selectNum].fontStyle = FontStyle.Bold;
            menueText[0].fontStyle = FontStyle.Normal;
        }
        if (selectNum < 0)
        {
            selectNum = 0;
            menueText[selectNum].fontStyle = FontStyle.Bold;
            menueText[1].fontStyle = FontStyle.Normal;
        }
        if (Input.GetKeyDown(KeyCode.Space))//スペースキーで決定
        {
            if (selectNum == 0)//ダンジョン画面に戻る
            {
                SceneManager.LoadScene("Quest");
            }
            else if (selectNum == 1)//ゲーム終了
            {
                Application.Quit(); 
            }
        }
    }
}
