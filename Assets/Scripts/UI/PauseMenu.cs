using InventoryExample.UI;
using RPG.Saving;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : ShowHideUI, ISaveable
{
    public static bool GameIsPaused = false;
    public AudioMixer audioMixer;
    [SerializeField] Slider slider;

    SavingWrapper savingWrapper;
    float currentVolume=0f;
    private void Awake()
    {
        savingWrapper = GameObject.FindObjectOfType<SavingWrapper>();
    }
    private void Start()
    {
        GetUIContainer().SetActive(false);
        SetVolume(currentVolume);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GetToggleKey()))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        GetUIContainer().SetActive(!GetUIContainer().activeSelf);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    private void Pause()
    {
        GetUIContainer().SetActive(!GetUIContainer().activeSelf);
        Time.timeScale = 0f;
        GameIsPaused= true;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void Load()
    {
        savingWrapper.Load();
        Resume();
    }
    public void Save()
    {
        savingWrapper.Save();
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("quitting");
    }

    public object CaptureState()
    {
        return currentVolume;
    }

    public void RestoreState(object state)
    {
        currentVolume = (float)state;
        slider.value = currentVolume;
    }
}
