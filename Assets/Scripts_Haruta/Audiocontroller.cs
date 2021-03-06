﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Audiocontroller : MonoBehaviour
{
    private AudioSource audioSource;
    public TapPoint Note;

/*    [SerializeField]
    private Text MusicTimeText;
    [SerializeField]
    private Text DeltaTimeText;*/
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text ComboText;
    [SerializeField]
    private Text ComboNaviText;
    /*    [SerializeField]
        private Text MaxComboText;
    */
    public Note note;

    public float MusicTime = -3.0f;
    private int Score;
    private int Combo = 0;
    private int MaxCombo = 0;
    public float ComboBonus = 1.0f;

    private bool PlayFlag = false;


    public string filename;


    public HumenData humenData;


    public result res;
    public Canvas Can;
    public Text Sco, Com;

    class NoteControl
    {
        public Note note;
        public TapPoint tapPoint;
        public float time;
    };

    // Start is called before the first frame update
    void Start()
    {
        ComboText.enabled = false;
        ComboNaviText.enabled = false;

        res = Can.GetComponent<result>();
        Can.enabled = false;
        string data = "";

        StreamReader streamReader;
        streamReader = new StreamReader(Application.dataPath + "/Resources/HumenData/" + MainSaveData.instance.MusicNameGet() + ".json");
        data = streamReader.ReadToEnd();
        streamReader.Close();
        humenData = JsonUtility.FromJson<HumenData>(data);
        res.MaxNote = humenData.jsonNoteDatas.Count;

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = Resources.Load("music/" + humenData.audioClipName)as AudioClip;
        AudioClip audioClip = audioSource.clip;
        
                for(int x = 0; x < humenData.jsonNoteDatas.Count; x++)
                {
                    NoteMake(Instantiate(note), Note, humenData.jsonNoteDatas[x].Time);
                }
               /* 
        NoteMake(Instantiate(note), Note, 1);
        NoteMake(Instantiate(note), Note, 2);
        NoteMake(Instantiate(note), Note, 3);
        NoteMake(Instantiate(note), Note, 3.5f);
        NoteMake(Instantiate(note), Note, 4);
        NoteMake(Instantiate(note), Note, 4.5f);
        NoteMake(Instantiate(note), Note, 5);
        NoteMake(Instantiate(note), Note, 5.5f);
        NoteMake(Instantiate(note), Note, 6);
        NoteMake(Instantiate(note), Note, 6.5f);
        NoteMake(Instantiate(note), Note, 7);
        NoteMake(Instantiate(note), Note, 7.5f);
        NoteMake(Instantiate(note), Note, 8);
        NoteMake(Instantiate(note), Note, 8.5f);
        NoteMake(Instantiate(note), Note, 9);
        NoteMake(Instantiate(note), Note, 9.5f);
        NoteMake(Instantiate(note), Note, 10);
        NoteMake(Instantiate(note), Note, 10.5f);
        NoteMake(Instantiate(note), Note, 11);
        NoteMake(Instantiate(note), Note, 11.5f);
        NoteMake(Instantiate(note), Note, 12);
        NoteMake(Instantiate(note), Note, 12.5f);
        NoteMake(Instantiate(note), Note, 13);
        NoteMake(Instantiate(note), Note, 13.5f);
        */
        Debug.Log(audioClip.length);
        Debug.Log(UniBpmAnalyzer.AnalyzeBpm(audioClip));
    }

    // Update is called once per frame
    void Update()
    {
        MusicTime += Time.deltaTime;
        if (MusicTime >= 0 && audioSource.isPlaying == false && PlayFlag == false)
        {
            PlayFlag = true;
            Debug.Log("Play");
            audioSource.Play();
        }
        else if (MusicTime > audioSource.clip.length && PlayFlag == true)
        {
            if(Can.enabled == false)
            {
                Debug.Log("End");
                res.scoreint = Score;
                res.comboint = MaxCombo;
                res.ResultView();
                Can.enabled = true;
            }
//            MainSaveData.instance.StartCoroutine("Load", "Title");
        }
        /*
        MusicTimeText.text = audioSource.time.ToString();
        DeltaTimeText.text = MusicTime.ToString();*/
        ScoreText.text = Score.ToString();
        ComboText.text = Combo.ToString();
    }

    void NoteMake(Note note,TapPoint parent, float pos)
    {
        note.transform.position = new Vector3(0,parent.transform.position.y,0);
        note.posSet(pos);
        note.transform.parent = parent.transform;
        parent.Notes.Insert(0,note);
        note.NoteSetting();
    }

    public void ComboPlus()
    {
        Combo++;

        if(Combo % 20 == 0)
        {
            ComboBonus +=  0.1f;
            Debug.Log("Bonus=" + ComboBonus);
        }
        if (Combo > MaxCombo) MaxCombo = Combo;
        if (Combo >= 2 && ComboText.enabled == false)
        {
            ComboText.enabled = true;
            ComboNaviText.enabled = true;
        }
    }

    public void ComboReset()
    {
        Combo = 0;
        ComboBonus = 1.0f;
        ComboText.enabled = false;
        ComboNaviText.enabled = false;
    }

    public void ScoreSet(int score)
    {
        Score += score;
    }
}
