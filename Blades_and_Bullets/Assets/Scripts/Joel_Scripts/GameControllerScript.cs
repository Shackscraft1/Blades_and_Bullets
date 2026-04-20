using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField] private Image abilityBarImage;
    public static EventHandler AbilityActiveStatus; 
    [SerializeField] private Slider hpSlider;
    private float duration = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilityBarImage.type = Image.Type.Filled;
        abilityBarImage.fillMethod = Image.FillMethod.Vertical;
        abilityBarImage.fillAmount = 0f;
        Player.ModifyAbilityCooldown +=ModifyAbilityCooldown;
        StartCoroutine(FillAbilityProgressBar());

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
    
    IEnumerator FillAbilityProgressBar()
    {
        while (duration >= 0f)
        {
            yield return new WaitForSeconds(1f);
            duration -= .1f;
            hpSlider.value = duration;
        }
        
        
    }
}
