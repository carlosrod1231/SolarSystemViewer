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

            // Disable grabbing
            var grab = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grab != null) grab.enabled = false;

            // Disable rigidbody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Snap planet to correct orbit height and start orbiting
            PlanetOrbit orbit = other.GetComponent<PlanetOrbit>();
            if (orbit != null)
            {
                Vector3 snappedPosition = other.transform.position;
                snappedPosition.y = orbit.center.y;
                other.transform.position = snappedPosition;
                orbit.StartOrbiting();
            }

            // Hide slot sphere
            GetComponent<MeshRenderer>().enabled = false;

            // Tell GameManager
            gameManager.OnCorrectPlanetPlaced(gameObject);
        }
        else
        {
            gameManager.OnWrongPlanetPlaced();
        }
    }
}