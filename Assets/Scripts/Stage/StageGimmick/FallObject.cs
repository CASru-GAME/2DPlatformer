using UnityEngine;
using System.Collections;

public class FallObject : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1.5f;
    [SerializeField] private float resetDelay = 3f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(FallAndReset());
        }
    }

    IEnumerator FallAndReset()
    {
        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(resetDelay);

        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = startPosition;
        transform.rotation = startRotation;

        isTriggered = false;
    }
}
