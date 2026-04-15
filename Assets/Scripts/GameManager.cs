using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI clueText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI counterText;

    [Header("Slots")]
    public GameObject[] slots;

    private List<GameObject> remainingSlots = new List<GameObject>();
    private GameObject currentActiveSlot;

    void Start()
    {
        foreach (GameObject slot in slots)
            remainingSlots.Add(slot);

        feedbackText.text = "";
        counterText.text = "0 / 9 Planets Placed";
        PickNextSlot();
    }

    public void PickNextSlot()
    {
        int placed = 9 - remainingSlots.Count;
        counterText.text = placed + " / 9 Planets Placed";

        if (remainingSlots.Count == 0)
        {
            infoPanel.SetActive(true);
            feedbackText.color = Color.yellow;
            clueText.text = "Congratulations!\nYou have placed all the planets correctly!";
            feedbackText.text = "The Solar System is complete!";
            return;
        }

        int randomIndex = Random.Range(0, remainingSlots.Count);
        currentActiveSlot = remainingSlots[randomIndex];

        string slotName = currentActiveSlot.name;
        PlanetData[] allPlanets = FindObjectsOfType<PlanetData>();
        foreach (PlanetData planet in allPlanets)
        {
            if (planet.correctSlotName == slotName)
            {
                clueText.text = "Find this planet:\n\n" + planet.clue;
                break;
            }
        }

        infoPanel.SetActive(true);
    }

    public void OnCorrectPlanetPlaced(GameObject slot)
    {
        remainingSlots.Remove(slot);
        feedbackText.color = Color.green;
        feedbackText.text = "Correct! Well done!";
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