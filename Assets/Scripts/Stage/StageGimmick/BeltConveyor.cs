using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    public float Conveyorspeed; //速度

    void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * Conveyorspeed; //ベルトコンベアに乗っているオブジェクトを移動させる
    }
}
