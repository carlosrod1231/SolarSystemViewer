using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRing : MonoBehaviour
{
    public float radiusX = 6f;
    public float radiusZ = 4f;
    public int resolution = 128;

    private LineRenderer lr;

    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        if (lr != null)
            DrawEllipse();
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        DrawEllipse();
    }

    void DrawEllipse()
    {
        lr.positionCount = resolution + 1;
        for (int i = 0; i <= resolution; i++)
        {
            float angle = Mathf.Deg2Rad * (360f / resolution) * i;
            float x = Mathf.Cos(angle) * radiusX;
            float z = Mathf.Sin(angle) * radiusZ;
            lr.SetPosition(i, new Vector3(x, transform.position.y, z));
        }
    }
}