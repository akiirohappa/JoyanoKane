using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;


[System.Serializable]
public class JsonNoteData
{
    [SerializeField]
    public float Time;
    [SerializeField]
    public int noteLine;

    public JsonNoteData(float x ,int y)
    {
        Time = x;
        noteLine = y;
    }
};

[System.Serializable]
public class HumenData
{
    public string audioClipName;
    public List<JsonNoteData> jsonNoteDatas = new List<JsonNoteData>();
}

public class HumenMake : MonoBehaviour
{
    [System.Serializable]
    public class NoteData
    {
        [SerializeField]
        public float noteTime;
        [SerializeField]
        public int noteLine;

        public NoteData(float x , int y)
        {
            noteTime = x;
            noteLine = y;
            //for (int i = 0; i < y; i++) noteLine[y] = z;
        }
    };

    public AudioClip audioClip;
    private AudioSource audioSource;

    private int bpm;
    private int MaxNotes = 0;
    public string MusicPath;
    [SerializeField]
    private GameObject HumenBase;
    [SerializeField]
    private GameObject NoteBase;
    [SerializeField]
    private GameObject LineBase;
    [SerializeField]
    private GameObject NoteLine;
    [SerializeField]
    private HumenNote note_Humen;

    [SerializeField]
    private SpriteRenderer tapPoint = new SpriteRenderer();
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text MusicTimeText;
    [SerializeField]
    private Text BPMText;
    [SerializeField]
    private Text MaxNoteText;
    [SerializeField]
    private AudioSource tapSE;
    [SerializeField]
    private AudioSource NoteSE;

    bool playnow = false;
    private int SelectLine_LR = 0;

    [SerializeField]
    private List<NoteData> Notes = new List<NoteData>();

    [SerializeField]
    private List<NoteData> TestPlayNotes = new List<NoteData>();

