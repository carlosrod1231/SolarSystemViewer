using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

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
            if (planet.planetName != gameManager.currentRequestedPlanet)
            {
                XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
                if (grab != null)
                    grab.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grab);

                gameManager.OnWrongTime();

                PlanetShake shake = other.GetComponent<PlanetShake>();
                if (shake != null) shake.Shake();

                StartCoroutine(DisableSlotTemporarily());
                return;
            }

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

            PlanetInspectTrigger inspectTrigger = other.GetComponent<PlanetInspectTrigger>();
            if (inspectTrigger != null) inspectTrigger.SetOrbiting(true);

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

            StartCoroutine(DisableSlotTemporarily());
        }
    }

    private IEnumerator DisableSlotTemporarily()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        yield return new WaitForSeconds(2f);
        if (col != null) col.enabled = true;
    }
}