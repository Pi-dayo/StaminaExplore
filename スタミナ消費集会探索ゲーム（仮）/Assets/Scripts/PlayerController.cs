using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;//�ړ��X�s�[�h
    [SerializeField] LayerMask solidObject;//�j��\���C���[
    [SerializeField] DungeonGenerator generator;//�X�^�[�g�ʒu�����߂�p
    [SerializeField] Tilemap clearTile;//�N���Amap
    [SerializeField] Clear clear;//�N���A�������ǂ����p
    [SerializeField] TileBase cursor;//�J�[�\���̉摜
    [SerializeField] AttackCollision attackCollision;//�U�����Ԕ���p
    [SerializeField] float canHitTime;//�U���J�n����̓����蔻��̎���
    [SerializeField] float attackCoolTime;//�U���̃N�[���^�C��

    public Tilemap tile;//�^�C���ݒu�p�^�C���}�b�v
    public Tilemap cursorTile;//�J�[�\���p�^�C��

    bool canHit = false;//�U���������邩�ǂ���
    bool isMoving = false;//�ړ�������
    bool isWatch = false;//�A�N�V�����\����
    bool isAttack;//�U���̃N�[���^�C�������ǂ���

    int havingtoolNum;//���ݎ����Ă���c�[���̎��

    float canHitTimer;//�U�����ԗp�̃^�C�}�[
    float attackCoolTimeTimer;//�U���N�[���^�C���p�^�C�}�[

    Vector2 input;//�ړ�����
    Vector3 watchingPoint;//�v���C���[���ǂ��������Ă��邩�@�����͈͂P�}�X
    Vector3 mousePos;//�}�E�X�̈ʒu
    Vector3 direction;//�v���C���[�̌���
    Vector3Int selectTilePos;//�A�N�V�����\�Ȕ͈͂őI������Ă���^�C���̈ʒu
    Vector3Int selectTilePosCopy;//�j���ɃJ�[�\���������p�̃R�s�[

    private string[] solidLayerName = { "solidObject", "wall", "clear" };//�����蔻�背�C���[
    private string[] canSelectLayerName = { "solidObject", "clear" };//�J�[�\�����o�郌�C���[

    private LayerMask solidLayers;//�j��\���C���[
    private LayerMask canSelectLayers;//�J�[�\�����o�郌�C���[

    public bool IsAttack { get => isAttack; }
    public bool CanHit { get => canHit; }
    public int HavingtoolNum { get => havingtoolNum;}
    [SerializeField]Animator animator;

    private void Start()
    {
        attackCoolTimeTimer = 0;//������
        havingtoolNum = 0;//�����̃c�[������͂���
        solidLayers = LayerMask.GetMask(solidLayerName);//������
        canSelectLayers = LayerMask.GetMask(canSelectLayerName);//������
        transform.position = new Vector3(generator.StartPos.x + 0.5f, generator.StartPos.y + 0.5f, 0);//�X�^�[�g�ʒu�̃Z�b�g
    }
    void Update()
    {

        if (!clear.IsClear)//�N���A�����𖞂����ĂȂ��ꍇ�̂ݓ�����
        {
            havingtoolNum += (int)Input.mouseScrollDelta.y;//�}�E�X�z�C�[���Ō��݂̃c�[����ύX
            if (havingtoolNum < 0)
            {
                havingtoolNum = 1;
            }
            if (havingtoolNum > 1)
            {
                havingtoolNum = 0;
            }
            ActionRange();//�A�N�V����
            MoveControll();//�ړ�
            Attack();
        }

    }
    void MoveControll()
    {
        if (input != Vector2.zero)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
        if (!isMoving)//�ړ����͓��͂��󂯕t���Ȃ�
            {
                Boolean diagonalflg = false;
                Vector2 basePos = input;

                input.x = Input.GetAxisRaw("Horizontal")/2;//����������
                input.y = Input.GetAxisRaw("Vertical")/2;//�c��������
                if (input != Vector2.zero)//���͂���������ړ�
                {
                    Vector2 targetPos = transform.position;//�ړ���̏�����
                    Vector2 diagX = targetPos;
                    Vector2 diagY = targetPos;
                    targetPos += input;//�ړ���

                    //x,y�@�����ɒl������ꍇ�͎΂߂Ƃ��Ĕ��f
                    if (input.x != 0 && input.y != 0)
                    {
                        diagonalflg = true; //�΂߃t���O
                        diagX.x += input.x; //�΂߈ړ��̒ʉߓ_(X)��ݒ�
                        diagY.y += input.y; //�΂߈ړ��̒ʉߓ_(Y)��ݒ�
                    }

                    //�ړ���ɏ�Q�����Ȃ�������
                    //�΂߈ړ��̏ꍇ�A�ʉߓ_�̏�Q����������킹�čs��
                    if ((diagonalflg && (IsWalkable(diagX) && IsWalkable(diagY)) && IsWalkable(targetPos)) || (!diagonalflg && IsWalkable(targetPos)))
                    {
                        StartCoroutine(Move(targetPos));//�ړ�
                    }
                }
            }
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;//�ړ����ɂ��ē��͂��󂯂Ȃ��悤��
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //�P�}�X�����X�Ɉړ����鏈��
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;//�v���C���[�̈ʒu�𒲐�
        isMoving = false;//�ړ���������I�t
    }
    bool IsWalkable(Vector2 targetpos)
    {
        return !Physics2D.OverlapCircle(targetpos, 0.2f, solidLayers);//�ړ���ɏ�Q�������邩�ǂ���
    }
    bool Watching(Vector2 dir)//�}�E�X�J�[�\���̕����ɉ������邩�̔���
    {
        if (Physics2D.Raycast(transform.position, dir, 1, canSelectLayers))//�v���C���[����1�}�X���Ray���΂��Ĕ���
        {
            return true;//�A�N�V�����\�̕�����������treu;
        }
        else
        {
            return false;
        }
    }

    void ActionRange()//�̌@�\���ǂ���
    {
        mousePos = Input.mousePosition;//�}�E�X�̈ʒu
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);//�}�E�X�̍��W���v���C��ʂ̍��W�ɕϊ�
        direction = (mousePos - transform.position).normalized;//�}�E�X�ƃv���C���[�̈ʒu�ŕ������o���Đ��K��
        isWatch = Watching(direction);//����

        if (isWatch)//�̌@�\�Ȃ��̂���������
        {
            watchingPoint = Physics2D.Raycast(transform.position, direction, 1, canSelectLayers).point;//���W���o��
            selectTilePos = tile.WorldToCell(watchingPoint + direction);//TileMap���W�ɕϊ��ƒ���
            cursorTile.SetTile(selectTilePosCopy, null);//�̌@�\�ȏꏊ�ɏo��
            cursorTile.SetTile(selectTilePos, cursor);//�ߋ��̃J�[�\��������
            PickAxe();//�̌@
            selectTilePosCopy = selectTilePos;//�J�[�\���̈ʒu�̃R�s�[�@�O��̑I���J�[�\������������
        }
    }
    //��͂�
    void PickAxe()
    {
        if (Input.GetMouseButton(0))//�}�E�X�̍��N���b�N�@�������ςȂ�����
        {
            if (havingtoolNum == 0)
            {
                tile.SetTile(selectTilePos, null);//�I���ꏊ�̃^�C��������
                clearTile.SetTile(selectTilePos, null);//�I���ꏊ�̃N���A�^�C��������
                cursorTile.SetTile(selectTilePos, null);//�󂵂��ꏊ�̃J�[�\�������� 
            }
        }
    }
    //�U��
    void Attack()
    {
        if (Input.GetMouseButton(0))//�}�E�X�̍��N���b�N�@�������ςȂ�����
        {
            if (havingtoolNum == 1)//�U���c�[���������Ă��邩�ǂ���
            {
                if (!isAttack)//�U��������Ȃ����
                {
                    isAttack = true;//�U�����ɂ���
                    canHit = true;//�U���������鎞�Ԃɂ���
                }
            }
        }
        if (isAttack)//�U�����Ȃ�
        {
            canHitTimer += Time.deltaTime;//�U��������o������
            attackCoolTimeTimer += Time.deltaTime;//�U���̃N�[���^�C�������Z
            if (canHitTimer > canHitTime)//�U��������o�����Ԃ𒴂�����
            {
                canHit = false;//�U�������off
                canHitTimer = 0;//�U���\�^�C�}�[���Z�b�g
            }
            if (attackCoolTimeTimer > attackCoolTime)//�U���N�[���^�C�����߂�����
            {
                isAttack = false;//�U����������
                attackCollision.IsHit = false;//�U�����d����p�̃t���O�����@�i�ł��ĂȂ��j
                attackCoolTimeTimer = 0;//�U���N�[���^�C���^�C�}�[���Z�b�g
            }
        }
        attackCollision.HitTime(canHit);//�U��������o��
    }
}