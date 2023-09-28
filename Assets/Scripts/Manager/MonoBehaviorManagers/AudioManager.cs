using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float TIME_BETWEEN_CLIPS;

    private AudioSource audioSource;
    private float TimeClipTotal;
    private const float MAX_VOLUME = 0.9f;

    [Header("OnlyReading")]
    public int IndexLecture;
    public float TimeLecture;

    private bool isChanching;
    private bool isPlaying;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = clips[IndexLecture];
        audioSource.time = TimeLecture;
        audioSource.Play();

        TimeClipTotal = clips[IndexLecture].length;

        isChanching = false;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeClipTotal - (TIME_BETWEEN_CLIPS / 4f) < audioSource.time && !isChanching)
        {
            Invoke("changeSound", (TIME_BETWEEN_CLIPS / 4f) * 5f);
            isChanching = true;
        }
    }

    private void changeSound()
    {
        IndexLecture = (IndexLecture + 1) % clips.Length;

        audioSource.clip = clips[IndexLecture];
        audioSource.time = 0;
        audioSource.Play();

        TimeClipTotal = clips[IndexLecture].length;

        isChanching = false;
    }

    public void changeVolume(float volume)
    {
        audioSource.volume = volume * MAX_VOLUME;
    }

    public void stopMusic()
    {
        if (isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(fadeOut());

            if (!isChanching)
                TimeLecture = audioSource.time;
            else
            {
                IndexLecture = (IndexLecture + 1) % clips.Length;
                TimeLecture = 0;
            }
            isPlaying = false;
        }
    }

    public void restartMusic()
    {
        if (!isPlaying)
        {
            StopAllCoroutines();
            StartCoroutine(fadeIn());

            audioSource.clip = clips[IndexLecture];
            audioSource.time = TimeLecture;
            audioSource.Play();

            TimeClipTotal = clips[IndexLecture].length;

            isPlaying = true;
        }
    }

    private IEnumerator fadeIn()
    {
        float volumeTarget = audioSource.volume;
        float volumeCurent = 0;

        audioSource.volume = volumeCurent;

        while (audioSource.volume < volumeTarget)
        {
            audioSource.volume = LeanSmooth.linear(audioSource.volume, volumeTarget, 0.7f);
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.volume = SettingManager.instance.BackGroundVolume;
    }
    private IEnumerator fadeOut()
    {
        float volumeTarget = 0;

        while (audioSource.volume > volumeTarget)
        {
            audioSource.volume = LeanSmooth.linear(audioSource.volume, volumeTarget, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        audioSource.Stop();
        audioSource.volume = SettingManager.instance.BackGroundVolume;
    }

    //Save methode:
    public void saveSoundProgression()
    {


        SaveSystem.saveSound(instance);
    }
    public void loadSoundProgression()
    {
        SoundData data = SaveSystem.loadSound();
        if (data != null)
        {
            IndexLecture = data.indexMusicAt;
            TimeLecture = data.timeMusicAt;
        }
        else
        {
            loadNew();
        }
    }
    public void loadNew()
    {
        IndexLecture = 0;
        TimeLecture = 0;
    }

}
