using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterAudioSlider;
    public Slider bgmAudioSlider;
    public Slider effectAudioSlider;

    public AudioSource bgmAudio;
    public AudioSource actionAudio;
    public AudioSource arrowAudio;
    public AudioSource bombAudio;
    public AudioSource explosionAudio;
    public AudioSource magicAudio;
    //float volume = audioSlider.value;
    //audioMixer.SetFloat(string parameter, volume); 

    public static SoundManager sm;

    void Awake()
    {
        if (sm == null) sm = this;
    }

    public AudioClip defaultBGM;
    public AudioClip dangerBGM;
    public AudioClip clickSound;
    public AudioClip menuSound;
    public AudioClip buildSound;

    void Start()
    {
        DefaultBGM();
    }

    public void MasterAudioControl()
    {
        float volume = masterAudioSlider.value;
        audioMixer.SetFloat("Master", volume);
    }

    public void BGMAudioControl()
    {
        float volume = bgmAudioSlider.value;
        audioMixer.SetFloat("BGM", volume);
    }

    public void EffectAudioControl()
    {
        float volume = effectAudioSlider.value;
        audioMixer.SetFloat("Effect", volume);
    }

    public void AudioMute()
    {
        AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
    }
    
    public void ClickAudio()
    {
        actionAudio.clip = clickSound;
        actionAudio.Play();
    }

    public void BuildAudio()
    {
        actionAudio.clip = buildSound;
        actionAudio.Play();
    }

    public void MenuAudio()
    {
        actionAudio.clip = menuSound;
        actionAudio.Play();
    }

    public void ArrowAudio()
    {
        if (!arrowAudio.isPlaying)
            arrowAudio.Play();
    }

    public void BombAudio()
    {
        //if (!bombAudio.isPlaying)
            bombAudio.Play();
    }

    public void ExplosionAudio()
    {
        //if (!explosionAudio.isPlaying)
            explosionAudio.Play();
    }

    public void MagicWhooshAudio()
    {
        //if (!magicAudio.isPlaying)
            magicAudio.Play();
    }

    public void DefaultBGM()
    {
        //if(bgmAudio.isPlaying)
            bgmAudio.Stop();

        bgmAudio.clip = defaultBGM;
        bgmAudio.Play();
    }

    public void DangerBGM()
    {
        if (bgmAudio.isPlaying)
            bgmAudio.Stop();

        bgmAudio.clip = dangerBGM;
        bgmAudio.Play();
    }
}
