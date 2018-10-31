﻿using System.Collections;
using UnityEngine;
using Y3P1;
using System;
using Photon.Pun;
using TMPro;

public class Teleporter : MonoBehaviourPunCallbacks
{

    [SerializeField] private Animator screenFadeAnim;
    [SerializeField] private float screenFadeDuration;
    [SerializeField] private string teleportParticle;
    [SerializeField] private TextMeshProUGUI teleportMessageText;
    [SerializeField] private float teleportMessageDuration;

    public event Action OnStartTeleport = delegate { };
    public event Action OnEndTeleport = delegate { };

    public void Teleport(Vector3 destination, string messageToShow = null)
    {
        StartCoroutine(StartTeleport(destination, messageToShow));
    }

    private IEnumerator StartTeleport(Vector3 destination, string messageToShow)
    {
        OnStartTeleport();
        photonView.RPC("SyncTeleportEffect", RpcTarget.All);
        screenFadeAnim.SetTrigger("Fade");

        yield return new WaitForSeconds(screenFadeDuration);
        Player.localPlayer.transform.position = destination;
        Player.localPlayer.playerCam.transform.position = new Vector3(Player.localPlayer.transform.position.x, Player.localPlayer.playerCam.transform.position.y, Player.localPlayer.transform.position.z);

        if (!string.IsNullOrEmpty(messageToShow))
        {
            teleportMessageText.text = messageToShow;
            teleportMessageText.enabled = true;

            yield return new WaitForSeconds(teleportMessageDuration);

            teleportMessageText.enabled = false;
        }

        screenFadeAnim.SetTrigger("Fade");
        OnEndTeleport();
    }

    [PunRPC]
    private void SyncTeleportEffect()
    {
        //ObjectPooler.instance.GrabFromPool(teleportParticle, transform.position, Quaternion.identity);
        // Activate player anim
    }
}