    [SerializeField]
    private List<HumenNote> NoteLists = new List<HumenNote>();


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        Debug.Log(Notes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //音源再生処理
        if (playnow == true)
        {
            if (TestPlayNotes.Count <= 0) playnow = false;
            else if (TestPlayNotes.Last().noteTime <= audioSource.time)
            {
                if(TestPlayNotes.Last().noteLine == 1) NoteSE.PlayOneShot(NoteSE.clip);
                TestPlayNotes.Remove(TestPlayNotes.Last());
                LineBase.transform.position = new Vector3(NoteBase.transform.position.x - 0.1f, HumenBase.transform.position.y, HumenBase.transform.position.z);
                NoteBase.transform.position = new Vector3(NoteBase.transform.position.x - 0.1f, HumenBase.transform.position.y, HumenBase.transform.position.z);
            }
        }

        //音源再生中でないなら編集可能にする
        else if (playnow == false)
        {
            //カーソルを移動する処理
            //右移動
            if (Input.GetKeyDown(KeyCode.RightArrow) && SelectLine_LR < MaxNotes)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectLine_LR + 10 < MaxNotes) SelectLine_LR += 10;
                    else SelectLine_LR = MaxNotes - 1;
                }
                else SelectLine_LR++;
                Debug.Log(SelectLine_LR);
                LineBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
                NoteBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
            }
            //下移動
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && SelectLine_LR > 0)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectLine_LR - 10 > 0) SelectLine_LR -= 10;
                    else SelectLine_LR = 0;
                }
                else SelectLine_LR--;
                Debug.Log(SelectLine_LR);
                LineBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
                NoteBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
            }
            //単押しノートを配置
            else if (Input.GetButtonDown("Note1") && Notes.Count > 0)
            {
                Notes[Notes.Count - 1 - SelectLine_LR].noteLine = 1;
                HumenNote humenNote = Instantiate(note_Humen);
                humenNote.transform.position = new Vector3(NoteBase.transform.position.x + (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
                humenNote.transform.parent = NoteBase.transform;
                humenNote.Line_LR = SelectLine_LR;
                NoteLists.Add(humenNote);
            }
            //ノートを削除
            else if (Input.GetButtonDown("Note2") && Notes.Count > 0)
            {
                Notes[Notes.Count - 1 - SelectLine_LR].noteLine = 0;
                for (int x = 0; x < NoteLists.Count; x++)
                {
                    if (NoteLists[x].Line_LR == SelectLine_LR)
                    {
                        Destroy(NoteLists[x].gameObject);
                        NoteLists.Remove(NoteLists[x]);
                        break;
                    }
                }
            }
        }
    }

    //音源を読み込みいろいろセッティングする関数
    public void MusicSet()
    {
        //musicフォルダから指定されたファイルを読み込みaudiosourceに入れる
        audioClip = Resources.Load("music/" + inputField.text) as AudioClip;
        audioSource.clip = audioClip;
        //bpmを算出し、250以上(テンポが速すぎる)なら半分にする
        bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);
        if (bpm >= 250) Mathf.Ceil(bpm /= 2);
        //曲中に1列におけるノートの最大数を求める
        MaxNotes = (int)(audioSource.clip.length / ((1 / (bpm / 60.0f) / 4)));
        //        Debug.Log((1 / (bpm / 60.0f) / 4));
        //        Debug.Log((int)(60 / (1 / (bpm / 60.0f) / 4)));
        //上で求めた数を表示
        MusicTimeText.text = audioSource.clip.length.ToString();
        BPMText.text = bpm.ToString();
        MaxNoteText.text = MaxNotes.ToString();

        //ノートを配置するリストを作成
        for (int x = 0; x < MaxNotes; x++)
        {
            Notes.Insert(0, new NoteData((1 / (bpm / 60.0f) / 4 * x), 0));
            GameObject line = Instantiate(NoteLine);
            SpriteRenderer sprite = line.GetComponent<SpriteRenderer>();
            line.transform.position = new Vector3(HumenBase.transform.position.x + (x * 0.1f), HumenBase.transform.position.y, 0);
            line.transform.parent = LineBase.transform;
            if (Notes[0].noteTime % 1 == 0) sprite.color = Color.red;
            //            if(x %  4 == 0) sprite.color = Color.red;
            else sprite.color = Color.white;
            //            Debug.Log(Notes[0]);
        }
        Debug.Log(Notes.Count);
    }

    public void MusicPlay()
    {
        if (playnow == false)
        {
            LineBase.transform.position = new Vector3(HumenBase.transform.position.x, HumenBase.transform.position.y, HumenBase.transform.position.z);
            NoteBase.transform.position = new Vector3(HumenBase.transform.position.x, HumenBase.transform.position.y, HumenBase.transform.position.z);
            TestPlayNotes = new List<NoteData>(Notes);
            audioSource.Play();
            playnow = true;
        }
    }
    public void MusicStop()
    {
        playnow = false;
        LineBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
        NoteBase.transform.position = new Vector3(HumenBase.transform.position.x - (0.1f * SelectLine_LR), HumenBase.transform.position.y, HumenBase.transform.position.z);
        audioSource.Stop();
    }

    //jsonファイルを作成するスクリプト
    public void JsonMake()
    {
        HumenData humenData = new HumenData();
        MainSaveData.instance.MusicNamesAdd(inputField.text);
        for (int x = MaxNotes - 1; x > 0; x--)
        {
            if (Notes[x].noteLine != 0)
            {
                humenData.jsonNoteDatas.Add(new JsonNoteData(Notes[x].noteTime, Notes[x].noteLine));
            }
        }
        humenData.audioClipName = audioClip.name;

        string str = JsonUtility.ToJson(humenData);

        StreamWriter streamWriter;
        streamWriter = new StreamWriter(Application.dataPath + "/Resources/HumenData/" + inputField.text + ".json",false);
        streamWriter.Write(str);
        streamWriter.Flush();
        streamWriter.Close();

        MainSaveData.instance.FileSave();
        Debug.Log(str);
    }

    public void JsonAdd()
    {
        MainSaveData.instance.MusicNamesAdd(inputField.text);
    }
}

