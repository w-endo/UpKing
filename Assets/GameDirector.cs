using System.Data;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    float timeLeft = 180f;
    public TextMeshProUGUI timeText;

    int plaerCount = 0;
    public int winner = -1;


    public enum GameState
    {
        TAKE,   //王冠獲得ターン
        KING    //王様ターン
    }
    GameState state = GameState.TAKE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        DontDestroyOnLoad(gameObject);

        //プレイヤーの数
        plaerCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    // Update is called once per frame
    void Update()
    {
        //残り時間表示
        timeText.text = "" + (int)timeLeft / 60 + ":" + (int)timeLeft % 60;
        timeLeft -= Time.deltaTime;


        if(timeLeft <= 0)
        {
            if (state == GameState.TAKE)
            {
                //王冠を消す
                Destroy(GameObject.FindWithTag("Crown"));
            }
        }
    }


    //王様ターンへ移行
    public void ChangeState()
    {
        timeLeft = 120;
        state = GameState.KING;
    }


    public void Death()
    {
        plaerCount--;
        if(plaerCount <= 1)
        {
            winner = GameObject.FindWithTag("Player").GetComponent<PlayerController>().ID;

            SceneManager.LoadScene("ResultScene");
        }
    }
}
