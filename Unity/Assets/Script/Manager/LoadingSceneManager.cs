using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public AudioClip nextSound;
    public Slider slider;

    AudioSource audio;
    float temp;
    float time;

    void Start()
    {
        audio = this.gameObject.AddComponent<AudioSource>();
        audio.clip = this.nextSound;
        audio.Play();

        Time.timeScale = 1;
        temp = Time.time;
        slider.value = 0;
        time = 0;
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        string sceneName =  Main.main.nextScene;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float T = Time.time - temp;
            time = T;
            slider.value = time / 3f;

            if (time > 3)   operation.allowSceneActivation = true;

            yield return null;
        }

    }
}