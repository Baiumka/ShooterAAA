using UnityEngine;
using TMPro;
using System;

public class GameWindow : Window
{
    private Player player;
    [SerializeField] private TMP_Text nicknameTMP;
    [SerializeField] private TMP_Text healthTMP;
    [SerializeField] private TMP_Text weaponNameTMP;
    [SerializeField] private TMP_Text weaponAmmoTMP;

    private new void Start()
    {
        base.Start();
        GameController.instance.onPlayerSpawned += InitPlayer;
    }

    private void InitPlayer(Player player)
    {
        this.player = player;
        nicknameTMP.text = player.Name;
        healthTMP.text = $"{player.Health}/{player.MaxHealth}";
    }

    private void Update()
    {
        if (this.player != null)
        {
            if (player.weapon != null)
            {
                weaponNameTMP.text = player.weapon.name.ToString();
                weaponAmmoTMP.text = $"{player.weapon.ammo}/{player.weapon.maxAmmo}";
            }
        }
    }

}
