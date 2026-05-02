using UnityEngine;

public class FlyingShuttle : MonoBehaviour
{
    public float speed = 10f;
    public float radius = 40f;
    public float height = 20f;
    public float startAngle = 0f;

    private float angle;

    void Start()
    {
        angle = startAngle;
        transform.position = new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            height,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        Vector3 newPos = new Vector3(x, height, z);
        Vector3 direction = (newPos - transform.position).normalized;

        transform.position = newPos;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}