using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

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
            // Planet belongs here but check if it's the requested one
            if (planet.planetName != gameManager.currentRequestedPlanet)
            {
                XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
                if (grab != null)
                    grab.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grab);

                gameManager.OnWrongTime();

                PlanetShake shake = other.GetComponent<PlanetShake>();
                if (shake != null) shake.Shake();
                return;
            }

            // Correct planet and correct time
            isOccupied = true;

            XRGrabInteractable grabCorrect = other.GetComponent<XRGrabInteractable>();
            if (grabCorrect != null)
            {
                grabCorrect.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grabCorrect);
                grabCorrect.enabled = false;
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            PlanetOrbit orbit = other.GetComponent<PlanetOrbit>();
            if (orbit != null)
            {
                Vector3 snappedPosition = other.transform.position;
                snappedPosition.y = orbit.center.y;
                other.transform.position = snappedPosition;
                orbit.StartOrbiting();
            }

            GetComponent<MeshRenderer>().enabled = false;
            gameManager.OnCorrectPlanetPlaced(gameObject);
        }
        else
        {
            XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
            if (grab != null)
                grab.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grab);

            gameManager.OnWrongPlanetPlaced();

            PlanetShake shake = other.GetComponent<PlanetShake>();
            if (shake != null) shake.Shake();
        }
    }
}