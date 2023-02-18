using InventoryExample.UI;
using RPG.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : ShowHideUI
{
    public static bool GameIsPaused = false;

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
        print(volume);
    }
    public void Load()
    {
        print("Press load");
    }
    public void Save()
    {
        print("Press save");
    }
}
