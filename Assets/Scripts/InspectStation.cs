using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InspectStation : MonoBehaviour
{
    [Header("Station")]
    public Transform planetDisplayPoint;

    [Header("Locked Sign")]
    public GameObject lockedSign;
    public GameObject lockedMessageText;
    public GameObject lockedMessageText2;

    [Header("UI")]
    public TextMeshProUGUI planetNameText;
    public TextMeshProUGUI planetInfoText;
    public GameObject inspectCanvas;

    [Header("Return Button")]
    public GameObject returnButton;

    private GameObject currentPlanet;
    private PlanetOrbit planetOrbit;
    private bool isUnlocked = false;
    private bool isOccupied = false;

    void Start()
    {
        if (lockedSign != null) lockedSign.SetActive(true);
        if (lockedMessageText != null) lockedMessageText.SetActive(true);
        if (lockedMessageText2 != null) lockedMessageText2.SetActive(true);
        if (inspectCanvas != null) inspectCanvas.SetActive(false);
        if (returnButton != null) returnButton.SetActive(false);
    }

    public void UnlockStation()
    {
        isUnlocked = true;
        if (lockedMessageText != null) lockedMessageText.SetActive(false);
        if (lockedMessageText2 != null) lockedMessageText2.SetActive(false);
        if (inspectCanvas != null) inspectCanvas.SetActive(true);
        planetNameText.text = "Point at an orbiting planet\nand pull the trigger to inspect it!";
        planetInfoText.text = "";
    }

    public void BringPlanetToStation(GameObject planet)
    {
        if (!isUnlocked || isOccupied) return;

        isOccupied = true;
        currentPlanet = planet;

        // Disable orbit FIRST before moving
        planetOrbit = planet.GetComponent<PlanetOrbit>();
        if (planetOrbit != null)
        {
            planetOrbit.enabled = false;
            Rigidbody rb = planet.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }

        // Wait one frame then move to ensure orbit is fully stopped
        StartCoroutine(MovePlanetAfterFrame(planet));

        // Disable grabbing while inspecting
        var grab = planet.GetComponent<XRGrabInteractable>();
        if (grab != null) grab.enabled = false;

        // Show info
        PlanetData data = planet.GetComponent<PlanetData>();
        if (data != null)
        {
            planetNameText.text = data.planetName;
            planetInfoText.text = GetPlanetInfo(data.planetName);
        }

        if (returnButton != null) returnButton.SetActive(true);
    }

    private System.Collections.IEnumerator MovePlanetAfterFrame(GameObject planet)
    {
        yield return null;
        yield return null;
        yield return null;

        PlanetData data = planet.GetComponent<PlanetData>();
        float heightOffset = data != null ? data.inspectHeightOffset : 0f;

        Vector3 displayPos = planetDisplayPoint.position;
        displayPos.y += heightOffset;
        planet.transform.position = displayPos;
    }

    public void ReturnPlanetToOrbit()
    {
        if (currentPlanet == null) return;

        if (planetOrbit != null) planetOrbit.enabled = true;

        var grab = currentPlanet.GetComponent<XRGrabInteractable>();
        if (grab != null) grab.enabled = true;

        Rigidbody rb = currentPlanet.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        currentPlanet = null;
        planetOrbit = null;
        isOccupied = false;

        if (returnButton != null) returnButton.SetActive(false);
        planetNameText.text = "Point at an orbiting planet\nand pull the trigger to inspect it!";
        planetInfoText.text = "";
    }

    string GetPlanetInfo(string planetName)
    {
        switch (planetName)
        {
            case "Mercury":
                return "Diameter: 4,879 km\nDistance from Sun: 57.9M km\nMoons: 0\nOrbital Period: 88 days\nFun Fact: A year on Mercury is shorter than its day!";
            case "Venus":
                return "Diameter: 12,104 km\nDistance from Sun: 108.2M km\nMoons: 0\nOrbital Period: 225 days\nFun Fact: Venus spins backwards and is the hottest planet!";
            case "Earth":
                return "Diameter: 12,742 km\nDistance from Sun: 149.6M km\nMoons: 1 (The Moon)\nOrbital Period: 365 days\nFun Fact: Earth is the only known planet to support life!";
            case "Mars":
                return "Diameter: 6,779 km\nDistance from Sun: 227.9M km\nMoons: 2 (Phobos, Deimos)\nOrbital Period: 687 days\nFun Fact: Mars has the tallest volcano in the solar system!";
            case "Jupiter":
                return "Diameter: 139,820 km\nDistance from Sun: 778.5M km\nMoons: 95\nOrbital Period: 12 years\nFun Fact: Jupiter's Great Red Spot is a storm over 350 years old!";
            case "Saturn":
                return "Diameter: 116,460 km\nDistance from Sun: 1.43B km\nMoons: 146\nOrbital Period: 29 years\nFun Fact: Saturn is so light it could float on water!";
            case "Uranus":
                return "Diameter: 50,724 km\nDistance from Sun: 2.87B km\nMoons: 28\nOrbital Period: 84 years\nFun Fact: Uranus rotates on its side at 98 degrees!";
            case "Neptune":
                return "Diameter: 49,244 km\nDistance from Sun: 4.5B km\nMoons: 16\nOrbital Period: 165 years\nFun Fact: Neptune has the strongest winds in the solar system!";
            case "Pluto":
                return "Diameter: 2,377 km\nDistance from Sun: 5.9B km\nMoons: 5 (Charon, Nix, Hydra, Kerberos, Styx)\nOrbital Period: 248 years\nFun Fact: Pluto has a heart-shaped glacier called Tombaugh Regio!";
            default:
                return "";
        }
    }
}