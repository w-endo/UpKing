using UnityEngine;

public class LadderItem : MonoBehaviour
{
    public float speed = 3.0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.down * 3.0f * Time.deltaTime);

        if(this.transform.position.y < -speed)
        {
            Destroy(this.gameObject);
        }
    }
}
