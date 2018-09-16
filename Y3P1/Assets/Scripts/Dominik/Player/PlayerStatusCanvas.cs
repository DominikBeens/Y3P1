﻿using UnityEngine;
using UnityEngine.UI;
using Y3P1;
using TMPro;

public class PlayerStatusCanvas : MonoBehaviour
{

    private HealthBar playerHealthBar;
    private float weaponSecondaryTimer;

    [SerializeField] private Image weaponSecondaryBar;
    [SerializeField] private GameObject localPlayerInfoPanel;
    [SerializeField] private TextMeshProUGUI localPlayerNameText; 
    [SerializeField] private TextMeshProUGUI localPlayerHealthText;

    private void Start()
    {
        playerHealthBar = GetComponentInChildren<HealthBar>();
        playerHealthBar.Initialise(Player.localPlayer.entity);
        weaponSecondaryTimer = 1;

        WeaponSlot.OnUseSecondary += WeaponSlot_OnUseSecondary;
    }

    private void WeaponSlot_OnUseSecondary()
    {
        weaponSecondaryTimer = 0;
    }

    private void Update()
    {
        if (WeaponSlot.currentWeapon)
        {
            weaponSecondaryTimer = (weaponSecondaryTimer < 1) ? weaponSecondaryTimer += 1 / WeaponSlot.currentWeapon.secondaryFireRate * Time.deltaTime : 1;
            weaponSecondaryBar.fillAmount = weaponSecondaryTimer;
        }
        else
        {
            weaponSecondaryBar.fillAmount = 0;
        }

        if (localPlayerInfoPanel.activeInHierarchy)
        {
            localPlayerInfoPanel.transform.position = Input.mousePosition;
        }
    }

    public void TogglePlayerInfoPanel(bool b)
    {
        if (b)
        {
            localPlayerNameText.text = Player.localPlayer.photonView.Owner.NickName;
            localPlayerHealthText.text = "HP: " + Player.localPlayer.entity.health.GetHealthString();
        }

        localPlayerInfoPanel.SetActive(b);
    }

    private void OnDisable()
    {
        WeaponSlot.OnUseSecondary -= WeaponSlot_OnUseSecondary;
    }
}