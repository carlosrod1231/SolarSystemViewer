using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRing : MonoBehaviour
{
    public float radiusX = 6f;
    public float radiusZ = 4f;
    public int resolution = 128;
    public float baseWidth = 0.05f;

    private LineRenderer lr;
    private Transform galaxyTransform;

    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        if (lr != null)
            DrawEllipse();
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        GameObject galaxy = GameObject.Find("Galaxy");
        if (galaxy != null)
            galaxyTransform = galaxy.transform;
        DrawEllipse();
    }

    void Update()
    {
        DrawEllipse();
    }

    void DrawEllipse()
    {
        if (lr == null) return;

        float galaxyScale = 1f;
        float galaxyX = 0f;
        float galaxyZ = 0f;
        float galaxyY = transform.position.y;

        if (galaxyTransform != null)
        {
            galaxyScale = galaxyTransform.localScale.x;
            galaxyX = galaxyTransform.position.x;
            galaxyZ = galaxyTransform.position.z;
        }

        // Scale line width with galaxy
        lr.startWidth = baseWidth * galaxyScale;
        lr.endWidth = baseWidth * galaxyScale;

        lr.positionCount = resolution + 1;
        for (int i = 0; i <= resolution; i++)
        {
            float angle = Mathf.Deg2Rad * (360f / resolution) * i;
            float x = galaxyX + Mathf.Cos(angle) * radiusX * galaxyScale;
            float z = galaxyZ + Mathf.Sin(angle) * radiusZ * galaxyScale;
            lr.SetPosition(i, new Vector3(x, galaxyY, z));
        }
    }
}