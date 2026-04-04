using UnityEngine;
using Perk.Model;   //パークの参照
using Perk.Data;
using Player.View;
using System;
using Scene.View;
using Scene.Controller;    //パークのイベント

public class PlayerController : MonoBehaviour
{
    private PlayerMove psm;

    //直前の状態
    private PlayerState lastState;
    [SerializeField] private PlayerView playerView;

    //プレイヤーの移動速度
    [Header("移動設定")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float ladderSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("判定設定")]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private float groundCheckOffset = 0.6f;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 2f);
    [SerializeField] private float wallCheckOffset = 0.5f;
    [SerializeField] private LayerMask groundLayer; // 地面判定用のレイヤー
    [SerializeField] private LayerMask wallLayer; // 壁判定用のレイヤー
   
    [Header("演出設定")]
    [SerializeField] private SpriteRenderer spriteRenderer; // プレイヤーの画像
    [SerializeField] private GameObject shieldObject; // シールドの画像オブジェクト

    private bool lastGrounded; // 前フレームの接地状態を保存する変数
    
    //現在無敵か（初期は無敵ではない）
    private bool isInvincible = false;
    private bool isInvincibleLast = false;
    
    // プレイヤーの残機などのステータスを管理するクラス
    private PlayerStatus status; 
    [Header("ステータス設定")]
    [SerializeField] private int initialLives = 3;
    [SerializeField] private Vector2 respawnPosition; // ミスした時の復活地点

    [Header("パーク詳細設定")]
    // 0.0（無重力）〜 1.0（通常重力）の間で調整
    [Range(0f, 1f)] 
    [SerializeField] private float glideGravityMultiplier = 0.2f;

    [Header("壁キックの設定")]
    [SerializeField] private Vector2 wallKickForce = new Vector2(10f, 12f); // 壁キックの力の大きさ
    [SerializeField] private float wallKickLockDuration = 0.2f; // 壁キックの操作不能期間
    
    [Header("デバッグ用")]
    [SerializeField, ReadOnly] private bool debugIsGrounded; // 地面に接地しているかどうかのフラグ（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private bool debugIsWalled; // 壁に接触しているかどうかのフラグ（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private bool debugIsNearWall; // 壁に近づいているかどうかのフラグ（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private bool debugIsClimbing; // 壁つかまり状態かどうかのフラグ（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private int debugRemainJumpCount; // 残りのジャンプ回数（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private PlayerState debugCurrentState; // 現在の状態（インスペクターで確認できるようにする）
    [SerializeField, ReadOnly] private int debugCurrentLives; // 現在の残機（インスペクターで確認できるようにする）
    
    //入力などの内部処理に使う変数
    private float moveInput;
    private bool jumpDown;
    private bool jumpHold;
    private bool dash;
    private float verticalInput;
    private bool canLadderClimb; //はしごに登れるかどうかのフラグ
    private float lockTimer; //実際に操作不能時間を操作するタイマーの変数(初期値は0)
    
    // パーク用の変数
    private int remainJumpCount; //残りのジャンプ回数
    private float defaultGravityScale; //デフォルトの重力の大きさ

    private Rigidbody2D rb;
   
    private void Awake()
    {
        // プレイヤーの状態管理クラスのインスタンス化
        psm = new PlayerMove();
        // 物理移動コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        //spriteRendererの取得（インスペクターで設定していない場合は同じオブジェクトから探す）
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        //それでも見つからなかったら警告を出す
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRendererが付いていないので点滅エフェクトは再生されません");
        }

        // プレイヤーのステータス管理クラスのインスタンス化
        status = new PlayerStatus(initialLives);

        // 残機が変化したときのイベントに、UI更新などの処理を登録する
        status.OnGameOver += HandleGameOver;

        // インスペクターで設定した重力を保存しておく
        defaultGravityScale = rb.gravityScale;
    }

    private void Start()
    {
        // 初期位置をrespawnPositionに設定
        respawnPosition = transform.position;
        playerView.PlayAnimation("Idle");    

        // パークの初期化
        if (status != null)
        {
            status.InitializeStatus();
        }

        GameView.OnInitialized?.Invoke(status.MaxLives, status.CurrentLives); // ゲーム全体の初期化が完了したことを通知
    }

    // Update is called once per frame
    private void Update()
    {   
        if (psm == null)
        {
            psm = new PlayerMove(); // 念のためPlayerMoveのインスタンスを作成
        }
        
        //パーク情報の取得
        var perk = PerkEffectReference.Instance;
        if (perk == null)
        {
            Debug.LogError("PerkEffectReferenceのインスタンスが見つかりません");
            return;
        }

        // 毎フレームの通知
        PerkEvents.CheckLife?.Invoke(status.CurrentLives);
        PerkEvents.CheckPosition?.Invoke(transform.position);
        PerkEvents.Update?.Invoke();

        //入力の取得をする関数を空の状態で定義
        moveInput = 0;
        verticalInput = 0;
        jumpDown = false;
        jumpHold = false;
        dash = false;

        // 操作不能時間がないときは入力を受け付ける
        if(lockTimer <= 0)
        {
            // 入力の取得
            moveInput = Input.GetAxisRaw("Horizontal"); 
            // GetKeyDown：押した瞬間（ジャンプ判定用）
            jumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow); // ジャンプボタンを押した瞬間
            // GetKey：押しっぱなし（滞空/グライド判定用）
            jumpHold = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow); // ジャンプボタンを押し続けているか
            dash = Input.GetKey(KeyCode.LeftShift); 
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            // 操作不能時間がある場合は入力を無効にしてカウントダウンする
            lockTimer -= Time.deltaTime;
        }
        
        //キャラクターの向きをきめる
        if (lockTimer <= 0 && spriteRenderer != null)
        {
            if (moveInput > 0)
            {
                spriteRenderer.flipX = false; // 右を向く
            }
            else if (moveInput < 0)
            {
                spriteRenderer.flipX = true; // 左を向く
            }
        }

        //シールドのパークがあるときはシールドオブジェクトを表示
        if (shieldObject != null)
        {
            //シールドの表示非表示を切り替える
            shieldObject.SetActive(perk.HasShield);
        }

        //無敵時間になったら無敵効果開始
        if(perk.IsInvincible && !isInvincible)
        {
            StartCoroutine(InvincibleCount());
        }
        
        //無敵時間のカウントダウン
        if (perk.InvincibleSeconds > 0)
        {
            // 無敵時間を減らす
            perk.InvincibleSeconds -= Time.deltaTime;
            // 無敵時間が0秒を下回らないようにする
            if (perk.InvincibleSeconds < 0)
            {
                perk.InvincibleSeconds = 0;
            }
        }
        
        // 地面に接地しているかの判定 
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        bool isGrounded = Physics2D.OverlapBox(groundCheckPos, groundCheckSize, 0f, groundLayer);

        // デバッグ用：ジャンプ入力と接地状態をログに表示
        if (jumpDown)
        {
            if(remainJumpCount > 0) SoundSourceObject.Instance.PlayJumpSE();
            Debug.Log($"ジャンプボタンを押した (接地: {isGrounded}) 残りジャンプ回数: {remainJumpCount}");
        }

        //着地イベントの通知
        if (isGrounded && !lastGrounded) // lastGroundedという変数を作って前回と比較
        {
            PerkEvents.Land?.Invoke();
            SoundSourceObject.Instance.PlayLandSE();
        }
        lastGrounded = isGrounded;
        
        // 壁に接触しているかの判定
        bool isWalled = CheckWall(moveInput);

        //壁キック用の判定
        bool isNearWall = CheckWall(1f) || CheckWall(-1f);

        //状態更新用のフラグを用意(今、壁つかまり中か滞空中かを計算して渡す）
        bool isClimbing = isWalled && perk.CanClimb && !isGrounded && rb.linearVelocity.y <= 0;
        bool isGliding = !isGrounded && jumpHold && perk.CanGlide && rb.linearVelocity.y < 0;

        // 壁がある方向に進もうとしているなら、入力を 0 にして侵入を防ぐ（補助的処理）
        //梯子に登れるときは壁判定があっても入力を消さないようにする
        float effectiveInput = (isWalled && !canLadderClimb) ? 0 : moveInput;

        //空中ジャンプ回数のリセット
        if (isGrounded)
        {
            remainJumpCount = 1 + perk.AdditionalJumpCount; //地面にいるときは追加ジャンプ回数+1回ジャンプできる
        }
        
        psm.UpdateState(effectiveInput, rb.linearVelocity.y, verticalInput, jumpDown, dash, isGrounded, isClimbing, isGliding, canLadderClimb);
        
        //実際の移動処理に引数を追加してパーク対応
        //滞空判定に使うのでjumpHoldを渡す
        ApplyMovement(effectiveInput, isWalled, isGrounded, jumpHold, perk, canLadderClimb);
        //ジャンプ処理を空中ジャンプ対応の「HandleJump」に変更
        //ジャンプの実行なのでjumpDownを使う
        if (jumpDown)
        {
            HandleJump(isGrounded, isNearWall, isWalled, perk);
        }

        //デバッグ用の変数に値を入れる
        debugIsGrounded = isGrounded;
        debugIsWalled = isWalled;
        debugIsNearWall = isNearWall;
        debugIsClimbing = isClimbing;
        debugRemainJumpCount = remainJumpCount;
        debugCurrentState = psm.CurrentState;
        debugCurrentLives = status.CurrentLives;
        
        //アニメーションの更新
        UpdateAnimation();
    }

    //アニメーションの更新を行うメソッド
    private void UpdateAnimation()
    {
        if (psm == null) return;
        if(lastState == psm.CurrentState)
        {
            if(!isInvincible && isInvincibleLast)
            {
                switch (psm.CurrentState)
                {
                    case PlayerState.Idle:
                        playerView.PlayAnimation("Idle");
                        break;
                    case PlayerState.Walk:
                        playerView.PlayAnimation("Walk");
                        break;
                    case PlayerState.Jump:
                        playerView.PlayAnimation("Jump");
                        break;
                    case PlayerState.Climb:
                        playerView.PlayAnimation("Jump");
                        break;
                    case PlayerState.Glide:
                        playerView.PlayAnimation("Jump");
                        break;
                    case PlayerState.Ladder:
                        playerView.PlayAnimation("Idle");
                        break;
                }
            }
            else if(isInvincible && !isInvincibleLast)
            {
                switch (psm.CurrentState)
                {
                    case PlayerState.Idle:
                        playerView.PlayAnimation("IdleInv");
                        break;
                    case PlayerState.Walk:
                        playerView.PlayAnimation("WalkInv");
                        break;
                    case PlayerState.Jump:
                        playerView.PlayAnimation("JumpInv");
                        break;
                    case PlayerState.Climb:
                        playerView.PlayAnimation("JumpInv");
                        break;
                    case PlayerState.Glide:
                        playerView.PlayAnimation("JumpInv");
                        break;
                    case PlayerState.Ladder:
                        playerView.PlayAnimation("IdleInv");
                        break;
                }
            }
            isInvincibleLast = isInvincible;
            return;
        }
        lastState = psm.CurrentState;
        switch (psm.CurrentState)
        {
            case PlayerState.Idle:
                if (isInvincible)
                    playerView.PlayAnimation("IdleInv");
                else
                    playerView.PlayAnimation("Idle");
                break;
            case PlayerState.Walk:
                if (isInvincible)
                    playerView.PlayAnimation("WalkInv");
                else
                    playerView.PlayAnimation("Walk");
                break;
            case PlayerState.Jump:
                if (isInvincible)
                    playerView.PlayAnimation("JumpInv");
                else
                    playerView.PlayAnimation("Jump");
                break;
            case PlayerState.Climb:
                if (isInvincible)
                    playerView.PlayAnimation("JumpInv");
                else
                    playerView.PlayAnimation("Jump");
                break;
            case PlayerState.Glide:
                if (isInvincible)
                    playerView.PlayAnimation("JumpInv");
                else
                    playerView.PlayAnimation("Jump");
                break;
            case PlayerState.Ladder:
                if (isInvincible)
                    playerView.PlayAnimation("IdleInv");
                else
                    playerView.PlayAnimation("Idle");
                break;
        }
        isInvincibleLast = isInvincible;
    }

    //ジャンプの判断
    private void HandleJump(bool isGrounded, bool isNearWall, bool isWalled,PerkEffectReference perk)
    {
        //地面にいなくて壁に触れていて、壁の方向に進む入力があるときは壁キック
        if (!isGrounded && isNearWall)
        {
            //壁のある方向に進む入力があるかどうかを判定するために、moveInputの値と壁の位置から判断する
            bool pressingTowardRightwall = (moveInput > 0 && CheckWall(1f));
            bool pressingTowardLeftwall = (moveInput < 0 && CheckWall(-1f));
            
            if(pressingTowardRightwall || pressingTowardLeftwall)
            {
                ApplyWallkick();
                //壁キック後は通常のジャンプ処理は行わない
                return;
            }
            
        }
        
        //壁つかまりのパークがあって壁についているときはジャンプできない
        if (isWalled && !isGrounded && perk.CanClimb)
        {
            return; // ジャンプさせない
        }

        //地面にいるなら跳べる
        if (isGrounded || remainJumpCount > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // ジャンプ前に一度垂直速度をリセットしてからジャンプ力を加える
            
            // ジャンプの処理（ジャンプ力の倍率も適用）
            ApplyJump(perk.JumpPowerMultiplier);

            //ジャンプイベントの通知
            PerkEvents.Jump?.Invoke();

            //空中ジャンプの回数を減らす
            if (!isGrounded)
            {
                remainJumpCount--;
            }
        }
    }
    
    private void ApplyMovement(float input, bool isWalled, bool isGrounded, bool isJumpHolding, PerkEffectReference perk, bool canLadderClimb)
    {
        //壁キックの操作不能時間は左右の速度更新をさせない
        if (lockTimer > 0)
        {
            return; // 操作不能時間がある場合は移動処理を行わない
        }
        
        //空中で何も入力していないときは空中での慣性を活かすために横の速度を維持する（壁キックの操作不能時間もここで維持される）
        float targetHorizontalVelocity;
        
        // 今の状態がDashならdashSpeed、そうでなければwalkSpeedを使う
        float speed = (psm.CurrentState == PlayerState.Dash) ? dashSpeed : walkSpeed;
        // パークの移動速度倍率を適用
        speed *= perk.MoveSpeedMultiplier;

        //梯子の状態のときは上下の入力を受け付ける
        if (psm.CurrentState == PlayerState.Ladder)
        {
            //はしごに触れているときの上下の移動処理
            rb.gravityScale = 0; //重力の影響を消す
            
            //左右移動と上下移動を両方適用する
            float vVelocity = verticalInput * ladderSpeed; // 上下の入力で移動する速度
            float hVelocity = input * speed; // 左右の入力で移動する速度
            rb.linearVelocity = new Vector2(hVelocity, vVelocity); // 上下の入力で移動する
            return; // はしご状態のときはこれ以上の移動処理をしない
        }
        
        if (!isGrounded && input == 0)
        {
            // 入力がないときは現在の速度を維持する（空中での慣性を活かす）
            targetHorizontalVelocity = rb.linearVelocity.x;
        }
        else
        {
            // 入力がある（または地面にいる）ときは通常の速度計算をする
            targetHorizontalVelocity = input * speed; 
        }
        
        //壁つかまりのパーク
        if (isWalled && perk.CanClimb && !isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            rb.linearVelocity = new Vector2(0, 0); // 止まる
            rb.gravityScale = 0; // 重力の影響を消す
            return;
        }
    
        //滞空のパーク
        // 落下中(y < 0)にジャンプボタンを押し続けていれば重力を軽くする
        if (!isGrounded && isJumpHolding && perk.CanGlide && rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = defaultGravityScale * glideGravityMultiplier; // 重力をglideGravityMultiplier倍にしてゆっくり落とす
        }
        else
        {
            // 通常時は元の重力に戻す
            rb.gravityScale = defaultGravityScale; 
        }

        //
        float currentYVelocity = rb.linearVelocity.y;
        
        // 物理速度を更新（左右移動のみ。縦はrb.velocity.yを維持）
        rb.linearVelocity = new Vector2(targetHorizontalVelocity, currentYVelocity);
    }

    private void ApplyJump(float jumpPowerMultiplier = 1.0f)
    {
        //jumpFoorceが正であることを確認
        float force = jumpForce * jumpPowerMultiplier;
        // 上方向に力を加える
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
        Debug.Log($"ジャンプ実行　現在の速度: {rb.linearVelocity} ジャンプ力の倍率: {jumpPowerMultiplier}");
    }

    private bool CheckWall(float horizontalInput)
    {
        if (horizontalInput == 0) return false; // 入力がない場合は壁判定不要

        float direction = horizontalInput > 0 ? 1 : -1; // 右なら1、左なら-1
        Vector2 wallCheckPos = (Vector2)transform.position + new Vector2(direction * wallCheckOffset, 0);
       
        //壁判定の中に梯子があるか確認
        Collider2D hitLadder = Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0f, wallLayer);

        if (hitLadder != null)
        {
            if (hitLadder.CompareTag("Ladder"))
            {
                return false; // 梯子がある場合は壁判定をしない
            }
            return true; // 梯子以外の壁がある場合は壁判定をする
        }
        
        return Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0f, wallLayer);
    }

    //壁キックの処理のメソッド
    private void ApplyWallkick()
    {
        //壁が左右のどちらにあるのか、入力方向と向いている方向からCheckWallと同じように判定する
        float wallDirection = 0;

        if (CheckWall(1f))
        {
            wallDirection = 1; // 右に壁がある
        }
        else if (CheckWall(-1f))
        {
            wallDirection = -1; // 左に壁がある
        }
        else
        {
            // キャラの向き（flipX）から壁の位置を推測する
        // flipXがfalse（右向き）なら右に壁があるはず、true（左向き）なら左に壁がある
            wallDirection = spriteRenderer.flipX ? -1f : 1f;
        }

        //壁と反対の方向にする
        float kickDirection = -wallDirection;

        //速度を上書きして壁キックをする
        rb.linearVelocity = new Vector2(kickDirection * wallKickForce.x, wallKickForce.y);

        //飛んでいく方向にキャラクターの向きを変える
        if (spriteRenderer != null)
        {
            // 右に飛ぶなら右を向く、左に飛ぶなら左を向く
            spriteRenderer.flipX = (kickDirection < 0); 
        }
        
        //壁キックの操作不能時間をセットする
        lockTimer = wallKickLockDuration;

        //壁キック後は空中ジャンプ回数を回復(いまのところ使わなそうなのでコメントアウト)
        //var perk = PerkEffectReference.Instance;
        //remainJumpCount = perk.AdditionalJumpCount;
    }

    // デバッグ用：地面と壁の判定の可視化
    private void OnDrawGizmosSelected()
    {
        // 地面（赤）
        Gizmos.color = Color.red;
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);

        // 壁（青）
        Gizmos.color = Color.blue;
        //右側
        Gizmos.DrawWireCube((Vector2)transform.position + Vector2.right * wallCheckOffset, wallCheckSize);
        //左側
        Gizmos.DrawWireCube((Vector2)transform.position + Vector2.left * wallCheckOffset, wallCheckSize);
    }

    
    
    // ミス判定（例：トゲに触れた等）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突相手に "Hazard" タグなどがついているかチェック
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Miss();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.CompareTag("Ladder"))
        {
            canLadderClimb = true; // はしごに触れたら登れるようにする
        }

        //落下死用のトリガー（例：画面下に落ちたときなど）
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log("落下死");
            status.ForceGameOver();
            GameSceneStateMachine.OnOver?.Invoke();
        }
        
        //ゴール判定
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("ゴール！");
            GameSceneStateMachine.OnClear?.Invoke(); // ゴールイベントを通知
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   
        if (collision.gameObject.CompareTag("Ladder"))
        {
            canLadderClimb = false; // はしごから離れたら登れなくする
        }
    }
 
    private void Miss()
    {
        var perk = PerkEffectReference.Instance;
        
        //無敵中はダメージ判定を行わない
        if (perk.IsInvincible)
        {
            Debug.Log("ダメージを受けない（無敵状態）");
            return;
        }
        
        PerkEvents.Damaged?.Invoke(); // ダメージを受けたイベントを通知
        
        //強制ジャンプのパークがある場合
        if (perk.IsForcedJump)
        {
            ApplyJump(perk.JumpPowerMultiplier); // ジャンプ力の倍率も適用
            Debug.Log("強制ジャンプ");

            //空中ジャンプ回数をリセット
            remainJumpCount = perk.AdditionalJumpCount;
            
            //強制ジャンプのスタックを減らす
            perk.ForcedJumpStack--;
            Debug.Log($"残り強制ジャンプスタック: {perk.ForcedJumpStack}");
            
            //強制ジャンプさせた後はダメージを受けないように無敵時間を付与（例: 1秒間無敵）
            perk.InvincibleSeconds = 1f;
            return;
        }
        
        //PlayerStatusの方でダメージを受けるかどうかを判定してもらう
        if (status.CanTakeDamage())
        {
            Debug.Log("ダメージを受ける");
            status.DecreaseLife();
            GameView.OnDamaged?.Invoke(status.MaxLives, status.CurrentLives); // ダメージを受けたことを通知
            SoundSourceObject.Instance.PlayDamagedSE();

            perk.InvincibleSeconds = 2f;

        }
        else
        {
            Debug.Log("ダメージを受けない");
        }

    }

    private void Respawn()
    {
        // 位置を戻し、速度をゼロにする
        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;
        // 状態を待機状態に戻す（ここは後々）
    }

    private void HandleGameOver()
    {
        Debug.Log("ゲームオーバー！タイトル画面へ戻るなどの処理");
        GameSceneStateMachine.OnOver?.Invoke(); // ゲームオーバーイベントを通知
    }

    private System.Collections.IEnumerator InvincibleCount()
    {
        if (spriteRenderer == null)
        {
            yield break; // SpriteRendererがない場合はコルーチンを終了
        }
        
        isInvincible = true;
        var perk = PerkEffectReference.Instance;

        // 無敵時間が続く限り点滅を続ける
        while (perk.InvincibleSeconds > 0)
        {
            yield return null;
        }

        isInvincible = false;
    }
}