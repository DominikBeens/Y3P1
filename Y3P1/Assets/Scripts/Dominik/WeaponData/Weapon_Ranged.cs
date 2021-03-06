﻿using UnityEngine;

[System.Serializable]
public class Weapon_Ranged : Weapon
{

    [Header("Primary Attack")]
    public string primaryProjectile = "Arrow";
    [Space(10)]
    public float force = 20;
    public int amountOfProjectiles = 1;
    public int coneOfFireInDegrees = 0;

    public override string[] RangedInfo()
    {
        string[] rI = new string[] {"Primary:", "Arrows: <color=#00A8FF>" + amountOfProjectiles.ToString() };
        return rI;
    }

    public override void StartRanged(float fP, int aP, int dP)
    {
        force = fP;
        amountOfProjectiles = aP;
        coneOfFireInDegrees = dP;
    }
    public override bool IsRanged()
    {
        return true;
    }
}
