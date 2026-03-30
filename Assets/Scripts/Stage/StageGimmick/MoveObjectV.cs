using UnityEngine;

public class MoveObjectV : MonoBehaviour
{
    
    public float moveDistance = 3f;
    public float speed = 2f;
    public Transform moveCenter;

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * moveDistance;

        transform.position = new Vector3(moveCenter.position.x , moveCenter.position.y + y, moveCenter.position.z);
    }
}
