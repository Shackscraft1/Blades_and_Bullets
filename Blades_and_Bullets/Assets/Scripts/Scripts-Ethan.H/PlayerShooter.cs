using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private BaseBulletPattern bulletPattern;

    private float timer;
    private float timer2;

    private void Start()
    {
        Player.PlayerFiresBullet += PlayerFiresBullet;


    }

    private void PlayerFiresBullet(object sender, EventArgs e)
    {
        timer += Time.deltaTime;


        if (timer >= fireRate)
        {

            timer = 0f;

            bulletPattern.FirePattern();
        }
    }

    public float getFireRate(int fireRate)
    {
        return fireRate;
    }

    // Update is called once per frame
    private void Update()
    {


        if (bulletPattern == null)
        {

            return;
        }
      

       


    }
}
