using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    [SerializeField]
    string SceneName;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MusicSelect_Move()
    {
        //曲名が入っていない(曲がない)場合は移動できない
        if (MainSaveData.instance.MusicNamesGet().Count > 0)
        {
            MainSaveData.instance.StartCoroutine("Load", "MusicSelect");
        }
    }

    public void SceneMove()
    {
        MainSaveData.instance.StartCoroutine("Load", SceneName);
    }

    public void Option_Move()
    {
        MainSaveData.instance.StartCoroutine("Load", "Option");
    }

    public void HumenMaker_Move()
    {
        MainSaveData.instance.StartCoroutine("Load", "HumenMaker");
    }

    public void GameEnd()
    {
        //        UnityEditor.EditorApplication.isPlaying = false;
        UnityEngine.Application.Quit();
    }
}
