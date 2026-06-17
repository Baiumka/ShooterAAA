using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyReplica : TargetReplica, ITargetable
{
    private Enemy enemyInfo;
    protected override Target target { get => enemyInfo; }
    [SerializeField] private Outline outline;
    [SerializeField] private TMP_Text nickNameTMP;

    private new void Awake()
    {
        base.Awake();
        outline.enabled = false;
        nickNameTMP.gameObject.SetActive(false);
    }

    private new void Update()
    {
        base.Update();
        if (nickNameTMP.gameObject.activeSelf)
        {
            nickNameTMP.text = $"{enemyInfo.Health}/{enemyInfo.MaxHealth}";
        }
    }

    public void Init(Enemy enemy)
    {
        this.enemyInfo = enemy;
        base.Init(enemy);        
    }

    public new void SetHighlighted(bool state)
    {
        outline.enabled = state;
        nickNameTMP.gameObject.SetActive(state);
    }

    private new void OnDestroy()
    {
        base.OnDestroy();

    }


}
