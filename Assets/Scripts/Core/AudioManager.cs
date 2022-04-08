using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private class Sound {
        [SerializeField]
        public string name;
        [SerializeField]
        public AudioClip clip;
        [Range (0f, 1f)] [SerializeField]
        public float volume = 1f;
        [SerializeField]
        public float pitch = 1f;
        [SerializeField]
        public bool loop = false;

    [HideInInspector]
        public AudioSource source;
    }


    [SerializeField]
    private Sound[] music;
    [SerializeField]
    private Sound[] sfx;
    //public double bpm = 90;
    
    //public phase currentPhase = phase.MENU;
    //public phase nextPhase;
    //private double spb;
    //private double phaseDuration;
    //private double phaseTime = 0;
    //private double nextLoopTime = 0;

    [SerializeField] float fadeTime = 4f;
    public enum theme {ZERO, BLOOD, MOMENT, BIG};
    theme currentTheme = theme.MENU;
    [SerializeField] theme nextTheme = theme.MENU;



    public static AudioManager Instance { get; private set; }

           
    void Awake () {

         if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = true;
        }
        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = false;
        }
        // PlayMusic("main", 1f);
        // PlayMusic("second", 0f);
        PlayMusic("main", 1f);
        PlayMusic("second", 0f);
        PlayMusic("menu", 1f);
        PlayMusic("menu", 0f);



        //StartCoroutine(Crossfade("main", "second", fadeTime));
        //phaseDuration = 60 / bpm * 4 * 4; //4 bars
        //nextLoopTime = phaseDuration;
    }

    void Update () {
/*
        if(AudioSettings.dspTime >= nextLoopTime) {
            StartCoroutine(Crossfade("main", "second", 4));
        }
        */
        if(nextTheme != currentTheme) {
            StartCoroutine(Crossfade(currentTheme, nextTheme, fadeTime));
            currentTheme = nextTheme;
        }

    }
    

    public IEnumerator Crossfade(theme current, theme next, float time) {
        string currentName;
        switch(current) {
            case theme.BATTLE: currentName = "second";
                break;
            default: currentName = "main";
                break;
        }
        string nextName;
        switch(next) {
            case theme.BATTLE: nextName = "second";
                break;
            default: nextName = "main";
                break;
        }
        AudioSource currentSource = getSource(currentName);
        AudioSource nextSource = getSource(nextName);

        float increment = 1f / (time * 60f);

        for (float volume = 0f; volume < 1f; volume += increment)
        {
            currentSource.volume = 1f - volume;
            nextSource.volume = volume;
            yield return null;
        }
   

    }

    private string getName(theme theme) {
        switch(theme) {
            case theme.ZERO: return "zero";
                break;
            case theme.BLOOD: return "blood";
                break;
            case theme.MOMENT: return "moment";
                break;
            case theme.BIG: return "big";
                break;
            default: return "";
                break;
        }
    }

    public void PlaySFX(string name) {
        getSFXSource(name).Play();
    }

    private AudioSource getSFXSource(string name) {
        int index = Random.Range(1, 4);

        foreach(Sound s in sfx) {
            if(s.name == name + index || s.name == name) {
                return s.source;            
            }

        }
        foreach(Sound s in sfx) {
            if(s.name == name + "1") {
                return s.source;            
            }

        }
        Debug.Log("Couldn't find sfx with name " + name);
        return null;
    }

    public void PlayMusic(string name, float volume) {
            AudioSource source = getSource(name);
            source.volume = volume;
            source.Play();
        }

    private AudioSource getSource(string name) {
        foreach(Sound s in music) {
            if(s.name == name) {
                return s.source;          
            }

        }
        Debug.Log("Couldn't find music with name " + name);
        return null;

    }

}
