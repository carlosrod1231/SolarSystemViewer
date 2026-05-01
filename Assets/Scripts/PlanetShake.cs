using UnityEngine;
using System.Collections;

public class PlanetShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.15f;

    private Vector3 originalPosition;

    public void Shake()
    {
        originalPosition = transform.position;
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPosition.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPosition.y + Random.Range(-1f, 1f) * shakeMagnitude;
            float z = originalPosition.z + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(x, y, z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}