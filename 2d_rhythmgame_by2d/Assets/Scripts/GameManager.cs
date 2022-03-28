using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }//게임매니저를 싱글 톤으로 처리함 

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public float noteSpeed;

    /*
     * bad:1
     * good:2
     * perfect:3
     * miss:4
     */

    public enum judges { NONE=0, BAD,GOOD, PERFECT, MISS};



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
