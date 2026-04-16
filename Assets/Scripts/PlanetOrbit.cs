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
    private Transform galaxyTransform;

    public void StartOrbiting()
    {
        float dx = transform.position.x - GetWorldCenter().x;
        float dz = transform.position.z - GetWorldCenter().z;
        angle = Mathf.Atan2(dz, dx) * Mathf.Rad2Deg;
        isOrbiting = true;
    }

    Vector3 GetWorldCenter()
    {
        if (galaxyTransform == null)
        {
            GameObject galaxy = GameObject.Find("Galaxy");
            if (galaxy != null)
                galaxyTransform = galaxy.transform;
        }

        if (galaxyTransform != null)
        {
            // Account for galaxy scale and position
            return new Vector3(
                galaxyTransform.position.x + center.x * galaxyTransform.localScale.x,
                galaxyTransform.position.y + center.y * galaxyTransform.localScale.y,
                galaxyTransform.position.z + center.z * galaxyTransform.localScale.z
            );
        }

        return center;
    }

    float GetScaledRadiusX()
    {
        if (galaxyTransform != null)
            return radiusX * galaxyTransform.localScale.x;
        return radiusX;
    }

    float GetScaledRadiusZ()
    {
        if (galaxyTransform != null)
            return radiusZ * galaxyTransform.localScale.z;
        return radiusZ;
    }

    void Update()
    {
        if (!isOrbiting) return;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Vector3 worldCenter = GetWorldCenter();

        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        float x = worldCenter.x + Mathf.Cos(rad) * GetScaledRadiusX();
        float z = worldCenter.z + Mathf.Sin(rad) * GetScaledRadiusZ();
        transform.position = new Vector3(x, worldCenter.y, z);

        transform.Rotate(0f, selfRotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}