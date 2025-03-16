using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Clear : MonoBehaviour
{

    [SerializeField] DungeonGenerator DungeonGenerator;
    Tilemap tilemap;
    bool isClear=false; //�N���A����

    public bool IsClear { get => isClear;}

    private void Start()
    {
        isClear = false;//������
        tilemap = GetComponent<Tilemap>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!tilemap.HasTile(DungeonGenerator.ClearPos)&&!isClear)//�N���A�̃^�C�������邩�ǂ����̔���
        {
            isClear = true;//�^�C������������N���A����
            SceneManager.LoadScene("Clear");//�N���A��ʂ�
        }

    }
}
