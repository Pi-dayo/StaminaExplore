using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;//移動スピード
    [SerializeField] LayerMask solidObject;//破壊可能レイヤー
    [SerializeField] DungeonGenerator generator;//スタート位置を決める用
    [SerializeField] Tilemap clearTile;//クリアmap
    [SerializeField] Clear clear;//クリアしたかどうか用
    [SerializeField] TileBase cursor;//カーソルの画像
    [SerializeField] AttackCollision attackCollision;//攻撃時間判定用
    [SerializeField] float canHitTime;//攻撃開始からの当たり判定の時間
    [SerializeField] float attackCoolTime;//攻撃のクールタイム

    public Tilemap tile;//タイル設置用タイルマップ
    public Tilemap cursorTile;//カーソル用タイル

    bool canHit = false;//攻撃が当たるかどうか
    bool isMoving = false;//移動中判定
    bool isWatch = false;//アクション可能判定
    bool isAttack;//攻撃のクールタイム中かどうか

    int havingtoolNum;//現在持っているツールの種類

    float canHitTimer;//攻撃時間用のタイマー
    float attackCoolTimeTimer;//攻撃クールタイム用タイマー

    Vector2 input;//移動入力
    Vector3 watchingPoint;//プレイヤーがどこを向いているか　見れる範囲１マス
    Vector3 mousePos;//マウスの位置
    Vector3 direction;//プレイヤーの向き
    Vector3Int selectTilePos;//アクション可能な範囲で選択されているタイルの位置
    Vector3Int selectTilePosCopy;//破壊後にカーソルを消す用のコピー

    private string[] solidLayerName = { "solidObject", "wall", "clear" };//当たり判定レイヤー
    private string[] canSelectLayerName = { "solidObject", "clear" };//カーソルが出るレイヤー

    private LayerMask solidLayers;//破壊可能レイヤー
    private LayerMask canSelectLayers;//カーソルが出るレイヤー

    public bool IsAttack { get => isAttack; }
    public bool CanHit { get => canHit; }
    public int HavingtoolNum { get => havingtoolNum;}
    [SerializeField]Animator animator;

    private void Start()
    {
        attackCoolTimeTimer = 0;//初期化
        havingtoolNum = 0;//初期のツールをつるはしに
        solidLayers = LayerMask.GetMask(solidLayerName);//初期化
        canSelectLayers = LayerMask.GetMask(canSelectLayerName);//初期化
        transform.position = new Vector3(generator.StartPos.x + 0.5f, generator.StartPos.y + 0.5f, 0);//スタート位置のセット
    }
    void Update()
    {

        if (!clear.IsClear)//クリア条件を満たしてない場合のみ動ける
        {
            havingtoolNum += (int)Input.mouseScrollDelta.y;//マウスホイールで現在のツールを変更
            if (havingtoolNum < 0)
            {
                havingtoolNum = 1;
            }
            if (havingtoolNum > 1)
            {
                havingtoolNum = 0;
            }
            ActionRange();//アクション
            MoveControll();//移動
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
        if (!isMoving)//移動中は入力を受け付けない
            {
                Boolean diagonalflg = false;
                Vector2 basePos = input;

                input.x = Input.GetAxisRaw("Horizontal")/2;//横方向入力
                input.y = Input.GetAxisRaw("Vertical")/2;//縦方向入力
                if (input != Vector2.zero)//入力があったら移動
                {
                    Vector2 targetPos = transform.position;//移動先の初期化
                    Vector2 diagX = targetPos;
                    Vector2 diagY = targetPos;
                    targetPos += input;//移動先

                    //x,y　両方に値がある場合は斜めとして判断
                    if (input.x != 0 && input.y != 0)
                    {
                        diagonalflg = true; //斜めフラグ
                        diagX.x += input.x; //斜め移動の通過点(X)を設定
                        diagY.y += input.y; //斜め移動の通過点(Y)を設定
                    }

                    //移動先に障害物がなかったら
                    //斜め移動の場合、通過点の障害物判定も合わせて行う
                    if ((diagonalflg && (IsWalkable(diagX) && IsWalkable(diagY)) && IsWalkable(targetPos)) || (!diagonalflg && IsWalkable(targetPos)))
                    {
                        StartCoroutine(Move(targetPos));//移動
                    }
                }
            }
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;//移動中にして入力を受けないように
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //１マスを徐々に移動する処理
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;//プレイヤーの位置を調整
        isMoving = false;//移動中判定をオフ
    }
    bool IsWalkable(Vector2 targetpos)
    {
        return !Physics2D.OverlapCircle(targetpos, 0.2f, solidLayers);//移動先に障害物があるかどうか
    }
    bool Watching(Vector2 dir)//マウスカーソルの方向に何があるかの判定
    {
        if (Physics2D.Raycast(transform.position, dir, 1, canSelectLayers))//プレイヤーから1マス先にRayを飛ばして判定
        {
            return true;//アクション可能の物があったらtreu;
        }
        else
        {
            return false;
        }
    }

    void ActionRange()//採掘可能かどうか
    {
        mousePos = Input.mousePosition;//マウスの位置
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);//マウスの座標をプレイ画面の座標に変換
        direction = (mousePos - transform.position).normalized;//マウスとプレイヤーの位置で方向を出して正規化
        isWatch = Watching(direction);//判定

        if (isWatch)//採掘可能なものがあったら
        {
            watchingPoint = Physics2D.Raycast(transform.position, direction, 1, canSelectLayers).point;//座標を出す
            selectTilePos = tile.WorldToCell(watchingPoint + direction);//TileMap座標に変換と調整
            cursorTile.SetTile(selectTilePosCopy, null);//採掘可能な場所に出す
            cursorTile.SetTile(selectTilePos, cursor);//過去のカーソルを消す
            PickAxe();//採掘
            selectTilePosCopy = selectTilePos;//カーソルの位置のコピー　前回の選択カーソルを消すため
        }
    }
    //つるはし
    void PickAxe()
    {
        if (Input.GetMouseButton(0))//マウスの左クリック　押しっぱなしも可
        {
            if (havingtoolNum == 0)
            {
                tile.SetTile(selectTilePos, null);//選択場所のタイルを消す
                clearTile.SetTile(selectTilePos, null);//選択場所のクリアタイルを消す
                cursorTile.SetTile(selectTilePos, null);//壊した場所のカーソルを消す 
            }
        }
    }
    //攻撃
    void Attack()
    {
        if (Input.GetMouseButton(0))//マウスの左クリック　押しっぱなしも可
        {
            if (havingtoolNum == 1)//攻撃ツールを持っているかどうか
            {
                if (!isAttack)//攻撃中じゃなければ
                {
                    isAttack = true;//攻撃中にする
                    canHit = true;//攻撃があたる時間にする
                }
            }
        }
        if (isAttack)//攻撃中なら
        {
            canHitTimer += Time.deltaTime;//攻撃判定を出す時間
            attackCoolTimeTimer += Time.deltaTime;//攻撃のクールタイムを加算
            if (canHitTimer > canHitTime)//攻撃判定を出す時間を超えたら
            {
                canHit = false;//攻撃判定をoff
                canHitTimer = 0;//攻撃可能タイマーリセット
            }
            if (attackCoolTimeTimer > attackCoolTime)//攻撃クールタイムが過ぎたら
            {
                isAttack = false;//攻撃中を解除
                attackCollision.IsHit = false;//攻撃多重判定用のフラグ解除　（できてない）
                attackCoolTimeTimer = 0;//攻撃クールタイムタイマーリセット
            }
        }
        attackCollision.HitTime(canHit);//攻撃判定を出す
    }
}