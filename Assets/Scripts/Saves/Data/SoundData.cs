using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public int indexMusicAt;
    public float timeMusicAt;

    public SoundData(AudioManager timeManager)
    {
        indexMusicAt = timeManager.IndexLecture;
        timeMusicAt = timeManager.TimeLecture;
    }
}
