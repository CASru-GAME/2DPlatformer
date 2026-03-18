using UnityEngine;

//状態の定義
public enum PlayerState
{
    Idle,
    Walk,
    Dash,
    Jump,
    Fall,
    Climb,  //壁つかまり
    Glide   //滑空
}

//プレイヤーの動作を管理するクラス
public class PlayerMove
{
    private PlayerState currentState;

    //コンストラクタ
    public PlayerMove()
    {
        //初期状態はIdle
        currentState = PlayerState.Idle;
    }

    //horizontalInput: -1 (左), 0 (なし), 1 (右)の横方向の入力値
    //verticalVelocity: プレイヤーの垂直速度
    public void UpdateState(float horizontalInput, float verticalVelocity, bool isJumpPressed, bool isDashPressed, bool isGrounded, bool isClimbing, bool isGliding)
    {
        if (isClimbing)
        {
            currentState = PlayerState.Climb;
            return;
        }
        
        if (isGliding)
        {
            currentState = PlayerState.Glide;
            return;
        }

        switch (currentState)
        {
            //待機状態
            case PlayerState.Idle:
                if (!isGrounded)
                {
                    currentState = PlayerState.Fall;
                }
                else if (isJumpPressed)
                {
                    currentState = PlayerState.Jump;
                }
                else if (horizontalInput != 0)
                {
                    currentState = isDashPressed ? PlayerState.Dash : PlayerState.Walk;
                }
                break;

            //歩行状態    
            case PlayerState.Walk:
                if (!isGrounded)
                {
                    currentState = PlayerState.Fall;
                }
                else if (isJumpPressed)
                {
                    currentState = PlayerState.Jump;
                }
                else if (horizontalInput == 0)
                {
                    currentState = PlayerState.Idle;
                }
                else if (isDashPressed)
                {
                    currentState = PlayerState.Dash;
                }
                break;

            //ダッシュ状態
            case PlayerState.Dash:
                if (!isGrounded)
                {
                    currentState = PlayerState.Fall;
                }
                else if (isJumpPressed)
                {
                    currentState = PlayerState.Jump;
                }
                else if (horizontalInput == 0)
                {
                    currentState = PlayerState.Idle;
                }
                else if (!isDashPressed)
                {
                    currentState = PlayerState.Walk;
                }
                break;

            //ジャンプ状態
            case PlayerState.Jump:
                if (verticalVelocity <= 0.1f)
                {
                    currentState = PlayerState.Fall;
                }
                else if (isGrounded && verticalVelocity <= 0)
                {
                    currentState = PlayerState.Idle;
                }
                break;

            //落下状態
            case PlayerState.Fall:
                if (isGrounded)
                {
                    if (horizontalInput != 0)
                    {
                        currentState = isDashPressed ? PlayerState.Dash : PlayerState.Walk;
                    }
                    else
                    {
                        currentState = PlayerState.Idle;
                    }
                }
                break;

            //壁つかまり状態からの遷移
            case PlayerState.Climb: // 壁つかまり状態からの遷移
                if (isGrounded) 
                {
                    currentState = PlayerState.Idle;
                }
                else if (!isClimbing) 
                {
                    currentState = PlayerState.Fall;
                }
                break;

            //滑空状態からの遷移
            case PlayerState.Glide:
                if (isGrounded)
                {
                    currentState = PlayerState.Idle;
                }
                else if (!isGliding)
                {
                    currentState = PlayerState.Fall;
                }
                break;
        }
    }

    public PlayerState CurrentState => currentState;
}
