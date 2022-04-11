using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public int noteType;
    private GameManager.judges judge;
    private KeyCode keyCode;

    void Start()
    {
        if (noteType == 1) keyCode=KeyCode.D;
        else if(noteType == 2) keyCode = KeyCode.F;
        else if (noteType == 3) keyCode = KeyCode.J;
        else if (noteType == 4) keyCode = KeyCode.K;
    }

    public void Initialize()//노트가 처음 만들어졌는데(오브젝트 풀링) 판정값이 다 미스가 되지 않도록 초기화!
    {
        judge = GameManager.judges.NONE;
    }
    void Update()
    {
        transform.Translate(Vector3.down * GameManager.instance.noteSpeed);

        if (Input.GetKey(keyCode))//사용자가 노트키를 입력하면 해당 노트에 대한 판정 진행
        {
            GameManager.instance.processJudge(judge, noteType);
            Debug.Log(judge);
            //if (judge != GameManager.judges.NONE) Destroy(gameObject);//노트가 판정 선에 닿기 시작한 이후엔 해당노트 제거
            if (judge != GameManager.judges.NONE) gameObject.SetActive(false);//오브젝트 풀링에서 비활성화! 위 코드는 이제 사용x
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Bad Line")
        {
            judge = GameManager.judges.BAD;
        }
        else if (other.gameObject.tag == "Good Line")
        {
            judge = GameManager.judges.GOOD;
        }
        else if (other.gameObject.tag == "Perfect Line")
        {
            judge = GameManager.judges.PERFECT;
        }
        else if (other.gameObject.tag == "Miss Line")
        {
            judge = GameManager.judges.MISS;
            GameManager.instance.processJudge(judge, noteType);//노트가 미스가 되었을 때 콤보가 끊이기 위해 다시 한번 더 넣어줌
            //원래 이 다음줄에 destroy가 있었나?
            gameObject.SetActive(false);//오브젝트 풀링, 비활성화
        }
    }
}
