using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyReplica : TargetReplica, ITargetable
{
    private Enemy enemyInfo;
    protected override Target target { get => enemyInfo; }
    [SerializeField] private Outline outline;


    private new void Awake()
    {
        base.Awake();
        outline.enabled = false;
    }

    public void Init(Enemy enemy)
    {
        this.enemyInfo = enemy;
        base.Init(enemy);
       
    }

    public void SetHighlighted(bool state)
    {
        outline.enabled = state;
    }

    private new void OnDestroy()
    {
        base.OnDestroy();

    }
}
