using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI clueText;
    public TextMeshProUGUI feedbackText;

    [Header("Slots")]
    public GameObject[] slots;

    private List<GameObject> remainingSlots = new List<GameObject>();
    private GameObject currentActiveSlot;

    void Start()
    {
        // Add all slots to the remaining list
        foreach (GameObject slot in slots)
            remainingSlots.Add(slot);

        feedbackText.text = "";
        PickNextSlot();
    }

    public void PickNextSlot()
    {
        if (remainingSlots.Count == 0)
        {
            clueText.text = "Congratulations! You placed all the planets!";
            infoPanel.SetActive(true);
            return;
        }

        // Pick a random slot from remaining
        int randomIndex = Random.Range(0, remainingSlots.Count);
        currentActiveSlot = remainingSlots[randomIndex];

        // Find which planet belongs here
        string slotName = currentActiveSlot.name;
        PlanetData[] allPlanets = FindObjectsOfType<PlanetData>();
        foreach (PlanetData planet in allPlanets)
        {
            if (planet.correctSlotName == slotName)
            {
                clueText.text = planet.clue;
                break;
            }
        }

        infoPanel.SetActive(true);
    }

    public void OnCorrectPlanetPlaced(GameObject slot)
    {
        remainingSlots.Remove(slot);
        feedbackText.color = Color.green;
        feedbackText.text = "Correct!";
        Invoke("ClearFeedback", 2f);
        PickNextSlot();
    }

    public void OnWrongPlanetPlaced()
    {
        feedbackText.color = Color.red;
        feedbackText.text = "Wrong planet! Try again.";
        Invoke("ClearFeedback", 2f);
    }

    void ClearFeedback()
    {
        feedbackText.text = "";
    }
}