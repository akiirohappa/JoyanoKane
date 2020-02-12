using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private float DefaultXpos = -9f;
    [SerializeField]
    private float notePos;
    [SerializeField]
    private Audiocontroller audiocontroller;

    // Start is called before the first frame update
    void Start()
    {
//        DefaultYpos = -4.2f;
    }

    // Update is called once per frame
    void Update()
    {
        NoteMove();
    }

    public void posSet(float f)
    {
        notePos = f;
    }

    public float posGet()
    {
        return notePos;
    }

    public virtual void NoteSetting()
    {
        audiocontroller = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audiocontroller>();
    }

    public virtual void NoteMove()
    {
        transform.position = new Vector3(DefaultXpos + (notePos - audiocontroller.MusicTime) * MainSaveData.Instance.NoteMoveGet(),transform.position.y , transform.position.z);
    }
}
