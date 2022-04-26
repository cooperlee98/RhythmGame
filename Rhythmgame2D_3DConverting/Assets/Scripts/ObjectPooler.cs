using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    //이중 리스트 사용하기
    //리스트1- 노트10개, 리스트2- 노트 10개 ....
    //리스트들의 리스트=이중 리스트/2차원 배열 활용
    public List<GameObject> Notes;
    private List<List<GameObject>> poolsOfNotes;
    public int noteCount = 10;
    private bool more = true;


    void Start()
    {
        poolsOfNotes = new List<List<GameObject>>();
        for(int i=0; i < Notes.Count; i++)// 4번 반복
        {
            poolsOfNotes.Add(new List<GameObject>());
            for(int n=0; n<noteCount; n++)//10번 반복
            {
                GameObject obj = Instantiate(Notes[i]);
                obj.SetActive(false);//생성과 삭제 대신 활성과 비활성화 false 는 비활성화
                poolsOfNotes[i].Add(obj);
            }
        }
        
    }

    public GameObject getObject(int noteType)
    {
        foreach(GameObject obj in poolsOfNotes[noteType - 1])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        if (more)
        {
            GameObject obj = Instantiate(Notes[noteType - 1]);
            poolsOfNotes[noteType - 1].Add(obj);
            return obj;
        }
        return null;//오류 검출용, 거의 사용 x
    }

    void Update()
    {
        
    }
}
