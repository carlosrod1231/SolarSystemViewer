using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class ButtonPress : MonoBehaviour
{
    [Header("Button Settings")]
    public float maxPressDepth = 0.08f;
    public float returnSpeed = 5f;

    private Vector3 originalLocalPosition;
    private float minY;
    private bool isTriggered = false;
    private bool isReturning = false;
    private XRSimpleInteractable interactable;
    private InspectStation inspectStation;
    private bool isBeingPoked = false;
    private Transform pokerTransform;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        minY = originalLocalPosition.y - maxPressDepth;
        interactable = GetComponentInParent<XRSimpleInteractable>();
        inspectStation = FindObjectOfType<InspectStation>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnPoked);
            interactable.selectExited.AddListener(OnPokeReleased);
        }
    }

    void OnPoked(SelectEnterEventArgs args)
    {
        isBeingPoked = true;
        pokerTransform = args.interactorObject.transform;
        isTriggered = false;
    }

    void OnPokeReleased(SelectExitEventArgs args)
    {
        isBeingPoked = false;
        pokerTransform = null;
        isReturning = true;
    }

    void Update()
    {
        if (isBeingPoked && pokerTransform != null)
        {
            // Calculate how far the poker has pushed down
            float pokerY = transform.parent.InverseTransformPoint(pokerTransform.position).y;
            float targetY = Mathf.Clamp(pokerY, minY, originalLocalPosition.y);
            transform.localPosition = new Vector3(
                originalLocalPosition.x,
                targetY,
                originalLocalPosition.z
            );

            // Trigger when button reaches bottom
            if (!isTriggered && targetY <= minY + 0.01f)
            {
                isTriggered = true;
                if (inspectStation != null)
                    inspectStation.ReturnPlanetToOrbit();
            }
        }
        else if (isReturning)
        {
            // Spring back to original position
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                originalLocalPosition,
                Time.deltaTime * returnSpeed
            );

            if (Vector3.Distance(transform.localPosition, originalLocalPosition) < 0.001f)
            {
                transform.localPosition = originalLocalPosition;
                isReturning = false;
                isTriggered = false;
            }
        }
    }

    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnPoked);
            interactable.selectExited.RemoveListener(OnPokeReleased);
        }
    }
}
//test