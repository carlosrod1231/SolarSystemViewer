using UnityEngine;

public class SolarSystemRise : MonoBehaviour
{
    [Header("Galaxy")]
    public GameObject galaxy;
    public float raisedY = 50f;
    public float loweredY = 0f;
    public float riseSpeed = 5f;

    [Header("Galaxy Scale")]
    public float loweredScale = 1f;
    public float raisedScale = 3f;

    [Header("Objects")]
    public GameObject blackHole;
    public GameObject comet;

    private bool isMoving = false;
    private float targetY;
    private Vector3 originalScale;

    void Start()
    {
        targetY = loweredY;
        if (galaxy != null)
            originalScale = galaxy.transform.localScale;
    }

    public float GetRaisedAmount()
    {
        if (galaxy == null) return 0f;
        return Mathf.InverseLerp(loweredY, raisedY, galaxy.transform.position.y);
    }

    public void ActivateWinState()
    {
        if (blackHole != null) blackHole.SetActive(true);
        if (comet != null) comet.SetActive(true);
        RaiseGalaxy();
    }

    public void LowerGalaxy()
    {
        targetY = loweredY;
        isMoving = true;
    }

    public void RaiseGalaxy()
    {
        targetY = raisedY;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || galaxy == null) return;

        // Move galaxy up/down
        Vector3 currentPos = galaxy.transform.position;
        float newY = Mathf.MoveTowards(currentPos.y, targetY, riseSpeed * Time.deltaTime);
        galaxy.transform.position = new Vector3(currentPos.x, newY, currentPos.z);

        // Scale galaxy based on raised amount
        float t = Mathf.InverseLerp(loweredY, raisedY, newY);
        float currentScale = Mathf.Lerp(loweredScale, raisedScale, t);
        galaxy.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if (Mathf.Approximately(newY, targetY))
            isMoving = false;
    }
}