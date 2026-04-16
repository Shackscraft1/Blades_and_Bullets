using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBarProgress : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private float duration = 0f;
    

    private class OndeathArgs : EventArgs
    {
        private int hp;
        private int maxHp;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barImage.type = Image.Type.Filled;
        barImage.fillMethod = Image.FillMethod.Vertical;
        barImage.fillAmount = 0f;
        StartCoroutine(FillAbilityProgressBar());
        

    }

    IEnumerator FillAbilityProgressBar()
    {
        
        while (duration <= 1f)
        {
            Debug.Log("test");
            yield return new WaitForSeconds(1f);
            duration += .1f;
            barImage.fillAmount = duration;
           
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
