﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStats : MonoBehaviour {

    public int exp = 0;
    public int level = 1;
    public int potions = 0;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
    public void loadStats(PlayerValues player) {
        player.currentXP = exp;
        player.hpFlaskAmount = potions;
        player.setLevel(level);
        FindObjectOfType<CharSheetController>().updateStats();
        FindObjectOfType<CombatManager>().xpText.text = "XP " + player.getXP() + "/" + player.getXPForLevel(player.getLevel());
    }

    public void saveStats(PlayerValues player) {
        exp = player.currentXP;
        potions = player.hpFlaskAmount;
        level = CharSheetController.Lvl;
    }
}
