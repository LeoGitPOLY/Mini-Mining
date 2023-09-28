using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem 
{ 
    //SCORE:
    public static void saveScore(ScoreManager score)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/score.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreDataV2 data = new ScoreDataV2(score);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static ScoreDataV2 loadScore()
    {
        string path = Application.persistentDataPath + "/score.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreDataV2 score = formatter.Deserialize(stream) as ScoreDataV2;
            stream.Close();

            return score;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static ScoreData loadScoreV1()
    {
        string path = Application.persistentDataPath + "/score.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreData score = formatter.Deserialize(stream) as ScoreData;
            stream.Close();

            return score;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //PLAYER:
    public static void savePlayer(PlayerManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData loadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";

        //if (!File.Exists(path))
        //{
        //    path = "/storage/emulated/0/Android/data/com.Akakamakstudio.MiniMining/files/player.save";
        //}
        //if (!File.Exists(path))
        //{
        //    path = "/storage/emulated/O/Android/data/com.Akakamakstudio.MiniMining/files/player.save";
        //}
        //if (!File.Exists(path))
        //{
        //    path = "/Android/data/com.Akakamakstudio.MiniMining/files/player.save";
        //}


        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData player = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
       
            return player;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //MONDE:
    public static void saveMonde(string[,] tableauMonde, string[,] tableauExploration)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/monde.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        MondeData data = new MondeData(tableauMonde, tableauExploration);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static MondeData loadMonde()
    {
        string path = Application.persistentDataPath + "/monde.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            MondeData monde = formatter.Deserialize(stream) as MondeData;
            stream.Close();

            return monde;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //IMPROUVEMENTS:
    public static void saveImprouvements(ImprouvementManager improuv)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/improuvements.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        ImprouvementsData data = new ImprouvementsData(improuv);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static ImprouvementsData loadImprouvements()
    {
        string path = Application.persistentDataPath + "/improuvements.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ImprouvementsData improuv = formatter.Deserialize(stream) as ImprouvementsData;
            stream.Close();

            return improuv;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    
    //TIME:
    public static void saveTime(TimeManager time)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/time.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        TimerData data = new TimerData(time);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static TimerData loadTime()
    {
        string path = Application.persistentDataPath + "/time.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TimerData time = formatter.Deserialize(stream) as TimerData;
            stream.Close();

            return time;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //Sound:
    public static void saveSound(AudioManager audio)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/audio.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SoundData data = new SoundData(audio);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SoundData loadSound()
    {
        string path = Application.persistentDataPath + "/audio.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SoundData data = formatter.Deserialize(stream) as SoundData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //Settings:
    public static void saveSettings(SettingManager settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/setting.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsDataV2 data = new SettingsDataV2(settings);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SettingsDataV2 loadSettings()
    {
        string path = Application.persistentDataPath + "/setting.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsDataV2 data = formatter.Deserialize(stream) as SettingsDataV2;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static SettingsData loadSettingsV1()
    {
        string path = Application.persistentDataPath + "/setting.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    //Version:
    public static void saveVersion(VersionManager versionManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/version.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        VersionData data = new VersionData(versionManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static VersionData loadVersion()
    {
        string path = Application.persistentDataPath + "/version.save";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            VersionData data = formatter.Deserialize(stream) as VersionData;
            stream.Close();

            return data;
        }
        else
        {            
            Debug.LogError("Save file not found in " + path);

            return null;
        }
    }
}
