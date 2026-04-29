using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private Image abilityBarImage;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    
    public static EventHandler AbilityActiveStatus; 
    public static EventHandler OnPlayerDeath; 
    [SerializeField] private Slider hpSlider;
    private float _currentPlayerHp = 1f;
    private int _currentScore = 0;
    private int _HighScore = 0;
    
    public static EventHandler<OnHighScoreDataGatheredArgs> OnNewHighScoreChange;

    public class OnHighScoreDataGatheredArgs : EventArgs
    {
        public int newHighScore;
    }
    private enum HighScoreAchieved
    {
        NoHighScore,
        NewHighScore
    }
    private HighScoreAchieved _highSoreState = HighScoreAchieved.NoHighScore;
  
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityBarImage.type = Image.Type.Filled;
        abilityBarImage.fillMethod = Image.FillMethod.Vertical;
        abilityBarImage.fillAmount = 0f;
        hpSlider.value = 1f;
        Player.ModifyAbilityCooldown +=ModifyAbilityCooldown;
        Player.PlayerGetsHit += PlayerGetsHit;
        SlashScript.OnSlashingSomething += OnSlashingSomething;
        SavedDataJSON.OnHighScoreDataGathered +=OnHighScoreDataGathered;

    }

    private void OnHighScoreDataGathered(object sender, SavedDataJSON.OnHighScoreDataGatheredArgs e)
    {
        _HighScore = e.highScore;
        highScoreText.text =  ($"HighScore: {_HighScore.ToString("D6")}");
    }

    private void OnSlashingSomething(object sender, EventArgs e)
    {
        _currentScore += 100;
        ScoreChange(_currentScore);
    }

    void OnDestroy()
    {
        Player.ModifyAbilityCooldown -=ModifyAbilityCooldown;
        Player.PlayerGetsHit -= PlayerGetsHit;
        SlashScript.OnSlashingSomething -= OnSlashingSomething;
        SavedDataJSON.OnHighScoreDataGathered -=OnHighScoreDataGathered;
    }

    private void PlayerGetsHit(object sender, EventArgs e)
    {
        _currentPlayerHp -= .05f;
        hpSlider.value = _currentPlayerHp;
        if (_currentPlayerHp <= 0f) HpDropsToZero();
        
    }

    private void ScoreChange(int scoreChange)
    {
        currentScoreText.text =  ($"Score: {scoreChange.ToString("D6")}");
        if (scoreChange >= _HighScore)
        {
            _HighScore = scoreChange;
            highScoreText.text =  ($"HighScore: {_HighScore.ToString("D6")}");
            _highSoreState =  HighScoreAchieved.NewHighScore;
        }
    }

    private void ModifyAbilityCooldown(object sender, Player.ModifyAbilityCooldownArgs e)
    {
        if (e.changeAmount > 0f)
        {
            abilityBarImage.fillAmount += e.changeAmount;
            if (abilityBarImage.fillAmount >= 1f) AbilityActiveStatus?.Invoke(this, EventArgs.Empty);
            
        }
        else abilityBarImage.fillAmount = e.changeAmount;
    }

    private void HpDropsToZero()
    {
        if(_highSoreState.Equals(HighScoreAchieved.NewHighScore)) OnNewHighScoreChange?.Invoke(this, new OnHighScoreDataGatheredArgs{newHighScore = _HighScore});
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }
    
}
