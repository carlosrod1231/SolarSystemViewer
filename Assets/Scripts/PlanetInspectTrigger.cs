using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlanetInspectTrigger : MonoBehaviour
{
    private bool isHovered = false;
    private bool isOrbiting = false;
    private InspectStation inspectStation;
    private XRGrabInteractable grabInteractable;
    private XRSimpleInteractable simpleInteractable;

    void Start()
    {
        inspectStation = FindObjectOfType<InspectStation>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();

        // Start with simple interactable disabled
        if (simpleInteractable != null)
            simpleInteractable.enabled = false;
    }

    public void SetOrbiting(bool orbiting)
    {
        isOrbiting = orbiting;

        // Disable grab when orbiting
        if (grabInteractable != null)
            grabInteractable.enabled = !orbiting;

        // Enable simple interactable when orbiting
        if (simpleInteractable != null)
            simpleInteractable.enabled = orbiting;
    }

    public void OnHoverEntered() { isHovered = true; }
    public void OnHoverExited() { isHovered = false; }

    void Update()
    {
        if (!isHovered || !isOrbiting) return;

        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool rightTrigger = false;
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool leftTrigger = false;
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);

        if (rightTrigger || leftTrigger)
        {
            if (inspectStation != null)
                inspectStation.BringPlanetToStation(gameObject);
        }
    }
}