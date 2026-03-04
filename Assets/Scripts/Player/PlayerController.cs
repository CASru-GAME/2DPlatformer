using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMove psm;
    //プレイヤーの移動速度
    [Header("移動設定")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float jumpForce = 12f;

    [Header("地面判定設定")]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private float groundCheckOffset = 0.6f;
    [SerializeField] private LayerMask groundLayer; // 地面判定用のレイヤー
   
    private Rigidbody2D rb;
   
    private void Awake()
    {
        // プレイヤーの状態管理クラスのインスタンス化
        psm = new PlayerMove();
        // 物理移動コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();
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

        psm.UpdateState(moveInput, rb.linearVelocity.y, jump, dash, isGrounded);
    
        //実際の移動処理を適用
        ApplyMovement(moveInput);

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

    // デバッグ用：地面判定の可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 groundCheckPos = (Vector2)transform.position + Vector2.down * groundCheckOffset;
        Gizmos.DrawWireCube(groundCheckPos, groundCheckSize);
    }
}
