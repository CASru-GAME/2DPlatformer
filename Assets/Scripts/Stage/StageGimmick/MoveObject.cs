using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    [Header("スピード")]
    public float speed = 2.0f;

    private float t = 0f;
    private bool goingToB = true;

    void Update()
    {
        if (goingToB)
        {
            t += Time.deltaTime * speed;
            if (t >= 1f)
            {
                t = 1f;
                goingToB = false;
            }
        }
        else
        {
            t -= Time.deltaTime * speed;
            if (t <= 0f)
            {
                t = 0f;
                goingToB = true;
            }
        }
        
        transform.position = Vector3.Lerp(pointA, pointB, t);
    }

}
