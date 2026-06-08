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
    [SerializeField] private TMP_Text ReloadingTMP;

    private new void Start()
    {
        base.Start();
        GameController.instance.onPlayerSpawned += InitPlayer;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        GameController.instance.onPlayerSpawned -= InitPlayer;
        if(player != null )
        {
            if(player.weapon != null)
            {
                player.weapon.onStartLoad -= StartReload;
                player.weapon.onEndLoad -= EndReload;
            }
        }
    }

    private void InitPlayer(Player player)
    {
        this.player = player;
        nicknameTMP.text = player.Name;
        healthTMP.text = $"{player.Health}/{player.MaxHealth}";
        if (player.weapon != null)
        {
            player.weapon.onStartLoad -= StartReload;
            player.weapon.onEndLoad -= EndReload ;
        }
    }

    private void EndReload()
    {
       // throw new NotImplementedException();
    }

    private void StartReload()
    {
       // throw new NotImplementedException();
    }

    private void Update()
    {
        if (this.player != null)
        {
            if (player.weapon != null)
            {
                weaponNameTMP.text = player.weapon.name.ToString();
                weaponAmmoTMP.text = $"{player.weapon.ammo}/{player.weapon.maxAmmo}";
                ReloadingTMP.gameObject.SetActive(player.weapon.isReload);
            }
        }
    }

}
