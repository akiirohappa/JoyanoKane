using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.Audio;

public class MainSaveData : SingletonMonoBehaviour<MainSaveData>
{
    [SerializeField]
    private List<string> PlayMusicNames;
    [SerializeField]
    [Range(-80,20)]
    private int BGM_Volume = 0, SE_Volume = 0;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private int NoteSpeed = 5;


    private AudioSource SE;

    private AsyncOperation async;
    private Canvas canvas;

    private string musicName;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        if(!Directory.Exists(Application.dataPath + "/Resources/HumenData")) Directory.CreateDirectory(Application.dataPath + "/Resources/HumenData");
        if (!Directory.Exists(Application.dataPath + "/Resources/Music")) Directory.CreateDirectory(Application.dataPath + "/Resources/Music");
        //配列の初期化
        PlayMusicNames = new List<string>();
        FileLoad();
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        canvas = GameObject.FindGameObjectWithTag("LoadUI").GetComponent<Canvas>();
        canvas.GetComponent<Canvas>().enabled = false;
        SE = gameObject.GetComponent<AudioSource>();
        audioMixer.SetFloat("Music_Volume", BGM_VolumeGet());
        audioMixer.SetFloat("Effect_Volume", SE_VolumeGet());
    }

    //jsonからデータを読み込む関数
    public void FileLoad()
    {
        if (File.Exists(Application.dataPath + "/MainSaveData.json"))
        {
            string loadjson = File.ReadAllText(Application.dataPath + "/MainSaveData.json");
            JsonUtility.FromJsonOverwrite(loadjson, instance);
            Debug.Log("File Load");
        }
        else
        {
            instance.FileSave();
            Debug.Log("No File");
        }
    }

    //データをjsonに保存する関数
    public void FileSave()
    {
        string savejson = JsonUtility.ToJson(instance);
        File.WriteAllText(Application.dataPath + "/MainSaveData.json", savejson);
        Debug.Log("File Save");
    }

    public void MusicNamesAdd(string name)
    {
        if (PlayMusicNames.Count == 0) PlayMusicNames.Add(name);
        else
        {
            for(int x = 0; x < PlayMusicNames.Count; x++)
            {
                if(PlayMusicNames[x] == name)
                {
                    PlayMusicNames.Remove(PlayMusicNames[x]);
                    break;
                }
            }
            PlayMusicNames.Add(name);
        }
    }

    public List<string> MusicNamesGet()
    {
        return instance.PlayMusicNames;
    }

    public void MusicNameSet(string name)
    {
        instance.musicName = name;
    }

    public string MusicNameGet()
    {
        return instance.musicName;
    }

    //BGM_Volumeを変更する関数
    public void BGM_VolumeSet(int i)
    {
        instance.BGM_Volume = i;
    }

    //SE_Volumeを変更する関数
    public void SE_VolumeSet(int i)
    {
        instance.SE_Volume = i;
    }

    //BGM_Volumeを取得する関数
    public int BGM_VolumeGet()
    {
        return instance.BGM_Volume;
    }

    //SE_Volumeを取得する関数
    public int SE_VolumeGet()
    {
        return instance.SE_Volume;
    }

    public void MusicSet(string s)
    {
        musicName = s;
    }

    public string MusicGet()
    {
        return musicName;
    }

    public int NoteMoveGet()
    {
        return NoteSpeed;
    }

    public void NoteMoveSet(int x)
    {
        NoteSpeed = x;
    }

    //シーンをロードする関数
    IEnumerator Load(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        canvas.GetComponent<Canvas>().enabled = true;
        while (!async.isDone)
        {
            yield return null;
        }
        canvas.GetComponent<Canvas>().enabled = false;
    }

    //効果音を再生する関数
    public void SEPlay(AudioClip audioClip)
    {
        SE.PlayOneShot(audioClip);
    }
}