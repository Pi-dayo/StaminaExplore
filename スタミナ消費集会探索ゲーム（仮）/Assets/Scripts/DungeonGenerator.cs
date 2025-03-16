using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tile;//設置するタイルマップ
    [SerializeField] Tilemap clearTile;//クリアタイル用タイルマップ

    [SerializeField] List<TileBase> tileBase;//タイルリスト

    [SerializeField] TileBase clearTileBase;//クリアタイル

    [SerializeField] List<TilesInfomation> wallTileInfo;//未実装
    [SerializeField] List<TilesInfomation> itemTiletileInfo;//未実装
    [SerializeField] List<TilesInfomation> derivationTileInfo;//未実装
    [SerializeField] List<TilesInfomation> obstacleTileInfo;//未実装

    [SerializeField] int width, height;//ダンジョン全体の横縦
    [SerializeField] int decorationMinNum;//装飾の最小サイズ
    [SerializeField] int decorationMaxNum;//装飾の最大サイズ
    [SerializeField] int itemTileRato;//未実装
    [SerializeField] int obstacleTileRato;//未実装
    [SerializeField] int derivationTileRato;//未実装

    Vector3Int startPos;//プレイヤーのスタート位置
    Vector3Int clearPos;//クリアタイルの位置
    Vector2 decorationPos;//装飾の位置

    int startRandom;//スタート位置をランダムな部屋にする
    int clearRandom;//クリア位置をランダムな部屋にする
    int areasNum;//スタートとクリアの判別用
    int tileRandom;//ランダムなタイルを選択
    int decorationSize;//装飾のサイズ
    int decorationNum;//装飾の数


    int tileRato;//配置するタイルの割合
    public Vector3Int StartPos { get => startPos; }
    public Vector3Int ClearPos { get => clearPos; }
    static System.Random random = new System.Random((int)DateTime.Now.Ticks);

    //エリア分割用
    public class Area
    {
        public int width, height;//横幅、立幅
        public int left, right, bottom, top;//左、右、下、上の位置
        public int pathJointPos;//エリア分割の場所
        public Vector2 pathPoint;//道の開始地点
        public bool vh = false;//エリアを縦か横に分割するか

        //何もなし
        public Area()
        {

        }
        //セット
        public void Set(int left, int right, int bottom, int top, int pathPos, bool vh)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;

            this.width = right - left;
            this.height = top - bottom;
            this.pathJointPos = pathPos;
            this.vh = vh;
        }


    }
    Area dgArea;//ダンジョンのエリア
    List<Area> areas = new List<Area>();//分割したエリアを入れるようのリスト
    List<Area> pathPoints = new List<Area>();//道をつなげるリスト
    void Awake()
    {
        dgArea = new Area();//初期化
        dgArea.Set(0, width, 0, height, 0, false);//ダンジョンの全体のセット
        areas.Add(dgArea);//分割用区画の初期値をセット
        DivArear(areas);//エリアの分割
        areasNum = 0;//初期化
        startRandom = random.Next(0, areas.Count);//初期値の設定
        clearRandom = random.Next(0, areas.Count);//初期値の設定

        //ダンジョンを全部埋める （タイルの割合で）
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                tileRato=random.Next(0, 100);
                if (tileRato < 5)//全体の5%石炭
                {
                    tile.SetTile(new Vector3Int(i, j, 0), itemTiletileInfo[0].tile);
                }
                else if(tileRato < 15 && tileRato > 5)//全体の10%ガチャ鉱石
                {
                    tileRato = random.Next(0, 100);
                    if (tileRato < 30)//30%1段階目発展
                    {
                        tile.SetTile(new Vector3Int(i, j, 0), derivationTileInfo[1].tile);
                    }
                    else//70%0段階目発展
                    {
                        tile.SetTile(new Vector3Int(i, j, 0), derivationTileInfo[0].tile);
                    }
                }
                else//80%ただの障害物
                {
                    tile.SetTile(new Vector3Int(i, j, 0), obstacleTileInfo[0].tile);
                }
            }
        }
        decorationNum = random.Next(decorationMinNum, decorationMaxNum);//洞窟の数
        for(int i = 0; i < decorationNum; i++)
        {
            decorationPos = new Vector2(random.Next(0,width),random.Next(0,height));//洞窟の位置を決定
            decorationSize = random.Next(1, 15);//洞窟のサイズを決定
            //洞窟をランダムウォークで拡張
            for(int j = 0;j < decorationSize; j++)
            {
                int x, y;
                x = random.Next(-1, 2);
                y = random.Next(-1, 2);
                if((int)decorationPos.x + x>0&&(int)decorationPos.x++<width&& (int)decorationPos.y + y > 0&& (int)decorationPos.y+ y< height)
                {
                    tile.SetTile(new Vector3Int((int)decorationPos.x + x, (int)decorationPos.y + y, 0), null);
                }
            }
        }
        //岩を設置
        for (int i = 0; i < decorationNum; i++)
        {
            decorationPos = new Vector2(random.Next(0, width), random.Next(0, height));
            decorationSize = random.Next(1, 15);
            for (int j = 0; j < decorationSize; j++)
            {
                int x, y;
                x = random.Next(-1, 2);
                y = random.Next(-1, 2);
                if ((int)decorationPos.x + x > 0 && (int)decorationPos.x+x < width && (int)decorationPos.y + y > 0 && (int)decorationPos.y + y < height)
                {
                    tile.SetTile(new Vector3Int((int)decorationPos.x + x, (int)decorationPos.y + y, 0), tileBase[tileRandom]);
                }
            }
        }

        //部屋を設置
        foreach (Area area in areas)
        {
            Area room = new Area();//部屋用変数
            room = Rooms(area);//部屋の初期化
            pathPoints.Add(room);//部屋の道の場所をリストに入れる

            //部屋の作成
            for (int i = room.left; i <= room.right; i++)
            {
                for (int j = room.bottom; j <= room.top; j++)
                {
                    tile.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
            //スタートの部屋だったら
            if (areasNum == startRandom)
            {
                //部屋内でスタート位置を決める
                int posX = random.Next(room.left, room.right);
                int posY = random.Next(room.bottom, room.top);
                startPos = new Vector3Int(posX, posY);

            }
            //クリアの部屋だったら
            if (areasNum == clearRandom)
            {
                //部屋内でクリア位置を決める
                clearPos = new Vector3Int();
                int posX = random.Next(room.left, room.right);
                int posY = random.Next(room.bottom, room.top);
                clearPos = new Vector3Int(posX, posY);
            }
            areasNum++;//スタートとクリアの部屋の確認用
        }
        clearTile.SetTile(new Vector3Int((int)clearPos.x, (int)clearPos.y, 0), clearTileBase);//クリアタイルの設置


        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            int start, end;
            //部屋の区切り方が縦か横か
            if (!pathPoints[i].vh)
            {
                //部屋から区切り位置まで道を引く
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.x);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.x);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(j, (int)pathPoints[i].pathPoint.y, 0), null);
                }
                //次の部屋から前の区切り位置まで道を引く
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.x);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.x);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(j, (int)pathPoints[i + 1].pathPoint.y, 0), null);
                }
                //道同士をつなげる
                start = Math.Min((int)pathPoints[i].pathPoint.y, (int)pathPoints[i + 1].pathPoint.y);
                end = Math.Max((int)pathPoints[i].pathPoint.y, (int)pathPoints[i + 1].pathPoint.y);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(pathPoints[i].pathJointPos, j, 0), null);
                }
            }
            //上と同様
            else
            {
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.y);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.y);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int((int)pathPoints[i].pathPoint.x, j, 0), null);
                }
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.y);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.y);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int((int)pathPoints[i + 1].pathPoint.x, j, 0), null);
                }
                start = Math.Min((int)pathPoints[i].pathPoint.x, (int)pathPoints[i + 1].pathPoint.x);
                end = Math.Max((int)pathPoints[i].pathPoint.x, (int)pathPoints[i + 1].pathPoint.x);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(j, pathPoints[i].pathJointPos, 0), null);
                }
            }
        }


    }

    [SerializeField] int divMinSize = 5;//分割最小サイズ　5*5
    //区画分割
    public void DivArear(List<Area> areas)
    {
        //分割して両方分割最小サイズより小さかったらリターン
        if (areas[0].right - divMinSize < divMinSize && areas[0].top - divMinSize < divMinSize)
        {
            return;
        }
        //横に分割できるサイズなら
        if (areas[0].width - divMinSize > divMinSize && areas[0].right - divMinSize > areas[0].left)
        {
            //縦でも分割できるか確認　できるなら縦横で抽選　できないなら横に分割
            if (areas[0].height - divMinSize > divMinSize && areas[0].top - divMinSize > areas[0].bottom)
            {
                int divXY = random.Next(0, 2);//縦に割るか横に割るか
                if (divXY == 0)
                {
                    int divPos = random.Next(areas[0].left + divMinSize, areas[0].right - divMinSize);//分割位置の決定
                    //２つの部屋に分割
                    Area parent = new Area();
                    Area child = new Area();

                    parent.Set(areas[0].left, divPos, areas[0].bottom, areas[0].top, divPos, false);
                    child.Set(divPos, areas[0].right, areas[0].bottom, areas[0].top, divPos, false);
                    //分割元を削除
                    areas.Remove(areas[0]);

                    //大きいほうを分割もとに設定してループ
                    if (parent.width * parent.height > child.width * child.height)
                    {
                        areas.Add(parent);
                        areas.Add(child);
                        DivArear(areas);
                    }
                    else
                    {
                        areas.Add(child);
                        areas.Add(parent);
                        DivArear(areas);
                    }

                }
                else if (divXY == 1)
                {

                    int divPos = random.Next(areas[0].bottom + divMinSize, areas[0].top - divMinSize);

                    Area parent = new Area();
                    Area child = new Area();
                    parent.Set(areas[0].left, areas[0].right, divPos, areas[0].top, divPos, true);
                    child.Set(areas[0].left, areas[0].right, areas[0].bottom, divPos, divPos, true);
                    areas.Remove(areas[0]);
                    if (parent.width * parent.height > child.width * child.height)
                    {
                        areas.Add(parent);
                        areas.Add(child);
                        DivArear(areas);
                    }
                    else
                    {
                        areas.Add(child);
                        areas.Add(parent);
                        DivArear(areas);
                    }
                }
            }
            //縦横で割れないならループ解除
            else if (areas[0].top - areas[0].bottom - divMinSize < divMinSize || areas[0].top - divMinSize < areas[0].bottom)
            {
                return;
            }
        }
        //上と同じ
        else if (areas[0].top - areas[0].bottom - divMinSize > divMinSize && areas[0].top - divMinSize > areas[0].bottom)
        {
            if (areas[0].right - areas[0].left - divMinSize > divMinSize && areas[0].right - divMinSize > areas[0].left)
            {
                int divXY = random.Next(0, 2);
                if (divXY == 0)
                {
                    int divPos = random.Next(areas[0].left + divMinSize, areas[0].right - divMinSize + 1);
                    Area parent = new Area();
                    Area child = new Area();

                    parent.Set(areas[0].left, divPos, areas[0].bottom, areas[0].top, divPos, false);
                    child.Set(divPos, areas[0].right, areas[0].bottom, areas[0].top, divPos, false);
                    areas.Remove(areas[0]);
                    if (parent.width * parent.height > child.width * child.height)
                    {
                        areas.Add(parent);
                        areas.Add(child);
                        DivArear(areas);
                    }
                    else
                    {
                        areas.Add(child);
                        areas.Add(parent);
                        DivArear(areas);
                    }
                }
                else if (divXY == 1)
                {
                    int divPos = random.Next(areas[0].bottom + divMinSize, areas[0].top - divMinSize);

                    Area parent = new Area();
                    Area child = new Area();
                    parent.Set(areas[0].left, areas[0].right, divPos, areas[0].top, divPos, true);
                    child.Set(areas[0].left, areas[0].right, areas[0].bottom, divPos, divPos, true);
                    areas.Remove(areas[0]);
                    if (parent.width * parent.height > child.width * child.height)
                    {
                        areas.Add(parent);
                        areas.Add(child);
                        DivArear(areas);
                    }
                    else
                    {
                        areas.Add(child);
                        areas.Add(parent);
                        DivArear(areas);
                    }
                }
            }
            else if (areas[0].right - areas[0].left - divMinSize < divMinSize || areas[0].right - divMinSize < areas[0].left)
            {
                return;
            }
        }
    }

    [SerializeField] int roomMinSize = 3;//部屋の最小サイズ3*3

    //部屋の作成
    Area Rooms(Area area)//分割元を入れる
    {
        Area room = new Area();
        //もし　部屋のサイズがなかったらそのまま返す
        if (area.width < roomMinSize || area.height < roomMinSize)
        {
            return room;
        }
        int left, bottom;
        int right, top;
        int width, height;

        left = random.Next(area.left + 1, area.right - roomMinSize);//区画から左の位置をランダムで決める
        bottom = random.Next(area.bottom + 1, area.top - roomMinSize);//区画から下の位置をランダムで決める
        width = random.Next(roomMinSize, area.right - left);//横幅計算
        height = random.Next(roomMinSize, area.top - bottom);//縦幅計算

        right = left + width;//右の位置を計算
        top = bottom + height;//上の位置を計算

        room.Set(left, right, bottom, top, area.pathJointPos, area.vh);//部屋のデータをセット

        //道の開始位置を部屋内で決定
        Vector2 pathPos = new Vector2();
        pathPos.x = random.Next(room.left, room.right);
        pathPos.y = random.Next(room.bottom, room.top);
        room.pathPoint = pathPos;
        return room;

    }
}