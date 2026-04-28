using System;
using System.Collections.Generic;
using UnityEngine;

public class SavedDataJSON : MonoBehaviour
{
    private Dictionary<string, int> HighScore;
    public static EventHandler<OnHighScoreDataGatheredArgs> OnHighScoreDataGathered;

    public class OnHighScoreDataGatheredArgs : EventArgs
    {
        public int highScore;
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void onDestroy()
    {
        
    }

    
}
