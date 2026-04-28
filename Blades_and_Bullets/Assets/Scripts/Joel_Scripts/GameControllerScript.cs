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
        throw new NotImplementedException();
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

    private void ScoreChange(int scoreText)
    {
        
        currentScoreText.text =  ($"Score: {scoreText.ToString("D4")}");
        highScoreText.text =  ($"HighScore: {scoreText.ToString("D4")}");
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
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        SaveScore();
    }

    public void SaveScore()
    {
        
    }
    
}
