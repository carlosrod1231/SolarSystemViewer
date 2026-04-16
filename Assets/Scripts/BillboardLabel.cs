using UnityEngine;

public class BillboardLabel : MonoBehaviour
{
    public Transform targetPlanet;
    public float heightOffset = 1.5f;
    private Transform mainCamera;

    private float originalHeightOffset;
    private Vector3 originalScale;

    public float raisedHeightOffset = -4f;
    public Vector3 raisedScale = new Vector3(3f, 3f, 3f);

    private SolarSystemRise solarSystemRise;

    void Start()
    {
        mainCamera = Camera.main.transform;
        originalHeightOffset = heightOffset;
        originalScale = transform.localScale;
        solarSystemRise = FindObjectOfType<SolarSystemRise>();
    }

    void Update()
    {
        if (targetPlanet != null)
        {
            float t = 0f;
            if (solarSystemRise != null)
                t = solarSystemRise.GetRaisedAmount();

            float currentOffset = Mathf.Lerp(originalHeightOffset, raisedHeightOffset, t);
            transform.localScale = Vector3.Lerp(originalScale, raisedScale, t);
            transform.position = targetPlanet.position + Vector3.up * currentOffset;
        }

        if (mainCamera != null)
            transform.LookAt(transform.position + mainCamera.forward);
    }
}