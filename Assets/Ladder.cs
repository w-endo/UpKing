using UnityEngine;

public class Ladder : MonoBehaviour
{
    int life = 2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage()
    {
        life--;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
