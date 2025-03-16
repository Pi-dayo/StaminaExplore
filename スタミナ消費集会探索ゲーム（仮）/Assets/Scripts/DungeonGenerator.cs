using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tile;//�ݒu����^�C���}�b�v
    [SerializeField] Tilemap clearTile;//�N���A�^�C���p�^�C���}�b�v

    [SerializeField] List<TileBase> tileBase;//�^�C�����X�g

    [SerializeField] TileBase clearTileBase;//�N���A�^�C��

    [SerializeField] List<TilesInfomation> wallTileInfo;//������
    [SerializeField] List<TilesInfomation> itemTiletileInfo;//������
    [SerializeField] List<TilesInfomation> derivationTileInfo;//������
    [SerializeField] List<TilesInfomation> obstacleTileInfo;//������

    [SerializeField] int width, height;//�_���W�����S�̂̉��c
    [SerializeField] int decorationMinNum;//�����̍ŏ��T�C�Y
    [SerializeField] int decorationMaxNum;//�����̍ő�T�C�Y
    [SerializeField] int itemTileRato;//������
    [SerializeField] int obstacleTileRato;//������
    [SerializeField] int derivationTileRato;//������

    Vector3Int startPos;//�v���C���[�̃X�^�[�g�ʒu
    Vector3Int clearPos;//�N���A�^�C���̈ʒu
    Vector2 decorationPos;//�����̈ʒu

    int startRandom;//�X�^�[�g�ʒu�������_���ȕ����ɂ���
    int clearRandom;//�N���A�ʒu�������_���ȕ����ɂ���
    int areasNum;//�X�^�[�g�ƃN���A�̔��ʗp
    int tileRandom;//�����_���ȃ^�C����I��
    int decorationSize;//�����̃T�C�Y
    int decorationNum;//�����̐�


    int tileRato;//�z�u����^�C���̊���
    public Vector3Int StartPos { get => startPos; }
    public Vector3Int ClearPos { get => clearPos; }
    static System.Random random = new System.Random((int)DateTime.Now.Ticks);

    //�G���A�����p
    public class Area
    {
        public int width, height;//�����A����
        public int left, right, bottom, top;//���A�E�A���A��̈ʒu
        public int pathJointPos;//�G���A�����̏ꏊ
        public Vector2 pathPoint;//���̊J�n�n�_
        public bool vh = false;//�G���A���c�����ɕ������邩

        //�����Ȃ�
        public Area()
        {

        }
        //�Z�b�g
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
    Area dgArea;//�_���W�����̃G���A
    List<Area> areas = new List<Area>();//���������G���A������悤�̃��X�g
    List<Area> pathPoints = new List<Area>();//�����Ȃ��郊�X�g
    void Awake()
    {
        dgArea = new Area();//������
        dgArea.Set(0, width, 0, height, 0, false);//�_���W�����̑S�̂̃Z�b�g
        areas.Add(dgArea);//�����p���̏����l���Z�b�g
        DivArear(areas);//�G���A�̕���
        areasNum = 0;//������
        startRandom = random.Next(0, areas.Count);//�����l�̐ݒ�
        clearRandom = random.Next(0, areas.Count);//�����l�̐ݒ�

        //�_���W������S�����߂� �i�^�C���̊����Łj
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                tileRato=random.Next(0, 100);
                if (tileRato < 5)//�S�̂�5%�ΒY
                {
                    tile.SetTile(new Vector3Int(i, j, 0), itemTiletileInfo[0].tile);
                }
                else if(tileRato < 15 && tileRato > 5)//�S�̂�10%�K�`���z��
                {
                    tileRato = random.Next(0, 100);
                    if (tileRato < 30)//30%1�i�K�ڔ��W
                    {
                        tile.SetTile(new Vector3Int(i, j, 0), derivationTileInfo[1].tile);
                    }
                    else//70%0�i�K�ڔ��W
                    {
                        tile.SetTile(new Vector3Int(i, j, 0), derivationTileInfo[0].tile);
                    }
                }
                else//80%�����̏�Q��
                {
                    tile.SetTile(new Vector3Int(i, j, 0), obstacleTileInfo[0].tile);
                }
            }
        }
        decorationNum = random.Next(decorationMinNum, decorationMaxNum);//���A�̐�
        for(int i = 0; i < decorationNum; i++)
        {
            decorationPos = new Vector2(random.Next(0,width),random.Next(0,height));//���A�̈ʒu������
            decorationSize = random.Next(1, 15);//���A�̃T�C�Y������
            //���A�������_���E�H�[�N�Ŋg��
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
        //���ݒu
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

        //������ݒu
        foreach (Area area in areas)
        {
            Area room = new Area();//�����p�ϐ�
            room = Rooms(area);//�����̏�����
            pathPoints.Add(room);//�����̓��̏ꏊ�����X�g�ɓ����

            //�����̍쐬
            for (int i = room.left; i <= room.right; i++)
            {
                for (int j = room.bottom; j <= room.top; j++)
                {
                    tile.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
            //�X�^�[�g�̕�����������
            if (areasNum == startRandom)
            {
                //�������ŃX�^�[�g�ʒu�����߂�
                int posX = random.Next(room.left, room.right);
                int posY = random.Next(room.bottom, room.top);
                startPos = new Vector3Int(posX, posY);

            }
            //�N���A�̕�����������
            if (areasNum == clearRandom)
            {
                //�������ŃN���A�ʒu�����߂�
                clearPos = new Vector3Int();
                int posX = random.Next(room.left, room.right);
                int posY = random.Next(room.bottom, room.top);
                clearPos = new Vector3Int(posX, posY);
            }
            areasNum++;//�X�^�[�g�ƃN���A�̕����̊m�F�p
        }
        clearTile.SetTile(new Vector3Int((int)clearPos.x, (int)clearPos.y, 0), clearTileBase);//�N���A�^�C���̐ݒu


        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            int start, end;
            //�����̋�؂�����c������
            if (!pathPoints[i].vh)
            {
                //���������؂�ʒu�܂œ�������
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.x);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i].pathPoint.x);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(j, (int)pathPoints[i].pathPoint.y, 0), null);
                }
                //���̕�������O�̋�؂�ʒu�܂œ�������
                start = Math.Min(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.x);
                end = Math.Max(pathPoints[i].pathJointPos, (int)pathPoints[i + 1].pathPoint.x);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(j, (int)pathPoints[i + 1].pathPoint.y, 0), null);
                }
                //�����m���Ȃ���
                start = Math.Min((int)pathPoints[i].pathPoint.y, (int)pathPoints[i + 1].pathPoint.y);
                end = Math.Max((int)pathPoints[i].pathPoint.y, (int)pathPoints[i + 1].pathPoint.y);
                for (int j = start; j <= end; j++)
                {
                    tile.SetTile(new Vector3Int(pathPoints[i].pathJointPos, j, 0), null);
                }
            }
            //��Ɠ��l
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

    [SerializeField] int divMinSize = 5;//�����ŏ��T�C�Y�@5*5
    //��敪��
    public void DivArear(List<Area> areas)
    {
        //�������ė��������ŏ��T�C�Y��菬���������烊�^�[��
        if (areas[0].right - divMinSize < divMinSize && areas[0].top - divMinSize < divMinSize)
        {
            return;
        }
        //���ɕ����ł���T�C�Y�Ȃ�
        if (areas[0].width - divMinSize > divMinSize && areas[0].right - divMinSize > areas[0].left)
        {
            //�c�ł������ł��邩�m�F�@�ł���Ȃ�c���Œ��I�@�ł��Ȃ��Ȃ牡�ɕ���
            if (areas[0].height - divMinSize > divMinSize && areas[0].top - divMinSize > areas[0].bottom)
            {
                int divXY = random.Next(0, 2);//�c�Ɋ��邩���Ɋ��邩
                if (divXY == 0)
                {
                    int divPos = random.Next(areas[0].left + divMinSize, areas[0].right - divMinSize);//�����ʒu�̌���
                    //�Q�̕����ɕ���
                    Area parent = new Area();
                    Area child = new Area();

                    parent.Set(areas[0].left, divPos, areas[0].bottom, areas[0].top, divPos, false);
                    child.Set(divPos, areas[0].right, areas[0].bottom, areas[0].top, divPos, false);
                    //���������폜
                    areas.Remove(areas[0]);

                    //�傫���ق��𕪊����Ƃɐݒ肵�ă��[�v
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
            //�c���Ŋ���Ȃ��Ȃ烋�[�v����
            else if (areas[0].top - areas[0].bottom - divMinSize < divMinSize || areas[0].top - divMinSize < areas[0].bottom)
            {
                return;
            }
        }
        //��Ɠ���
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

    [SerializeField] int roomMinSize = 3;//�����̍ŏ��T�C�Y3*3

    //�����̍쐬
    Area Rooms(Area area)//������������
    {
        Area room = new Area();
        //�����@�����̃T�C�Y���Ȃ������炻�̂܂ܕԂ�
        if (area.width < roomMinSize || area.height < roomMinSize)
        {
            return room;
        }
        int left, bottom;
        int right, top;
        int width, height;

        left = random.Next(area.left + 1, area.right - roomMinSize);//��悩�獶�̈ʒu�������_���Ō��߂�
        bottom = random.Next(area.bottom + 1, area.top - roomMinSize);//��悩�牺�̈ʒu�������_���Ō��߂�
        width = random.Next(roomMinSize, area.right - left);//�����v�Z
        height = random.Next(roomMinSize, area.top - bottom);//�c���v�Z

        right = left + width;//�E�̈ʒu���v�Z
        top = bottom + height;//��̈ʒu���v�Z

        room.Set(left, right, bottom, top, area.pathJointPos, area.vh);//�����̃f�[�^���Z�b�g

        //���̊J�n�ʒu�𕔉����Ō���
        Vector2 pathPos = new Vector2();
        pathPos.x = random.Next(room.left, room.right);
        pathPos.y = random.Next(room.bottom, room.top);
        room.pathPoint = pathPos;
        return room;

    }
}