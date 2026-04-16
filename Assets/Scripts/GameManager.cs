using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject infoPanel;
    public TextMeshProUGUI clueText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI counterText;
    public GameObject clueLabel;

    [Header("Slots")]
    public GameObject[] slots;

    [Header("Sounds")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip winSound;
    public AudioClip backgroundMusic;

    [Range(0f, 1f)] public float musicVolume = 0.3f;
    [Range(0f, 1f)] public float sfxVolume = 0.4f;

    private List<GameObject> remainingSlots = new List<GameObject>();
    private GameObject currentActiveSlot;
    private AudioSource sfxSource;
    private AudioSource musicSource;
    private SunInteraction sunInteraction;
    private SolarSystemRise solarSystemRise;

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            sfxSource = sources[0];
            musicSource = sources[1];
        }
        else
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;

        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }

        foreach (GameObject slot in slots)
            remainingSlots.Add(slot);

        sunInteraction = FindObjectOfType<SunInteraction>();
        solarSystemRise = GetComponent<SolarSystemRise>();

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
            clueText.text = "Congratulations!\nYou placed all the planets correctly!\n\nThe solar system is rising!\n\nClick the BLACK HOLE to lower the galaxy.\nClick the COMET to raise it again.\n\nLower the galaxy and click the SUN to restart!";
            feedbackText.text = "The Solar System is complete!";
            if (clueLabel != null) clueLabel.SetActive(false);
            if (winSound != null) sfxSource.PlayOneShot(winSound, sfxVolume);
            musicSource.volume = 0.1f;
            if (sunInteraction != null) sunInteraction.SetGameWon();
            if (solarSystemRise != null) solarSystemRise.ActivateWinState();
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
        if (correctSound != null) sfxSource.PlayOneShot(correctSound, sfxVolume);
        Invoke("ClearFeedback", 2f);
        PickNextSlot();
    }

    public void OnWrongPlanetPlaced()
    {
        feedbackText.color = Color.red;
        feedbackText.text = "Wrong planet! Try again.";
        if (wrongSound != null) sfxSource.PlayOneShot(wrongSound, sfxVolume);
        Invoke("ClearFeedback", 2f);
    }

    void ClearFeedback()
    {
        feedbackText.text = "";
    }
}