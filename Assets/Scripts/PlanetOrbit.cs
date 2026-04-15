using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public float radiusX = 6f;
    public float radiusZ = 4f;
    public float orbitSpeed = 10f;
    public float selfRotationSpeed = 10f;
    public Vector3 center = Vector3.zero;

    private float angle = 0f;
    private bool isOrbiting = false;

    public void StartOrbiting()
    {
        isOrbiting = true;
    }

    void Update()
    {
        if (!isOrbiting) return;

        // Force kinematic off so orbit script can move it
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Orbit around sun
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        float x = center.x + Mathf.Cos(rad) * radiusX;
        float z = center.z + Mathf.Sin(rad) * radiusZ;
        transform.position = new Vector3(x, center.y, z);

        // Self rotation on Y axis
        transform.Rotate(0f, selfRotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}