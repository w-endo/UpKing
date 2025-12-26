using System.Data;
using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    float timeLeft = 180f;
    public TextMeshProUGUI timeText;


    public enum GameState
    {
        TAKE,   //王冠獲得ターン
        KING    //王様ターン
    }
    GameState state = GameState.TAKE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "" + (int)timeLeft / 60 + ":" + (int)timeLeft % 60;


        timeLeft -= Time.deltaTime;
    }


    //王様ターンへ移行
    public void ChangeState()
    {
        timeLeft = 120;
        state = GameState.KING;
    }
}
