using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Clear : MonoBehaviour
{

    [SerializeField] DungeonGenerator DungeonGenerator;
    Tilemap tilemap;
    bool isClear=false; //クリア判定

    public bool IsClear { get => isClear;}

    private void Start()
    {
        isClear = false;//初期化
        tilemap = GetComponent<Tilemap>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!tilemap.HasTile(DungeonGenerator.ClearPos)&&!isClear)//クリアのタイルがあるかどうかの判定
        {
            isClear = true;//タイルが消えたらクリア判定
            SceneManager.LoadScene("Clear");//クリア画面へ
        }

    }
}
