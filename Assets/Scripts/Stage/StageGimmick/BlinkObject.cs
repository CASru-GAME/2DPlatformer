using UnityEngine;
using System.Collections;

public class BlinkObject : MonoBehaviour
{
    public float interval = 1.0f;

    private Renderer rend;
    private Collider2D col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider2D>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink() 
    {
        while (true) 
        {
            rend.enabled = !rend.enabled;
            col.enabled = !col.enabled;
            yield return new WaitForSeconds(interval);
        }
    }
}
