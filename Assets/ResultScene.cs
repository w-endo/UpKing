using TMPro;
using UnityEngine;

public class ResultScene : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winnerText.text = "" + GameObject.Find("GameDirector").GetComponent<GameDirector>().winner + "P WIN!";
    }


}
