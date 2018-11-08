﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Y3P1;

public class PlayerStatusCanvas : MonoBehaviour
{

    private HealthBar playerHealthBar;
    private float weaponSecondaryTimer;
    private float weaponSecondaryBarFill;

    [SerializeField] private Image weaponSecondaryBar;
    [SerializeField] private GameObject noSecondaryText;
    [SerializeField] private TextMeshProUGUI secondaryProgressText;

    [Space(10)]

    [SerializeField] private List<StatusEffectIcon> weaponBuffIcons = new List<StatusEffectIcon>();
    [SerializeField] private GameObject buffDescPanel;
    [SerializeField] private TextMeshProUGUI buffDescText;

    public void Start()
    {
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.Initialise(Player.localPlayer.entity);

        InitialiseEvents();

        weaponSecondaryBar.fillAmount = 0;
    }

    private void InitialiseEvents()
    {
        WeaponSlot.OnUseSecondary += WeaponSlot_OnUseSecondary;
        WeaponSlot.OnEquipWeapon += WeaponSlot_OnEquipWeapon;

        Player.localPlayer.weaponSlot.OnWeaponBuffAdded += WeaponSlot_OnWeaponBuffAdded;
        Player.localPlayer.weaponSlot.OnWeaponBuffRemoved += WeaponSlot_OnWeaponBuffRemoved;
    }

    private void WeaponSlot_OnEquipWeapon(Weapon weapon)
    {
        if (weapon != null && !string.IsNullOrEmpty(weapon.secondaryProjectile))
        {
            noSecondaryText.SetActive(false);
            secondaryProgressText.gameObject.SetActive(true);
            UpdateSecondaryProgressText();
        }
        else
        {
            noSecondaryText.SetActive(true);
            secondaryProgressText.gameObject.SetActive(false);
        }
    }

    public void Hit(bool ranged)
    {
        int multiplier = ranged ? 1 : 2;
        weaponSecondaryBarFill += ranged ? (1 / (float)WeaponSlot.hitsRequiredToSecondary * multiplier) : (1 / (float)WeaponSlot.hitsRequiredToSecondary * multiplier);
        WeaponSlot.currentHits += (1 * multiplier);
    }

    private void WeaponSlot_OnUseSecondary(Weapon.SecondaryType secondaryType)
    {
        weaponSecondaryBarFill = 0;
        weaponSecondaryBar.fillAmount = 0;
        WeaponSlot.currentHits = 0;
    }

    private void Update()
    {
        if (weaponSecondaryBar.fillAmount < 1)
        {
            UpdateSecondaryProgressText();
        }

        if (buffDescPanel.activeInHierarchy)
        {
            buffDescPanel.transform.position = Input.mousePosition;
        }
    }

    private void UpdateSecondaryProgressText()
    {
        weaponSecondaryBar.fillAmount = Mathf.Lerp(weaponSecondaryBar.fillAmount, weaponSecondaryBarFill, Time.deltaTime * 10);

        if (secondaryProgressText.gameObject.activeInHierarchy)
        {
            secondaryProgressText.text = Mathf.RoundToInt(weaponSecondaryBar.fillAmount * 100) + "%";
        }
    }

    private void WeaponSlot_OnWeaponBuffAdded(StatusEffects.StatusEffectType type, float duration)
    {
        ToggleWeaponBuffIcon(type, true, duration);
    }

    private void WeaponSlot_OnWeaponBuffRemoved(StatusEffects.StatusEffectType type)
    {
        ToggleWeaponBuffIcon(type, false);
    }

    private void ToggleWeaponBuffIcon(StatusEffects.StatusEffectType type, bool toggle, float? duration = null)
    {
        for (int i = 0; i < weaponBuffIcons.Count; i++)
        {
            if (weaponBuffIcons[i].type == type)
            {
                if (toggle)
                {
                    weaponBuffIcons[i].Activate(duration);
                }
                else
                {
                    weaponBuffIcons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void ToggleBuffDescPanel(bool toggle, string desc)
    {
        buffDescText.text = desc;
        buffDescPanel.SetActive(toggle);
    }

    private void OnDisable()
    {
        WeaponSlot.OnUseSecondary -= WeaponSlot_OnUseSecondary;
        WeaponSlot.OnEquipWeapon -= WeaponSlot_OnEquipWeapon;

        Player.localPlayer.weaponSlot.OnWeaponBuffAdded -= WeaponSlot_OnWeaponBuffAdded;
        Player.localPlayer.weaponSlot.OnWeaponBuffRemoved -= WeaponSlot_OnWeaponBuffRemoved;
    }
}
