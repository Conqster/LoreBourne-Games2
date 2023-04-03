using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace LoreBourne
{
    public interface ICharacterAnimation
    {
        void AimingWeaponLayer(bool aim);
        void ReloadGun();
        void ShootingGun(float fireRate);
    }
}

