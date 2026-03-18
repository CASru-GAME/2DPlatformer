using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("回転速度")]
    public float rotateSpeed = 100f;

    private float angle;

    private void Update()
    {
        angle += rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
