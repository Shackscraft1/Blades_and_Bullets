using System;
using System.Collections.Generic;
using System.IO;
using Unity.Properties;
using UnityEngine;

public class SavedDataJSON : MonoBehaviour
{
    [Serializable]
    private class HighScoreData
    {
          public int currentHighScore;
    }
    public static EventHandler<OnHighScoreDataGatheredArgs> OnHighScoreDataGathered;

    public class OnHighScoreDataGatheredArgs : EventArgs
    {
        public int highScore;
    }

    private static HighScoreData data;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = new HighScoreData(); 
        GameControllerScript.OnNewHighScoreChange += OnNewHighScoreChange;
        LoadHighScore();
    }

    private void OnNewHighScoreChange(object sender, GameControllerScript.OnHighScoreDataGatheredArgs e)
    {
        Debug.Log(e.newHighScore);
        data.currentHighScore = e.newHighScore;
        
        SaveHighScore(data);
    }

    private void onDestroy()
    {   
        GameControllerScript.OnNewHighScoreChange -= OnNewHighScoreChange;
    }

    private void SaveHighScore(HighScoreData newHighScore)
    {
        
        string json = JsonUtility.ToJson(newHighScore);
        using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "SavedHighScore.json"))
        {
            writer.Write(json);
        }
        
    }

    private void LoadHighScore()
    {
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SavedHighScore.json";

        if (!File.Exists(path))
        {
            data.currentHighScore = 0;
            SaveHighScore(data);
        }
        
        string json = string.Empty;
        using (StreamReader reader =
               new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SavedHighScore.json")) json = reader.ReadToEnd();
        data = JsonUtility.FromJson<HighScoreData>(json);
        
        if (data == null)
        {
            data = new HighScoreData();
            data.currentHighScore = 0;
        }
        OnHighScoreDataGathered?.Invoke(this, new OnHighScoreDataGatheredArgs{highScore = data.currentHighScore});
        
    }
}
