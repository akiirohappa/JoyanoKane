using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class result : MonoBehaviour
{
    public Text score, combo,text;
    public int scoreint, comboint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score.text = scoreint + "人払";
        combo.text = comboint + "鳴";
        
        if (scoreint == 0)
        {
            text.text = "話にならん";
        }
        else if(scoreint < 300)
        {
            text.text = "出直してまいれ";
        }
        else if(scoreint < 600)
        {
            text.text = "まぁまぁじゃな";
        }
        else if(scoreint < 1200)
        {
            text.text = "なかなかじゃな";
        }
        else
        {
            text.text = "来年も頼むぞい";
        }
        
    }
}
