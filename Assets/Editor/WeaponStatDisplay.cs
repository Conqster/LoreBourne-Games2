using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponBehaviour))]
public class WeaponStatDisplay : Editor
{
    private void OnSceneGUI()
    {
        WeaponBehaviour weaponBehaviour = (WeaponBehaviour)target;
        if (weaponBehaviour == null)
            return;



        int bulletLeft = weaponBehaviour.BulletLeft();
        int magLeft = weaponBehaviour.MagLeft();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.blue;
        Handles.Label(weaponBehaviour.transform.position + new Vector3(0,2,0), ("bullet left: " + bulletLeft.ToString() + "\n mag left: " + magLeft.ToString()),style);


    }
}
