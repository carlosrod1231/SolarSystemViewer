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
    public GameObject restartButton;

    [Header("Slots")]
    public GameObject[] slots;

    [Header("Sounds")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip winSound;
    public AudioClip backgroundMusic;

    [Range(0f, 1f)] public float musicVolume = 0.3f;
    [Range(0f, 1f)] public float sfxVolume = 0.2f;

    private List<GameObject> remainingSlots = new List<GameObject>();
    private GameObject currentActiveSlot;
    private AudioSource sfxSource;
    private AudioSource musicSource;

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

        if (restartButton != null)
            restartButton.SetActive(false);

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
            clueText.text = "Congratulations!\nYou have placed all the planets correctly!\n\nHead to the Welcome Panel to play again!";
            feedbackText.text = "The Solar System is complete!";
            if (winSound != null) sfxSource.PlayOneShot(winSound, sfxVolume);
            musicSource.volume = 0.1f;
            if (restartButton != null)
                restartButton.SetActive(true);
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}