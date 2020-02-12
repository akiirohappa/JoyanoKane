using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject TitleObj;
    public GameObject MenuObj;
    public GameObject InfoObj;
    public GameObject RankObj;
    Ranking rank;
    // Start is called before the first frame update
    void Start()
    {
        ToTitle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToMenu()
    {
        MenuObj.SetActive(true);
        TitleObj.SetActive(false);
        InfoObj.SetActive(false);
        RankObj.SetActive(false);
    }
    public void ToTitle()
    {
        MenuObj.SetActive(false);
        TitleObj.SetActive(true);
        InfoObj.SetActive(false);
        RankObj.SetActive(false);
    }
    public void ToInfo()
    {
        MenuObj.SetActive(true);
        TitleObj.SetActive(false);
        InfoObj.SetActive(true);
        RankObj.SetActive(false);
    }
    public void ToRank()
    {
        MenuObj.SetActive(true);
        TitleObj.SetActive(false);
        InfoObj.SetActive(false);
        RankObj.SetActive(true);
        rank = new Ranking();
        RankingTextSet();
    }
    public void RankingTextSet()
    {
        for(int i = 0;i < 5; i++)
        {
            RankObj.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Text>().text = (i+1) + "位：" + rank.rank.score[i] + "点";
        }
    }
}
