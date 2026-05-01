using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = new Color(0.5f, 0.8f, 1f, 1f);
    public float glowIntensity = 0.3f;

    private Renderer[] renderers;
    private XRSimpleInteractable simpleInteractable;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (simpleInteractable != null)
        {
            simpleInteractable.hoverEntered.AddListener(OnHoverEntered);
            simpleInteractable.hoverExited.AddListener(OnHoverExited);
        }

        if (grabInteractable != null)
        {
            grabInteractable.hoverEntered.AddListener(OnHoverEntered);
            grabInteractable.hoverExited.AddListener(OnHoverExited);
        }

        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                m.EnableKeyword("_EMISSION");
            }
        }
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        SetGlow(true);
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        SetGlow(false);
    }

    void SetGlow(bool glow)
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                if (glow)
                    m.SetColor("_EmissionColor", glowColor * glowIntensity);
                else
                    m.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    void OnDestroy()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.hoverEntered.RemoveListener(OnHoverEntered);
            simpleInteractable.hoverExited.RemoveListener(OnHoverExited);
        }

        if (grabInteractable != null)
        {
            grabInteractable.hoverEntered.RemoveListener(OnHoverEntered);
            grabInteractable.hoverExited.RemoveListener(OnHoverExited);
        }
    }
}