using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SunInteraction : MonoBehaviour
{
    private bool gameWon = false;
    private bool isHovered = false;

    public void SetGameWon()
    {
        gameWon = true;
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
        if (!gameWon || !isHovered) return;

        // Check right hand trigger
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool rightTrigger = false;
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out rightTrigger);

        // Check left hand trigger
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool leftTrigger = false;
        leftDevice.TryGetFeatureValue(CommonUsages.triggerButton, out leftTrigger);

        if (rightTrigger || leftTrigger)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}