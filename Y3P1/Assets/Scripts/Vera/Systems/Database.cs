﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour {

    public static Database hostInstance;
    public List<Sprite> allSprites = new List<Sprite>();
    public List<GameObject> allGameobjects = new List<GameObject>();

    [Header("Weapons")]

    [Header("Gold")]
    public List<GameObject> goldObject = new List<GameObject>();

    [Header("Attacks")]
    [SerializeField] private List<string> secundaryRangedAttacks = new List<string>();
    [SerializeField] private List<string> secundaryMeleeAttacks = new List<string>();
    [SerializeField] private List<string> primaryAttacks = new List<string>();
    public List<string> secundaryBuffs = new List<string>();
    public List<string> singleSecondary = new List<string>();

    [Header("Crossbow")]
    [SerializeField] private List<string> crossbowNames = new List<string>();
    public List<Sprite> crossbowSprite = new List<Sprite>();
    public List<GameObject> crossbowObject = new List<GameObject>();

    [Header("Axe")]
    [SerializeField] private List<string> axeNames = new List<string>();
    public List<Sprite> axeSprite = new List<Sprite>();
    public List<GameObject> axeObject = new List<GameObject>();

    [Header("sword")]
    [SerializeField] private List<string> swordNames = new List<string>();
    public List<Sprite> swordSprite = new List<Sprite>();
    public List<GameObject> swordObject = new List<GameObject>();

    [Header("Hammer")]
    [SerializeField] private List<string> hammerNames = new List<string>();
    public List<Sprite> hammerSprite = new List<Sprite>();
    public List<GameObject> hammerObject = new List<GameObject>();

    [Header("Armor")]
    [Header("Helmet")]
    [SerializeField] private List<string> helmetNames = new List<string>();
    public List<Sprite> helmetSprite = new List<Sprite>();
    public List<GameObject> helmetObject = new List<GameObject>();

    [Header("Trinket")]
    [SerializeField] private List<string> trinketNames = new List<string>();
    public List<Sprite> trinketSprite = new List<Sprite>();
    public List<GameObject> trinketObject = new List<GameObject>();




    public void Awake()
    {
        if(hostInstance == null)
        {
            hostInstance = this;
        }
        allSprites.Add(null);
        allGameobjects.Add(null);

        allGameobjects.AddRange(crossbowObject);
        allGameobjects.AddRange(axeObject);
        allGameobjects.AddRange(swordObject);
        allGameobjects.AddRange(hammerObject);
        allGameobjects.AddRange(helmetObject);
        allGameobjects.AddRange(trinketObject);
        allGameobjects.AddRange(goldObject);

        allSprites.AddRange(crossbowSprite);
        allSprites.AddRange(axeSprite);
        allSprites.AddRange(swordSprite);
        allSprites.AddRange(hammerSprite);
        allSprites.AddRange(helmetSprite);
        allSprites.AddRange(trinketSprite);
    }

    // secundary's

    public string GetRangedSecundary(bool legen)
    {
        int rand = Random.Range(0, secundaryRangedAttacks.Count);
        if(legen && rand == 0)
        {
            rand = Random.Range(1, secundaryRangedAttacks.Count);
        }
        return secundaryRangedAttacks[rand];
    }

    public string GetMeleeSecundary(bool legen)
    {
        int rand = Random.Range(0, secundaryMeleeAttacks.Count);
        if (legen && rand == 0)
        {
            rand = Random.Range(1, secundaryMeleeAttacks.Count);
        }
        return secundaryMeleeAttacks[rand];
    }

    //names

    public string GetCrossbowName()
    {
        return crossbowNames[Random.Range(0, crossbowNames.Count)];
    }

    public string GetAxeName()
    {
        return axeNames[Random.Range(0, axeNames.Count)];
    }

    public string GetHammerName()
    {
        return hammerNames[Random.Range(0, hammerNames.Count)];
    }

    public string GetSwordName()
    {
        return swordNames[Random.Range(0, swordNames.Count)];
    }

    public string GetHelmetName()
    {
        return helmetNames[Random.Range(0, helmetNames.Count)];
    }

    public string GetTrinketName()
    {
        return trinketNames[Random.Range(0, trinketNames.Count)];
    }

    //sprites

    public int GetCrossbowSprite()
    {
        int randomSpri = Random.Range(0, crossbowSprite.Count);
        Sprite mySpri = crossbowSprite[randomSpri];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetAxeSprite()
    {
        Sprite mySpri = axeSprite[Random.Range(0, axeSprite.Count)];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetSwordSprite()
    {
        Sprite mySpri = swordSprite[Random.Range(0, swordSprite.Count)];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetHammerSprite()
    {
        Sprite mySpri = hammerSprite[Random.Range(0, hammerSprite.Count)];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetHelmetSprite()
    {
        Sprite mySpri =  helmetSprite[Random.Range(0, helmetSprite.Count)];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetTrinketSprite()
    {
        Sprite mySpri = trinketSprite[Random.Range(0, trinketSprite.Count)];
        int index = 0;
        for (int i = 0; i < allSprites.Count; i++)
        {
            if (mySpri == allSprites[i])
            {
                index = i;
            }
        }
        return index;
    }

    //objects

    public int GetGoldObject(int rarity)
    {

        GameObject myObj = goldObject[0];
        if(rarity == 1 && rarity < goldObject.Count)
        {
            myObj = goldObject[1];
        }
        if (rarity == 2 && rarity < goldObject.Count)
        {
            myObj = goldObject[2];
        }
        if (rarity == 3 && rarity < goldObject.Count)
        {
            myObj = goldObject[3];
        }

        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetCrossbowObject()
    {
        GameObject myObj = crossbowObject[Random.Range(0, crossbowObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetAxeObject()
    {
        GameObject myObj = axeObject[Random.Range(0, axeObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetSwordObject()
    {
        GameObject myObj = swordObject[Random.Range(0, swordObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetHammerObject()
    {
        GameObject myObj = hammerObject[Random.Range(0, hammerObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetTrinketObject()
    {
        GameObject myObj = trinketObject[Random.Range(0, trinketObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }

    public int GetHelmetObject()
    {
        GameObject myObj = helmetObject[Random.Range(0, helmetObject.Count)];
        int index = 0;
        for (int i = 0; i < allGameobjects.Count; i++)
        {
            if (myObj == allGameobjects[i])
            {
                index = i;
            }
        }
        return index;
    }
}
