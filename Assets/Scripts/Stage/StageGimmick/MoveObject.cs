using UnityEngine;

public class MoveObject : MonoBehaviour
{
    
    public float moveDistance = 3f;
    public float speed = 2f;
    public Transform moveCenter;

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * moveDistance;

        transform.position = new Vector3(moveCenter.position.x + x, moveCenter.position.y, moveCenter.position.z);
    }
}
