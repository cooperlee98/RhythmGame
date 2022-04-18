using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteController : MonoBehaviour
{
    //하나의 노트에 대한 정보를 담는 노트(Note) 클래스를 정의
    class Note
    {
        public int noteType { get; set; }
        public int order { get; set; }
        public Note(int noteType, int order)
        {
            this.noteType = noteType;
            this.order = order;
        }
    }

    public GameObject[] Notes;
    private List<Note> notes = new List<Note>();
    //private float beatInterval = 1.0f;

    private ObjectPooler noteObjectPooler;
    private float x, z, startY = 8.0f;

    void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);
        //설정된 시작 라인으로 노트를 이동시킴
        x = obj.transform.position.x;
        z = obj.transform.position.z;
        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }


    private string musicTitle; //텍스트파일 첫번째줄
    private string musicArtist; //두번째줄
    private int bpm;
    private int divider;
    private float startingPoint; //텍스트 파일에서 세번째 줄까지 해당!
    private float beatCount;
    private float beatInterval;

    IEnumerator AwaitMakeNote(Note note)
    {
        int noteType = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds(startingPoint+order * beatInterval);//몇초 동안 기다린 후, 아래 명령을 실행해라+startingPoint만큼 더 기다리기

        //Instantiate(Notes[noteType - 1]);//배열이므로 0번째 노트부터 나와야 함---- 빼기 1!
        MakeNote(note);
    }
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        /*notes.Add(new Note(1, 1));
        notes.Add(new Note(2, 2));
        notes.Add(new Note(3, 3));
        notes.Add(new Note(4, 4));
        notes.Add(new Note(1, 5));
        notes.Add(new Note(2, 6));
        notes.Add(new Note(3, 7));
        notes.Add(new Note(4, 8));
        */
        //리소스에서 비트 텍스트 파일을 불러와야함
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + GameManager.instance.music);
        StringReader reader = new StringReader(textAsset.text);
        musicTitle = reader.ReadLine();
        musicArtist = reader.ReadLine();
        string beatInformation = reader.ReadLine();//3번째 줄에 적힌 정보를 읽음
        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float)Convert.ToDouble(beatInformation.Split(' ')[2]);
        beatCount = (float)bpm / divider;//1초마다 떨어지는 비트개수
        beatInterval = 1 / beatCount;//비트가 떨어지는 간격 시간

        string line;//각 비트들이 떨어지는 위치 및 시간정보를 읽음
        while((line=reader.ReadLine()) != null)
        {
            Note note = new Note(Convert.ToInt32(line.Split(' ')[0]) + 1,Convert.ToInt32(line.Split(' ')[1]));  //위치 0에1을 더해야 1번 라인, ....
            notes.Add(note);
        }
        //모든 노트를 정해진 시간에 출발하도록 설정
        for(int i=0; i<notes.Count; i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));//마지막 노트를 기준으로 게임 종료 함수를 불러옴

    }

    IEnumerator AwaitGameResult(int order)
    {
        yield return new WaitForSeconds(startingPoint + order * beatInterval+8.0f);// 마지막 노트로부터 8초 뒤까지 기다림
        GameResult();
    }

    void GameResult()
    {
        PlayerInformation.maxCombo = GameManager.instance.maxCombo;
        PlayerInformation.score = GameManager.instance.score;
        PlayerInformation.musicTitle=musicTitle;
        PlayerInformation.musicArtist=musicArtist;
        SceneManager.LoadScene("GameResultScene");
    }
 
    void Update()
    {
        
    }
}
