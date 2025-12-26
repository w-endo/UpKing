using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public int direction = 1; // 1: right, -1: left

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);



        if (transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //’òŽq‚É“–‚½‚Á‚½
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            other.GetComponent<Ladder>().Damage();
            Destroy(gameObject);
        }
    }
}
