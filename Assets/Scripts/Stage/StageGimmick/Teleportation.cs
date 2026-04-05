using Scene.Controller;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
   [SerializeField] private Vector2 targetPosition; // ワープ先

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.position = targetPosition;
            SoundSourceObject.Instance.PlayWarpSE();
            Destroy(gameObject);
        }
    }

}
