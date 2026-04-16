using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class BlackHoleInteraction : MonoBehaviour
{
    private bool isHovered = false;
    private SolarSystemRise solarSystemRise;

    void Start()
    {
        solarSystemRise = FindObjectOfType<SolarSystemRise>();
    }

    public void OnHoverEntered()
    {
        isHovered = true;
    }

    public void OnHoverExited()
    {
        isHovered = false;
    }

    void Update()
    {
        if (!isHovered) return;

        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool rightTrigger = false;
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);

        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool leftTrigger = false;
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);

        if (rightTrigger || leftTrigger)
        {
            if (solarSystemRise != null)
                solarSystemRise.LowerGalaxy();
        }
    }
}