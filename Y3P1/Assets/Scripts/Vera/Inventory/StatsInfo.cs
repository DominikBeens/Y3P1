﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsInfo : MonoBehaviour {
    public static StatsInfo instance = new StatsInfo();
    [SerializeField] private List<TMP_Text> allText = new List<TMP_Text>();
    [SerializeField] private GameObject myPanel;
    [SerializeField] private Image myImage;
    [SerializeField] private TMP_Text gold;
    [SerializeField] private List<TMP_Text> allStatsText = new List<TMP_Text>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void DisablePanel()
    {
        myPanel.SetActive(false);
        myImage.enabled = false;
    }

    public void UpdateGold(int amount)
    {
        gold.text = "Gold: " + AddPoints(amount.ToString());
    }

    private string AddPoints(string amount)
    {
        List<char> myString = new List<char>(amount);
        myString.Reverse();
        List<char> allSymbols = new List<char>();
        int a = 0;
        
        for (int i = 0; i < myString.Count; i++)
        {
            a++;
            if (a < 4)
            {
                allSymbols.Add(myString[i]);
            }
            else
            {
                allSymbols.Add('.');
                a = 0;
                i--;
            }

        }
        allSymbols.Reverse();
        string test = new string(allSymbols.ToArray());
        return test;
    }



    public void SetPlayerStats(string[] stats,string[] avrILvl)
    {
        List<string> allStrings = new List<string>();
        if (avrILvl != null)
        {
            allStrings.AddRange(avrILvl);
            allStrings.Add("");
        }
        if (stats != null)
        {
            allStrings.AddRange(stats);
        }
        for (int i = 0; i < allStatsText.Count; i++)
        {
            if (i < allStrings.Count)
            {
                if (allStatsText[i] != null)
                {
                    allStatsText[i].text = allStrings[i];
                    allStatsText[i].enabled = true;
                }
            }
            else
            {
                allStatsText[i].enabled = false;
            }
        }
    }

    public void SetText(string[] item,string[] damage, string[] weapon, string[] ranged, string[] melee, string[] helmet, string[] trinket, string[] value)
    {
        List<string> allTextNeeded = new List<string>();
        if(item != null)
        {
            allTextNeeded.AddRange(item);
        }
        if(damage != null)
        {
            allTextNeeded.AddRange(damage);
            allTextNeeded.Add("");
        }
        if (ranged != null)
        {
            allTextNeeded.AddRange(ranged);
            allTextNeeded.Add("");
        }
        if (melee != null)
        {
            allTextNeeded.AddRange(melee);
            allTextNeeded.Add("");
        }
        if (weapon != null)
        {
            allTextNeeded.AddRange(weapon);
            allTextNeeded.Add("");
        }

        if(helmet != null)
        {
            allTextNeeded.AddRange(helmet);
        }
        if(trinket != null)
        {
            allTextNeeded.AddRange(trinket);
        }
        if(value != null)
        {
            allTextNeeded.AddRange(value);
        }


        for (int i = 0; i < allText.Count; i++)
        {
            if(i < allTextNeeded.Count)
            {
                if(allText[i] != null)
                {
                    myPanel.SetActive(true);
                    myImage.enabled = true;
                    allText[i].text = allTextNeeded[i];
                    allText[i].enabled = true;
                }
            }
            else
            {
                allText[i].enabled = false;
            }
        }
    }
}
