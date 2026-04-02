using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    private Transform player;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    private Vector2 velocity;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        velocity = transform.right * speed;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        float angle = Vector2.SignedAngle(velocity, direction);
        float rotateAmount = Mathf.Clamp(angle, -rotateSpeed * Time.deltaTime, rotateSpeed * Time.deltaTime);

        velocity = Quaternion.Euler(0, 0, rotateAmount) * velocity;
        velocity = velocity.normalized * speed;

        transform.position += (Vector3)velocity * Time.deltaTime;

    }
}
