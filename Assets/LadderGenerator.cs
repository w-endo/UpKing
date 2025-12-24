using UnityEngine;

public class LadderGenerator : MonoBehaviour
{
    public GameObject ladderPrefab;
    public float coolTime = 3.0f;

    float delta = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delta -= Time.deltaTime;

        if(delta <= 0.0f)
        {
            int count = Random.Range(2, 5);
            for(int i = 0; i < count; i++)
            {
                Vector3 position = new Vector3(
                    Random.Range(-7.0f, 7.0f),
                    transform.position.y,
                    0.0f
                );
                Instantiate(ladderPrefab, position, Quaternion.identity);
            }

            delta = coolTime;
        }
    }
}
