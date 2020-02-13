using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking
{
    public RankingData rank;
    string rankKey = "Ranking";
    public Ranking()
    {
        RankingLoad();
    }
    public int RankingAdd(int newScore)
    {
        int inRank = 0;
        int max = newScore;
        for(int i = 0;i < 5; i++)
        {
            if(inRank == 0)
            {
                inRank = i + 1;
            }
            if(rank.score[i] <= max)
            {
                int n = rank.score[i];
                rank.score[i] = max;
                max = n;
                Debug.Log((i+1) +":"+ (rank.score[i]));
            }
        }
        return inRank;
    }
    public void RankingLoad()
    {
        string j = PlayerPrefs.GetString(rankKey,"");
        if (j == "")
        {
            rank = new RankingData();
        }
        else
        {
            rank = JsonUtility.FromJson<RankingData>(j);
        }
    }
    public void RankingSave()
    {
        string j = JsonUtility.ToJson(rank);
        PlayerPrefs.SetString(rankKey, j);
        PlayerPrefs.Save();
    }
}
[System.Serializable]
public class RankingData
{
    public int[] score = new int[5];
    public RankingData()
    {
        score = new int[5];
        for(int i = 0;i < 5; i++)
        {
            score[i] = 0;
        }
    }
}