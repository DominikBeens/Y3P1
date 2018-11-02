﻿using Photon.Pun;
using UnityEngine;
using Y3P1;

public class TrinketPrefab : ItemPrefab
{

    private ParticleSystem.MainModule particleMain;
    //private Material mat;

    //[SerializeField] private ParticleSystem particle;

    protected override void Awake()
    {
        base.Awake();

        //particleMain = particle.main;
    }

    public override void OnEnable()
    {
        Player.localPlayer.trinketSlot.OnEquip += TrinketSlot_OnEquip;
    }

    private void TrinketSlot_OnEquip()
    {
        SetColors();
        renderer.enabled = false;
        //particle.gameObject.SetActive(true);
    }

    [PunRPC]
    private void SyncDropData(byte[] itemData)
    {
        ByteObjectConverter boc = new ByteObjectConverter();
        myItem = (Item)boc.ByteArrayToObject(itemData);

        isDropped = true;

        interactCollider.SetActive(true);
        objectCollider.enabled = true;

        transform.eulerAngles += dropRotationAdjustment;
        transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0), Space.World);

        SpawnDroppedItemLabel();

        renderer.enabled = true;
        Player.localPlayer.trinketSlot.OnEquip -= TrinketSlot_OnEquip;

        //DroppedItemManager.instance.RegisterDroppedItem(photonView.ViewID, myItem);
    }

    [PunRPC]
    private void PickUpDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }

        droppedItemLabel.anim.SetTrigger("Pickup");
    }

    private void SetColors()
    {
        //if (!string.IsNullOrEmpty(TrinketSlot.currentTrinket.color))
        //{
        //    particleMain.startColor = GetColor((myItem as Trinket).color);
        //    mat.color = particleMain.startColor.color;
        //}
    }

    private Color GetColor(string colorString)
    {
        Color color = Color.white;

        switch (colorString)
        {
            case "Blue":

                color = Color.blue;
                break;
            case "Cyan":

                color = Color.cyan;
                break;
            case "Green":

                color = Color.green;
                break;
            case "Magenta":

                color = Color.magenta;
                break;
            case "Red":

                color = Color.red;
                break;
            case "Yellow":

                color = Color.yellow;
                break;

        }

        return color;
    }

    public override void OnDisable()
    {
        Player.localPlayer.trinketSlot.OnEquip -= TrinketSlot_OnEquip;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //base.OnPhotonSerializeView(stream, info);

        //if (stream.IsWriting)
        //{
        //    stream.SendNext(renderer.enabled);
        //}
        //else
        //{
        //    renderer.enabled = (bool)stream.ReceiveNext();
        //}
    }
}
