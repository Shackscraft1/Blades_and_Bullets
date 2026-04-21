using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private Image abilityBarImage;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    
    public static EventHandler AbilityActiveStatus; 
    [SerializeField] private Slider hpSlider;
    private float duration = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        duration -= .1f;
        hpSlider.value = duration;
    }

    private void ScoreChange(int scoreText)
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
    
    
}
