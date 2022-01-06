using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;
    

    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX; //横方向速度
    public float speedJump;
    public float accelerationZ; //前進速度

    public int Life(){return life;}//ライフ取得
    bool IsStun(){return recoverTime > 0.0f || life <= 0;}//気絶判定

    void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //デバッグ用キー入力
        if(Input.GetKeyDown("left")) MoveToLeft();
        if(Input.GetKeyDown("right")) MoveToRight();
        if(Input.GetKeyDown("space")) Jump();

        if(IsStun()) //気絶していれば
        {
            //動きを止めてカウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        }
        else
        {

            //z軸方向は常に前進して常に加速
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            //x軸方向は目標のポジションまでの差分の割合
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;

        }

        // if(controller.isGrounded) //接地判定
        // {
        //     if(Input.GetAxis("Vertical") > 0.0f) //縦方向入力チェック。0以下を許容すると後ろを押したときにバックする
        //     {
        //         moveDirection.z = Input.GetAxis("Vertical") * speedZ;
        //     }
        //     else
        //     {
        //         moveDirection.z = 0;
        //     }

        //     transform.Rotate(0,Input.GetAxis("Horizontal") * 3,0); //横方向入力チェックでy軸を中心に回転

        //     if(Input.GetButton("Jump")) //ジャンプボタン
        //     {
        //         moveDirection.y = speedJump;
        //         animator.SetTrigger("jump");
        //     }
        // }

        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime; //Time.deltaTimeは前フレームからの経過時間

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後に設置していたらy方向の速度はリセット
        if(controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走っているフラグをtrueにする
        animator.SetBool("run",moveDirection.z > 0.0f);
    }

    public void MoveToLeft() //左に移動開始
    {
        if(IsStun()) return; //気絶していたらキャンセル
        if(controller.isGrounded && targetLane > MinLane) targetLane--;
    }
    public void MoveToRight() //右に移動開始
    {
        if(IsStun()) return; //気絶していたらキャンセル
        if(controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump() //ジャンプ開始
    {
        if(IsStun()) return; //気絶していたらキャンセル
        if(controller.isGrounded)
        {
            moveDirection.y = speedJump;
            //ジャンプトリガー
            animator.SetTrigger("jump");
        }
    }

    //CharacterControllerに衝突判定が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(IsStun()) return; //気絶中なら無視
        if(hit.gameObject.tag =="Robo")
        {
            //ライフを減らして気絶状態に移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガー
            animator.SetTrigger("damage");

            //ヒットしたら削除
            Destroy(hit.gameObject);
        }
    }
}
