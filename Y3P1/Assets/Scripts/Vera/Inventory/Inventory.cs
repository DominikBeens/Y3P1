﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using Y3P1;

public class Inventory : MonoBehaviourPunCallbacks
{
    public List<InventorySlot> allSlots = new List<InventorySlot>();
    [SerializeField] private List<Item> allItems = new List<Item>();
    public InventorySlot currentSlot;
    public InventorySlot lastSlot;
    public Item drag;
    private bool dragging;
    [SerializeField] private Image onMouse;
    [SerializeField] private List<Item> startingItems = new List<Item>();
    private bool isInitialised;

    public void AddSlots()
    {
        allItems.Clear();
        for (int i = 0; i < allSlots.Count; i++)
        {
            allItems.Add(null);
        }
    }

    public void SetCurrentSlot(InventorySlot current)
    {
        currentSlot = current;
    }

    public void SetLastSlot(InventorySlot last)
    {
        lastSlot = last;
    }

    public void StartDragging()
    {
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == currentSlot)
            {
                if (allItems[i] == null)
                {
                    return;
                }
                drag = allItems[i];
            }
        }
        dragging = true;

        lastSlot = currentSlot;

    }

    public void ExitInv()
    {
        GetComponentInParent<Canvas>().enabled = false;
    }

    [PunRPC]
    private void DropItem(string toDrop,byte[] item,Vector3 pos)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject insItem = PhotonNetwork.InstantiateSceneObject(toDrop, pos, Quaternion.identity);
            int id = insItem.GetComponent<PhotonView>().ViewID;
            photonView.RPC("RI", RpcTarget.AllBuffered, item, id);
        }
    }

    [PunRPC]
    private void RI(byte[] item, int id)
    {
        GameObject itemIG = PhotonNetwork.GetPhotonView(id).gameObject;
        itemIG.GetComponent<WeaponPrefab>().myItem = (Item)ByteArrayToObject(item);
        itemIG.GetComponent<WeaponPrefab>().Drop();
    }

    private byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private object ByteArrayToObject(byte[] bytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(bytes, 0, bytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        object obj = binForm.Deserialize(memStream);

        return obj;
    }

    void SaveItem(Item toSave, string objName)
    {
        byte[] saved = ObjectToByteArray(toSave);
        photonView.RPC("DropItem", RpcTarget.AllBuffered, objName, saved, Player.localPlayer.transform.position);
    }

    public void StopDragging()
    {
        if (drag == null || !CheckAvailability())
        {
            drag = null;
            return;
        }

        int lastSlotIndex = 0;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == lastSlot)
            {
                lastSlotIndex = i;
            }
        }
        if (currentSlot == null)
        {
            if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.weapon)
            {
                UnequipWeapon(lastSlotIndex);
            }
            GameObject newObj = Database.hostInstance.allGameobjects[allItems[lastSlotIndex].prefabIndex];
            print(newObj);
            SaveItem(allItems[lastSlotIndex], newObj.name);

            RemoveItem(lastSlotIndex);
            drag = null;
            return;
        }
        int currentSlotIndex = 0;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == currentSlot)
            {
                currentSlotIndex = i;
            }
        }

        if (allItems[currentSlotIndex] != null)
        {
            Item temp = allItems[currentSlotIndex];
            allItems[currentSlotIndex] = allItems[lastSlotIndex];
            allItems[lastSlotIndex] = temp;
            allSlots[currentSlotIndex].EnableImage();
            allSlots[lastSlotIndex].SetImage(Database.hostInstance.allSprites[allItems[lastSlotIndex].spriteIndex]);
            allSlots[currentSlotIndex].SetImage(Database.hostInstance.allSprites[allItems[currentSlotIndex].spriteIndex]);
        }
        else
        {
            if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.weapon)
            {
                UnequipWeapon(lastSlotIndex);
            }
            allItems[currentSlotIndex] = allItems[lastSlotIndex];
            allSlots[currentSlotIndex].EnableImage();
            allSlots[currentSlotIndex].SetImage(Database.hostInstance.allSprites[allItems[currentSlotIndex].spriteIndex]);
            RemoveItem(lastSlotIndex);
        }

        if (allSlots[currentSlotIndex].slotType == InventorySlot.SlotType.weapon)
        {
            allSlots[currentSlotIndex].EquipWeapon(allItems[currentSlotIndex] as Weapon);
        }
        else if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.weapon)
        {
            allSlots[lastSlotIndex].EquipWeapon(allItems[lastSlotIndex] as Weapon);
        }
        drag = null;
        dragging = false;

    }

    void RemoveItem(int lSI)
    {
        allItems[lSI] = null;
        allSlots[lSI].DisableImage();
    }

    public bool IsDragging()
    {
        if (drag == null)
        {
            return false;
        }
        return true;
    }

    private bool CheckAvailability()
    {
        int currentSlotIndex = 0;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == currentSlot)
            {
                currentSlotIndex = i;
            }
        }
        int lastSlotIndex = 0;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == lastSlot)
            {
                lastSlotIndex = i;
            }
        }
        if (allSlots[currentSlotIndex].slotType == InventorySlot.SlotType.weapon)
        {
            print("weapon");
            if (allItems[currentSlotIndex] != null)
            {
                if (allItems[lastSlotIndex] is Weapon)
                {
                    return true;
                }
            }

            if (allItems[lastSlotIndex] is Weapon)
            {
                return true;
            }
        }
        else if (allSlots[currentSlotIndex].slotType == InventorySlot.SlotType.helmet)
        {
            print("helmet");
            if (allItems[currentSlotIndex] != null)
            {
                if (allItems[lastSlotIndex] is Helmet)
                {
                    return true;
                }
            }

            if (allItems[lastSlotIndex] is Helmet)
            {
                return true;
            }
        }
        else if (allSlots[currentSlotIndex].slotType == InventorySlot.SlotType.trinket)
        {
            print("trinket");
            if (allItems[currentSlotIndex] != null)
            {
                if (allItems[lastSlotIndex] is Trinket)
                {
                    return true;
                }
            }

            if (allItems[lastSlotIndex] is Trinket)
            {
                return true;
            }
        }
        else if (allSlots[currentSlotIndex].slotType == InventorySlot.SlotType.all)
        {
            if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.weapon && allItems[currentSlotIndex] != null)
            {
                if (allItems[currentSlotIndex] is Weapon)
                {
                    return true;
                }
            }
            else if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.helmet && allItems[currentSlotIndex] != null)
            {
                if (allItems[currentSlotIndex] is Helmet)
                {
                    return true;
                }
            }
            else if (allSlots[lastSlotIndex].slotType == InventorySlot.SlotType.trinket && allItems[currentSlotIndex] != null)
            {
                if (allItems[currentSlotIndex] is Trinket)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void UnequipWeapon(int slot)
    {
        allSlots[slot].EquipWeapon(null);
    }

    private void Awake()
    {
        OpenCloseInv();
        drag = null;
    }

    public void OpenCloseInv()
    {
        if (GetComponentInParent<Canvas>().enabled == false)
        {
            GetComponentInParent<Canvas>().enabled = true;
        }
        else
        {
            GetComponentInParent<Canvas>().enabled = false;
        }
    }

    public void AddItem(Item toAdd)
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i] == null && allSlots[i].CheckSlotType())
            {
                allItems[i] = toAdd;
                allSlots[i].SetImage(Database.hostInstance.allSprites[allItems[i].spriteIndex]);
                allSlots[i].EnableImage();
                break;
            }
        }
    }

    // for testing
    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        if(currentSlot != null)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                int index = 0;
                for (int i = 0; i < allSlots.Count; i++)
                {
                    if(currentSlot == allSlots[i])
                    {
                        index = i;
                    }
                }
                if(allItems[index] != null)
                {
                    int newSlot = 0;
                    int type = 0;
                    if (allItems[index] is Weapon)
                    {
                        for (int i = 0; i < allSlots.Count; i++)
                        {
                            if (allSlots[i].slotType == InventorySlot.SlotType.weapon)
                            {
                                newSlot = i;
                                type = 1;
                            }
                        }
                    }
                    else if (allItems[index] is Helmet)
                    {
                        for (int i = 0; i < allSlots.Count; i++)
                        {
                            if (allSlots[i].slotType == InventorySlot.SlotType.helmet)
                            {
                                newSlot = i;
                                type = 2;
                            }
                        }
                    }
                    else if (allItems[index] is Trinket)
                    {
                        for (int i = 0; i < allSlots.Count; i++)
                        {
                            if (allSlots[i].slotType == InventorySlot.SlotType.trinket)
                            {
                                newSlot = i;
                                type = 3;
                            }
                        }
                    }
                    if(type != 0)
                    {
                        if(allItems[newSlot] == null)
                        {

                        }
                        Item temp = allItems[index];
                        allItems[index] = allItems[newSlot];
                        allItems[newSlot] = temp;
                        if (type == 1)
                        {
                            allSlots[newSlot].EquipWeapon((Weapon)allItems[newSlot]);
                        }
                        if (type == 2)
                        {
                            //allSlots[newSlot].EquipHelmet((Helmet)allItems[newSlot]);
                        }
                        if (type == 3)
                        {
                            //allSlots[newSlot].EquipTrinket((Trinket)allItems[newSlot]);
                        }
                        if (allItems[newSlot] != null)
                        {
                            allSlots[newSlot].EnableImage();
                            allSlots[newSlot].SetImage(Database.hostInstance.allSprites[allItems[newSlot].spriteIndex]);
                        }
                        else
                        {
                            allSlots[newSlot].DisableImage();
                        }
                        if (allItems[index] != null)
                        {
                            allSlots[index].EnableImage();
                            allSlots[index].SetImage(Database.hostInstance.allSprites[allItems[index].spriteIndex]);
                        }
                        else
                        {
                            allSlots[index].DisableImage();
                        }
                        drag = null;
                    }            
                }
            }
        }

        if (Input.GetButtonDown("Tab"))
        {
            OpenCloseInv();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddItem(LootRandomizer.instance.DropLoot(1));
        }
        if (drag == null)
        {
            onMouse.enabled = false;
        }
        else
        {
            onMouse.enabled = true;
            onMouse.sprite = Database.hostInstance.allSprites[drag.spriteIndex];
            onMouse.transform.position = Input.mousePosition;
        }
    }

    public bool CheckFull()
    {
        bool check = true;
        for (int i = 0; i < allSlots.Count; i++)
        {
            if(allSlots[i].slotType == InventorySlot.SlotType.all)
            {
                print(allItems[i]);
                if(allItems[i] == null)
                {
                    check = false;
                }
            }
        }
        return check;
    }

    public void Initialise()
    {
        isInitialised = true;
    }
}