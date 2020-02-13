using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class result : MonoBehaviour
{
    public Text score, combo,text;
    public int scoreint, comboint,MaxNote;
    Ranking ranking;
    // Start is called before the first frame update
    void Start()
    {
        ranking = new Ranking();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResultView()
    {
        score.text = scoreint + "人払";
        combo.text = comboint + "鳴";
        int MaxScore = MaxNote * 100;
        if (scoreint < MaxScore / 4)
        {
            text.text = "話にならん";
        }
        else if (scoreint < MaxScore / 2)
        {
            text.text = "まぁまぁじゃな";
        }
        else if (scoreint < MaxScore / 2 + MaxScore / 4)
        {
            text.text = "なかなかじゃな";
        }
        else
        {
            text.text = "来年も頼むぞい";
        }

        ranking.RankingAdd(scoreint);
        ranking.RankingSave();
    }
}
