using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearMenue : MonoBehaviour
{
    [SerializeField] List<Text> menueText;
    int selectNum;//0or1�őI�𐧌�
    int input;//�I�����

    // Start is called before the first frame update
    void Start()
    {
        selectNum = 0;//�����I����������x�V�Ԃ�
        menueText[selectNum].fontStyle=FontStyle.Bold;//�I�����Ă���ق���Text��Bold��
    }

    // Update is called once per frame
    void Update()
    {
        input = (int)Input.GetAxisRaw("Horizontal");//�ړ��L�[�őI��
        selectNum += input;
        //���l�͈̔͂𐧌�
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
        if (Input.GetKeyDown(KeyCode.Space))//�X�y�[�X�L�[�Ō���
        {
            if (selectNum == 0)//�_���W������ʂɖ߂�
            {
                SceneManager.LoadScene("Quest");
            }
            else if (selectNum == 1)//�Q�[���I��
            {
                Application.Quit(); 
            }
        }
    }
}
