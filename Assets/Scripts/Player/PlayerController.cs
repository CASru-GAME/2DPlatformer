using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMove psm;
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
   
    // プレイヤーの残機などのステータスを管理するクラス
    private PlayerStatus status; 
    [Header("ステータス設定")]
    [SerializeField] private int initialLives = 3;
    [SerializeField] private Vector2 respawnPosition; // ミスした時の復活地点

    private Rigidbody2D rb;
   
    private void Awake()
    {
        // プレイヤーの状態管理クラスのインスタンス化
        psm = new PlayerMove();
        // 物理移動コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        // プレイヤーのステータス管理クラスのインスタンス化
        status = new PlayerStatus(initialLives);

        // 残機が変化したときのイベントに、UI更新などの処理を登録する
        status.OnGameOver += HandleGameOver;
    }

    private void Start()
    {
        // 初期位置をrespawnPositionに設定
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // 入力の取得
        float moveInput = Input.GetAxisRaw("Horizontal"); 
        bool jump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space); 
        bool dash = Input.GetKey(KeyCode.LeftShift); 
        
        // 地面に接地しているかの判定 
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        bool isGrounded = Physics2D.OverlapBox(groundCheckPos, groundCheckSize, 0f, groundLayer);

        // 壁に接触しているかの判定
        bool isWalled = CheckWall(moveInput);

        // 壁がある方向に進もうとしているなら、入力を 0 にして侵入を防ぐ（補助的処理）
        float effectiveInput = isWalled ? 0 : moveInput;

        psm.UpdateState(effectiveInput, rb.linearVelocity.y, jump, dash, isGrounded);
    
        //実際の移動処理を適用
        ApplyMovement(effectiveInput);

        //jumpが押されていて、かつ地面に接地している場合はジャンプを適用
        if(jump && isGrounded)
        {
            ApplyJump();
        }
    }
    
    private void ApplyMovement(float input)
    {
        // 今の状態がDashならdashSpeed、そうでなければwalkSpeedを使う
        float speed = (psm.CurrentState == PlayerState.Dash) ? dashSpeed : walkSpeed;
        // 物理速度を更新（左右移動のみ。縦はrb.velocity.yを維持）
        rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
    }

    private void ApplyJump()
    {
        // 上方向に力を加える
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
        Debug.Log("ミス！");
        status.DecreaseLife(); // ロジッククラスに残機を減らしてもらう

        if (status.CurrentLives > 0)
        {
            Respawn();
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
}