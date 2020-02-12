using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Title : MonoBehaviour
{
    public GameObject BackGround;
    public GameObject TitleObj;
    public GameObject MenuObj;
    public GameObject InfoObj;
    public GameObject RankObj;
    Ranking rank;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        ToTitle();
        mat = BackGround.GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float scr = Mathf.Repeat(Time.time * 0.1f, 1);
        Vector2 off = new Vector2(scr, 0);
        mat.SetTextureOffset("_MainTex", off);
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
    public void GameStart()
    {
        SceneManager.LoadScene("otogeScene");
    }
    public void RankingTextSet()
    {
        for(int i = 0;i < 5; i++)
        {
            RankObj.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Text>().text = (i+1) + "位：" + rank.rank.score[i] + "点";
        }
    }
}
