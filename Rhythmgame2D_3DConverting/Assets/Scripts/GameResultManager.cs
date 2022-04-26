using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;

    void Start()
    {
        //Debug.log(-----); 를 사용하여 정상적으로 정보가 넘어오는지 확인 가능
        musicTitleUI.text = PlayerInformation.musicTitle;
        scoreUI.text = "" + PlayerInformation.score;
        maxComboUI.text = "" + PlayerInformation.maxCombo;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
