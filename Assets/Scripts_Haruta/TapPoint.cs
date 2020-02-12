using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TapPoint : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float waitTime;
    [SerializeField]
    private string NoteName;
    private Note note;
    public List<Note> Notes = new List<Note>();
    [SerializeField]
    private AudioClip TapSound;
    [SerializeField]
    private AudioClip MissSound;
    [SerializeField]
    private AudioClip ClapSound;
    [SerializeField]
    private Text text;
    private AudioSource audioSource;

    [SerializeField]
    private Audiocontroller audiocontroller;
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
        audioSource = gameObject.GetComponent<AudioSource>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        audiocontroller = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audiocontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        //ノートのデータを持っていない場合、最も近いノートのデータを取得
        if (Notes.Count > 0 && note == null) note = Notes.Last().GetComponent<Note>();

        //対応するボタンを押したら判定
        if (Input.GetButtonDown(NoteName))
        {
            Debug.Log("Tap");
            audioSource.clip = ClapSound;
            sprite.color = Color.red;
            if (Notes.Count > 0) Note_Jadge(note);
            else if (Notes.Count <= 0) Debug.Log("ﾉｰﾂｶﾞﾅｲﾖ");
            audioSource.Play();
        }

        //対応するボタンを離したら判定
        else if (Input.GetButtonUp(NoteName))
        {
            sprite.color = Color.white;
        }
        //一定距離を通過するとミスになる
        if (Notes.Count > 0 && note.posGet() - audiocontroller.MusicTime <= -0.2)
        {
            Debug.Log("Miss!");
            waitTime = audiocontroller.MusicTime;
            text.text = "不可";
            text.enabled = true;
            audiocontroller.ComboReset();
            Notes.Remove(Notes.Last());
            Destroy(note.gameObject);
        }

        if(audiocontroller.MusicTime - waitTime > 0.15f)
        {
            text.enabled = false;
        }
    }

    void Note_Jadge(Note note)
    {
            Debug.Log(note.posGet());
        if (Mathf.Abs(audiocontroller.MusicTime - note.posGet()) < 0.07)
        {
            Debug.Log("Great!");
            waitTime = audiocontroller.MusicTime;
            text.text = "良";
            text.enabled = true;
            audioSource.clip = TapSound;
            audiocontroller.ComboPlus();
            audiocontroller.ScoreSet((int)(100 * audiocontroller.ComboBonus));
            Notes.Remove(Notes.Last());
            Destroy(note.gameObject);
        }
        else if (Mathf.Abs(audiocontroller.MusicTime - note.posGet()) < 0.12)
        {
            Debug.Log("Nice!");
            waitTime = audiocontroller.MusicTime;
            text.text = "可";
            text.enabled = true;
            audioSource.clip = TapSound;
            audiocontroller.ComboPlus();
            audiocontroller.ScoreSet((int)(75 * audiocontroller.ComboBonus));
            Notes.Remove(Notes.Last());
            Destroy(note.gameObject);
        }
        else if (Mathf.Abs(audiocontroller.MusicTime - note.posGet()) < 0.2)
        {
            Debug.Log("Bad!");
            waitTime = audiocontroller.MusicTime;
            text.text = "不可";
            text.enabled = true;
            audioSource.clip = MissSound;
            audiocontroller.ComboReset();
            Notes.Remove(Notes.Last());
            Destroy(note.gameObject);
        }
    }
}