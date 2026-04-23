using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance {get; private set;}
    [SerializeField] private Image abilityBarImage;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    
    public static EventHandler AbilityActiveStatus; 
    [SerializeField] private Slider hpSlider;
    private float _currentPlayerHp = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        abilityBarImage.type = Image.Type.Filled;
        abilityBarImage.fillMethod = Image.FillMethod.Vertical;
        abilityBarImage.fillAmount = 0f;
        hpSlider.value = 1f;
        Player.ModifyAbilityCooldown +=ModifyAbilityCooldown;
        Player.PlayerGetsHit += PlayerGetsHit;

    }

    void OnDestroy()
    {
        Player.ModifyAbilityCooldown -=ModifyAbilityCooldown;
        Player.PlayerGetsHit -= PlayerGetsHit;
    }

    private void PlayerGetsHit(object sender, EventArgs e)
    {
        _currentPlayerHp -= .05f;
        hpSlider.value = _currentPlayerHp;
        if (_currentPlayerHp <= 0f) HpDropsToZero();
        
    }

    private void ScoreChange(float scoreText)
    {
        
        currentScoreText.text =  ($"Score\n  {scoreText.ToString("D4")}");
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
        
    }
    
}
