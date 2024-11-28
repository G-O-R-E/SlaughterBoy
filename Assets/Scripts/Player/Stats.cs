using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public enum PlayerStat
    {
        Hp,
        Regen,
        Speed,
        Attack,
        AttackSpeed,
        CloseDamage,
        DistanceDamage,
        Critical,
        Scope,
        Dodge,
        Harvest,
        Negotiation,
        NbStats,
    };

    [SerializeField] float[] stats = new float[(int)(PlayerStat.NbStats)];
    private float[] normalStats = new float[(int)(PlayerStat.NbStats)];
    private PlayerStat playerStats;

    public string SetDifferenceStats(int id)
    {
        string text = null;
        string arrow = "\u25B2";

        if (id != (int)PlayerStat.Negotiation)
        {
            if (stats[id] < 0)
            {
                arrow = "\u25BC";
            }
        }
        else
        {
            if (stats[id] > 0)
            {
                arrow = "\u25BC";
            }
        }

        if (stats[id] != 0)
        {
            playerStats = (PlayerStat)id;
            text = playerStats.ToString() + " : " + MathF.Abs(stats[id]) + arrow + "\n";
        }
        return text;
    }

    public bool IsColorGreen(int id)
    {
        bool isGreen = true;
        if (id != (int)PlayerStat.Negotiation)
        {
            if (stats[id] < 0)
            {
                isGreen = false;
            }
        }
        else
        {
            if (stats[id] > 0)
            {
                isGreen = false;
            }
        }

        return isGreen;
    }

    public void InitBasicPlayerStats()
    {
        normalStats[(int)PlayerStat.Hp] = 20;
        normalStats[(int)PlayerStat.Regen] = 0;
        normalStats[(int)PlayerStat.Speed] = 1;
        normalStats[(int)PlayerStat.Attack] = 2;
        normalStats[(int)PlayerStat.AttackSpeed] = 1;
        normalStats[(int)PlayerStat.CloseDamage] = 1;
        normalStats[(int)PlayerStat.DistanceDamage] = 1;
        normalStats[(int)PlayerStat.Critical] = 0;
        normalStats[(int)PlayerStat.Scope] = 0;
        normalStats[(int)PlayerStat.Dodge] = 0;
        normalStats[(int)PlayerStat.Harvest] = 0;
        normalStats[(int)PlayerStat.Negotiation] = 0;
    }

    public int GetLife()
    {
        return (int)stats[(int)PlayerStat.Hp];
    }
    public int GetRegen()
    {
        return (int)stats[(int)PlayerStat.Regen];
    }
    public int GetSpeed()
    {
        return (int)stats[(int)PlayerStat.Speed];
    }
    public int GetAttack()
    {
        return (int)stats[(int)PlayerStat.Attack];
    }
    public int GetAttackSpeed()
    {
        return (int)stats[(int)PlayerStat.AttackSpeed];
    }
    public int GetCloseDamage()
    {
        return (int)stats[(int)PlayerStat.CloseDamage];
    }
    public int GetDistanceDamage()
    {
        return (int)stats[(int)PlayerStat.DistanceDamage];
    }
    public int GetCrit()
    {
        return (int)stats[(int)PlayerStat.Critical];
    }
    public int GetScope()
    {
        return (int)stats[(int)PlayerStat.Scope];
    }
    public int GetDodge()
    {
        return (int)stats[(int)PlayerStat.Dodge];
    }
    public int GetHarvest()
    {
        return (int)stats[(int)PlayerStat.Harvest];
    }
    public int GetNegotiation()
    {
        return (int)stats[(int)PlayerStat.Negotiation];
    }
    public float[] GetStatsTab()
    {
        return this.stats;
    }

    public void InitStats()
    {
        InitBasicPlayerStats();

        for (int i = 0; i < (int)PlayerStat.NbStats; i++)
        {
            stats[i] += normalStats[i];
        }
    }

    public void AddStats(float[] _statsToAdd)
    {
        for (int i = 0; i < (int)PlayerStat.NbStats; i++)
        {
            stats[i] += _statsToAdd[i];
        }
    }

    public void SaveStat(GameManager.DataPlayer data)
    {
        for (int i = 0; i < (int)PlayerStat.NbStats; i++)
        {
            data.stats[i] = stats[i];
        }
    }
}
