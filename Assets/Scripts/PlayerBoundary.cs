using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        bool outOfBounds = false;

        if (pos.x < minX) { pos.x = minX; outOfBounds = true; }
        if (pos.x > maxX) { pos.x = maxX; outOfBounds = true; }
        if (pos.z < minZ) { pos.z = minZ; outOfBounds = true; }
        if (pos.z > maxZ) { pos.z = maxZ; outOfBounds = true; }

        if (outOfBounds)
            transform.position = pos;
    }
}