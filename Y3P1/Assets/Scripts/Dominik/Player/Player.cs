﻿using Photon.Pun;
using System;
using UnityEngine;

namespace Y3P1
{
    public class Player : MonoBehaviourPunCallbacks
    {

        public static GameObject localPlayerObject;
        public static Player localPlayer;

        public static event Action OnLocalPlayerInitialise = delegate { };

        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private Vector3 playerUISpawnOffset = new Vector3(0, 3, 0.2f);
        [SerializeField] private GameObject playerUICam;

        #region Components
        [HideInInspector] public WeaponChargeCanvas weaponChargeCanvas;
        [HideInInspector] public AnimationEventPrimaryAttack meleeAnimEvent;
        [HideInInspector] public IKControl armIK;
        [HideInInspector] public HeadTracking rangedWeaponLookAt;
        [HideInInspector] public PlayerController playerController;
        [HideInInspector] public WeaponSlot weaponSlot;
        [HideInInspector] public HelmetSlot helmetSlot;
        [HideInInspector] public TrinketSlot trinketSlot;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public PlayerCamera playerCam;
        [HideInInspector] public Entity entity;
        [HideInInspector] public Inventory myInventory;
        [HideInInspector] public DwarfAnimationsScript dwarfAnimController;
        #endregion

        private void Awake()
        {
            GatherPlayerComponents();

            if (photonView.IsMine || !PhotonNetwork.IsConnected)
            {
                localPlayerObject = gameObject;
                localPlayer = this;
            }
        }

        private void Start()
        {
            Initialise();
        }

        private void GatherPlayerComponents()
        {
            playerController = GetComponentInChildren<PlayerController>();
            weaponChargeCanvas = GetComponentInChildren<WeaponChargeCanvas>();
            rb = GetComponentInChildren<Rigidbody>();
            playerCam = GetComponentInChildren<PlayerCamera>();
            entity = GetComponentInChildren<Entity>();
            weaponSlot = GetComponentInChildren<WeaponSlot>();
            helmetSlot = GetComponentInChildren<HelmetSlot>();
            trinketSlot = GetComponentInChildren<TrinketSlot>();
            myInventory = GetComponentInChildren<Inventory>();
            dwarfAnimController = GetComponentInChildren<DwarfAnimationsScript>();
            meleeAnimEvent = GetComponentInChildren<AnimationEventPrimaryAttack>();
            armIK = GetComponentInChildren<IKControl>();
        }

        private void Initialise()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (!photonView.IsMine)
                {
                    playerCam.gameObject.SetActive(false);
                    playerUICam.SetActive(false);
                    playerController.enabled = false;
                    weaponChargeCanvas.gameObject.SetActive(false);
                    weaponSlot.enabled = false;
                    helmetSlot.enabled = false;
                    trinketSlot.enabled = false;
                    rangedWeaponLookAt.enabled = false;
                    Destroy(rb);

                    foreach (Collider col in GetComponentsInChildren<Collider>())
                    {
                        col.enabled = false;
                    }

                    CreatePlayerUI();
                    return;
                }
            }

            OnLocalPlayerInitialise();
            myInventory.Initialise();
            DontDestroyOnLoad(gameObject);
        }

        private void CreatePlayerUI()
        {
            PlayerUI playerUI = Instantiate(playerUIPrefab, transform.position + playerUISpawnOffset, Quaternion.identity, transform).GetComponent<PlayerUI>();
            playerUI.Initialise(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (photonView.IsMine)
                {
                    Player.localPlayer.entity.Hit(-10);
                }
            }
        }
    }
}
