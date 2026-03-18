using UnityEngine;
using Perk.Model;   //パークの参照

public class PlayerController : MonoBehaviour
{
    private PlayerMove psm;

    private Animator anim; //アニメーターコンポーネントを入れる変数

    //プレイヤーの移動速度
    [Header("移動設定")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
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

    //現在点滅中か（初期は点滅していない）
    private bool isFlashing = false;
    
    // プレイヤーの残機などのステータスを管理するクラス
    private PlayerStatus status; 
    [Header("ステータス設定")]
    [SerializeField] private int initialLives = 3;
    [SerializeField] private Vector2 respawnPosition; // ミスした時の復活地点

    [Header("パーク詳細設定")]
    // 0.0（無重力）〜 1.0（通常重力）の間で調整
    [Range(0f, 1f)] 
    [SerializeField] private float glideGravityMultiplier = 0.2f;

    
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
        
        // アニメーターコンポーネントの取得
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("アニメーターが付いていないのでアニメーションは再生されません");
        }

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
    }

    // Update is called once per frame
    private void Update()
    {
        //パーク情報の取得
        var perk = PerkEffectReference.Instance;

        //シールドのパークがあるときはシールドオブジェクトを表示
        if (shieldObject != null)
        {
            //シールドの表示非表示を切り替える
            shieldObject.SetActive(perk.HasShield);
        }

        //無敵時間になったら点滅開始
        if(perk.IsInvincible && !isFlashing)
        {
            StartCoroutine(FlashEffect());
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

                //念のため点滅を止める（無敵時間が0になったときに点滅も止まるように）
                if (spriteRenderer != null)
                {
                    // 点滅を止めて元の色に戻す
                    spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
        
        // 入力の取得
        float moveInput = Input.GetAxisRaw("Horizontal"); 
        // GetKeyDown：押した瞬間（ジャンプ判定用）
        bool jumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow); // ジャンプボタンを押した瞬間
        // GetKey：押しっぱなし（滞空/グライド判定用）
        bool jumpHold = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow); // ジャンプボタンを押し続けているか
        bool dash = Input.GetKey(KeyCode.LeftShift); 

        // 地面に接地しているかの判定 
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        bool isGrounded = Physics2D.OverlapBox(groundCheckPos, groundCheckSize, 0f, groundLayer);

        // 壁に接触しているかの判定
        bool isWalled = CheckWall(moveInput);

        //状態更新用のフラグを用意(今、壁つかまり中か滞空中かを計算して渡す）
        bool isClimbing = isWalled && perk.CanClimb && !isGrounded && rb.linearVelocity.y <= 0;
        bool isGliding = !isGrounded && jumpHold && perk.CanGlide && rb.linearVelocity.y < 0;

        // 壁がある方向に進もうとしているなら、入力を 0 にして侵入を防ぐ（補助的処理）
        float effectiveInput = isWalled ? 0 : moveInput;

        //空中ジャンプ回数のリセット
        if (isGrounded)
        {
            //基本１回(最初のジャンプ用) + 追加のジャンプ回数
            remainJumpCount = 1 + perk.AdditionalJumpCount; //地面にいるときは追加ジャンプ回数+1回ジャンプできる
        }
        
        psm.UpdateState(effectiveInput, rb.linearVelocity.y, jumpDown, dash, isGrounded, isClimbing, isGliding);
        
        //実際の移動処理に引数を追加してパーク対応
        //滞空判定に使うのでjumpHoldを渡す
        ApplyMovement(effectiveInput, isWalled, isGrounded, jumpHold, perk);

        //ジャンプ処理を空中ジャンプ対応の「HandleJump」に変更
        //ジャンプの実行なのでjumpDownを使う
        if (jumpDown)
        {
            HandleJump(isGrounded, isWalled, perk);
        }

        //アニメーションの更新
        UpdateAnimation();
    }

    //アニメーションの更新を行うメソッド
    private void UpdateAnimation()
    {
        if (anim == null)
        {
            return; // アニメーターがない場合は何もしない
        }
        
        anim.SetInteger("State", (int)psm.CurrentState);
    }

    //ジャンプの判断
    private void HandleJump(bool isGrounded, bool isWalled, PerkEffectReference perk)
    {
        //壁つかまりのパークがあって壁についているときはジャンプできない
        if (isWalled && !isGrounded && perk.CanClimb)
        {
            return; // ジャンプさせない
        }

        //地面にいるなら跳べる
        if (isGrounded)
        {
            ApplyJump(perk.JumpPowerMultiplier);
        }
        //空中にいるが、ジャンプ回数が残っているなら「空中ジャンプ」
        else if (remainJumpCount > 0)
        {
            ApplyJump(perk.JumpPowerMultiplier);
            remainJumpCount--; // 回数を1減らす
        }
    }
    
    private void ApplyMovement(float input, bool isWalled, bool isGrounded, bool isJumpHolding, PerkEffectReference perk)
    {
        // 今の状態がDashならdashSpeed、そうでなければwalkSpeedを使う
        float speed = (psm.CurrentState == PlayerState.Dash) ? dashSpeed : walkSpeed;
        // パークの移動速度倍率を適用
        speed *= perk.MoveSpeedMultiplier;

        //壁つかまりのパーク
        if (isWalled && perk.CanClimb && !isGrounded && rb.linearVelocity.y <= 0.01f)
        {
            rb.linearVelocity = new Vector2(0, 0); // 止まる
            rb.gravityScale = 0; // 重力の影響を消す
            return;
        }
    
        //滞空のパーク
        // 落下中(y < 0)にジャンプボタンを押し続けていれば重力を軽くする
        if (!isGrounded && isJumpHolding && perk.CanGlide && rb.linearVelocity.y < 0)
        {
            rb.gravityScale = defaultGravityScale * glideGravityMultiplier; // 重力をglideGravityMultiplier倍にしてゆっくり落とす
        }
        else
        {
            // 通常時は元の重力に戻す
            rb.gravityScale = defaultGravityScale; 
        }
        
        // 物理速度を更新（左右移動のみ。縦はrb.velocity.yを維持）
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
    }

    private void ApplyJump(float jumpPowerMultiplier = 1.0f)
    {
        // 上方向に力を加える
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * jumpPowerMultiplier);
    }

    private bool CheckWall(float horizontalInput)
    {
        if (horizontalInput == 0) return false; // 入力がない場合は壁判定不要

        float direction = horizontalInput > 0 ? 1 : -1; // 右なら1、左なら-1
        Vector2 wallCheckPos = (Vector2)transform.position + new Vector2(direction * wallCheckOffset, 0);
       
        return Physics2D.OverlapBox(wallCheckPos, wallCheckSize, 0f, wallLayer);
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

    private void Miss()
    {
        var perk = PerkEffectReference.Instance;
        
        //PlayerStatusの方でダメージを受けるかどうかを判定してもらう
        if (status.CanTakeDamage())
        {
            Debug.Log("ダメージを受ける");
            status.DecreaseLife();

            //無敵時間を付与（例: 2秒間無敵）
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
    }

    private System.Collections.IEnumerator FlashEffect()
    {
        if (spriteRenderer == null)
        {
            yield break; // SpriteRendererがない場合はコルーチンを終了
        }
        
        isFlashing = true;
        var perk = PerkEffectReference.Instance;

        // 無敵時間が続く限り点滅を続ける
        while (perk.InvincibleSeconds > 0)
        {
            // 半透明にする
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // 半透明にする
            yield return new WaitForSeconds(0.1f); // 点滅の速さ
            // 元の色に戻す
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 元の色に戻す
            yield return new WaitForSeconds(0.1f); // 点滅の速さ

        }
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 念のため元の色に戻す
        isFlashing = false;
    }
}