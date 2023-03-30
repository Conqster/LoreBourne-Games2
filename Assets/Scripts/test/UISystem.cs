using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoreBourne;
using UnityEngine.UI;
using TMPro;

public class UISystem : MonoBehaviour
{
    [Header("Values Ref")]
    [SerializeField] private PlayerBehaviour player;
    private float playerHealth;
    [SerializeField]private PlayerWeaponBehaviour playerWeapon;
    [SerializeField] private Image crossHair;
    private int bulletLeft;
    private int MagLeft;
    private int numberBulletFullMag;

    [Space][Header("UI Display")]
    [SerializeField] private TextMeshProUGUI ammunationLeft;
    [SerializeField] private Image playerHpUI;


    private void Update()
    {
        UpdateValues();
        AmmunationGuage();
    }

    public void HealthGuage(float current, float max)
    {
        float healthRatio = current / max;
        playerHpUI.fillAmount = healthRatio;
    }



    public void AmmunationGuage()
    {
        //have the total bullet
        int totalBulletLeft = bulletLeft + (numberBulletFullMag * MagLeft);
        ammunationLeft.text = bulletLeft.ToString("0") + "|" + 
            totalBulletLeft.ToString("0");
    }



    private void UpdateValues()
    {
        bulletLeft = playerWeapon.BulletLeft();
        MagLeft = playerWeapon.MagLeft();
        numberBulletFullMag = playerWeapon.FullMag();
    }


    public void CrossHairDisplay(bool enable)
    {
        crossHair.gameObject.SetActive(enable);
    }

}
