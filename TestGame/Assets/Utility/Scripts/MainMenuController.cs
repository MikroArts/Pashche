using Newtonsoft.Json;
using QuestSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject loadingImage;
    public Slider slider;
    public Text loadingText;
    public GameObject player;
    PlayerStats statsForSave;
    public GameObject optionsMenu;
    
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void MuteAllSounds(bool muteSound)
    {
        if (muteSound)
        {
            audioMixer.SetFloat("volume", -80f);
        }
        else
        {
            audioMixer.SetFloat("volume", 0f);
        }
        
    }
    public void StartGame(int levelIndex)
    {
        StartCoroutine(LoadLevelAsync(levelIndex));        
    }
    public void QuitGame()
    {        
        Application.Quit();
    }
    public void ManiMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;        
    }
    public void OpenOptions()
    {
        if (optionsMenu.activeSelf)
            optionsMenu.SetActive(false);
        else            
            optionsMenu.SetActive(true);
    }
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }    
    IEnumerator LoadLevelAsync(int levelIndex)
    {        
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        loadingImage.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = 1f - progress;

            loadingText.text = Mathf.Round(progress * 100f).ToString() + "%";

            yield return null;
        }
    }    
}
