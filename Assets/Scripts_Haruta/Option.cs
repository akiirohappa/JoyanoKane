using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = gameObject.GetComponentInChildren<Text>();
        if (image.activeSelf == true)
        {
            image.SetActive(false);
            buttonText.text = "操作方法";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeSwitch()
    {
        if (image.activeSelf == true)
        {
            image.SetActive(false);
            buttonText.text = "操作方法";
        }
        else if (image.activeSelf == false)
        {
            image.SetActive(true);
            buttonText.text = "戻る";
        }
    }

}
