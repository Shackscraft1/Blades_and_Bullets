using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpSliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;


    private float duration = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FillAbilityProgressBar());

    }
    
    
    IEnumerator FillAbilityProgressBar()
    {
        while (duration >= 0f)
        {
            yield return new WaitForSeconds(1f);
            duration -= .1f;
            slider.value = duration;
        }
        
        
    }
}
