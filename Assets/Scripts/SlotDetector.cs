using UnityEngine;

public class SlotDetector : MonoBehaviour
{
    public string expectedPlanetName;
    private GameManager gameManager;
    private bool isOccupied = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return;

        PlanetData planet = other.GetComponent<PlanetData>();
        if (planet == null) return;

        if (planet.planetName == expectedPlanetName)
        {
            isOccupied = true;

            // Snap planet to slot position
            other.transform.position = transform.position;
            other.transform.SetParent(transform);

            // Disable planet rigidbody so it stays in place
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Hide the slot sphere
            GetComponent<MeshRenderer>().enabled = false;

            // Tell GameManager it was correct
            gameManager.OnCorrectPlanetPlaced(gameObject);
        }
        else
        {
            gameManager.OnWrongPlanetPlaced();
        }
    }
}