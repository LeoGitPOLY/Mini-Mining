using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class fxSound
{
    public AudioClip[] audioClip;
    public string name;
    [Range(0, 1)] public float volume;
}
public class FxSoundManager : MonoBehaviour
{
    public static FxSoundManager instance;

    //All soundFx
    [SerializeField] private fxSound[] audioClips;
    private List<AudioSource> audioSources;
    private int currentIndex;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }
    void Start()
    {
        currentIndex = -1;
        if (audioSources == null)
            generateAudioSources();
    }

    private void generateAudioSources()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < audioClips.Length; i++)
        {
            GameObject localGame = new GameObject(audioClips[i].name);
            localGame.transform.parent = transform;
            localGame.AddComponent<AudioSource>();

            AudioSource source = localGame.GetComponent<AudioSource>();
            source.loop = true;
            audioSources.Add(source);
        }
    }


    public IEnumerator changeSound(string nameToPlay, int indexSound = 0, bool aleatoire = false, bool isfadeIn = false, bool isfadeOut = false)
    {
        if (currentIndex != -1)
        {
            stopSound(audioClips[currentIndex].name, isfadeOut);

            yield return new WaitForSeconds(2.5f);
        }

        playClip(nameToPlay, indexSound, aleatoire, isfadeIn);

        yield return null;
    }

    public void playClip(string nameToPlay, int indexSound = 0, bool aleatoire = false, bool isfadeIn = false)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            fxSound sound = audioClips[i];
            if (sound.name == nameToPlay)
            {
                audioSources[i].clip = sound.audioClip[indexSound];
                currentIndex = i;

                if (aleatoire)
                {
                    float aleaTime = Random.Range(0, audioSources[i].clip.length - 1);
                    audioSources[i].time = aleaTime;
                }
                if (isfadeIn)
                {
                    StopAllCoroutines();
                    StartCoroutine(fadeIn(audioSources[i], i));
                }
                else
                {
                    audioSources[i].Play();
                }
            }
        }
    }
    public void stopSound(string nameToPlay, bool isfadeOut = false)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            fxSound sound = audioClips[i];
            if (sound.name == nameToPlay)
            {
                if (isfadeOut)
                {
                    StopAllCoroutines();
                    StartCoroutine(fadeOut(audioSources[i], i));
                }
                else
                {
                    audioSources[i].Stop();
                }
            }
        }
    }
    public void changeGeneralVolume(float volumeGeneral)
    {
        if (audioSources == null)
            generateAudioSources();

        for (int i = 0; i < audioClips.Length; i++)
        {
            fxSound sound = audioClips[i];
            audioSources[i].volume = sound.volume * volumeGeneral;
        }
    }

    //FADE IN AND OUT:
    private IEnumerator fadeIn(AudioSource source, int index)
    {
        float volumeTarget = source.volume;
        float volumeCurent = 0;

        source.volume = volumeCurent;
        source.Play();

        while (source.volume < volumeTarget)
        {
            source.volume = LeanSmooth.linear(source.volume, volumeTarget, 0.7f);
            yield return new WaitForSeconds(0.1f);
        }
        source.volume = audioClips[index].volume * SettingManager.instance.SoundEffetsVolume;
    }
    private IEnumerator fadeOut(AudioSource source, int index = 0)
    {
        float volumeTarget = 0;

        while (source.volume > volumeTarget)
        {
            source.volume = LeanSmooth.linear(source.volume, volumeTarget, 1.2f);
            yield return new WaitForSeconds(0.1f);
        }
        source.Stop();
        source.volume = audioClips[index].volume * SettingManager.instance.SoundEffetsVolume;
    }
}