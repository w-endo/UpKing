using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3.0f;
    public float dashSpeed = 6.0f;
    public float ladderSpeed = 3.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public bool isClimbing = false;
    public bool isLadderRoot = false;
    //float ladderRootY = 0f;
    GameObject ladderRootObject = null;


    public GameObject ladderPrefab;




    private List<Collider> activeLadders = new List<Collider>();
    public bool isOnLadder => activeLadders.Count > 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ground"), false);

    }

    void Update()
    {
        // 【重要】ワープや消滅対策：無効になったコライダーをリストから削除
        if (activeLadders.Count > 0)
        {
            activeLadders.RemoveAll(l => l == null || !l.enabled || !l.gameObject.activeInHierarchy);
        }



        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;


        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        bool isDashing = Input.GetButton("Dash") || Input.GetKey(KeyCode.LeftShift);


        //梯子を上ってる
        if (isClimbing)
        {
            //梯子から離れた
            if(isOnLadder == false)
            {
                isClimbing = false;
                Vector3 pos = transform.position;
                pos.z = 0;
                controller.enabled = false;
                transform.position = pos;
                controller.enabled = true;


                Debug.Log("Stop Climbing");
            }

            // はしご中の垂直移動
            velocity.y = moveY * ladderSpeed;

            controller.Move(velocity * Time.deltaTime); 


            if(moveY < 0 && isLadderRoot && transform.position.y <= ladderRootObject.transform.position.y)
            {
                isClimbing = false;
                Vector3 pos = transform.position;
                pos.z = 0;
                pos.y = ladderRootObject.transform.position.y;
                controller.enabled = false;
                transform.position = pos;
                controller.enabled = true;


                Debug.Log("Stop Climbing at Ladder Root");
            }
        }


        //地面の上
        else
        {   
           if(isOnLadder)
            {
                if((isLadderRoot ==true && moveY > 0.2) || (isLadderRoot == false && moveY < -0.2))
                {
                    Vector3 pos = transform.position;
                    pos.z = -1;

                    controller.enabled = false;
                    transform.position = pos;
                    controller.enabled = true;
                    isClimbing = true;

                    Debug.Log("Start Climbing");
                }
            }


            // --- 移動計算 ---
            float currentSpeed = isDashing ? dashSpeed : walkSpeed;
            Vector3 horizontalMove = new Vector3(moveX, 0, 0);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;

            controller.Move(horizontalMove.normalized * currentSpeed * Time.deltaTime);
            controller.Move(velocity * Time.deltaTime);
        }

        //梯子設置
        if(Input.GetButtonDown("Setup"))
        {
            GameObject ladder = Instantiate(ladderPrefab);

            //梯子に触れてる
            if (isLadderRoot)
            {
                ladder.transform.parent = ladderRootObject.transform;
                ladder.transform.localPosition = new Vector3(0, ladderRootObject.transform.childCount-1, 0);
            }
            else
            {
                float y = (int)(transform.position.y - 0.3f) + 0.5f;
                ladder.transform.position = new Vector3(transform.position.x, y, 0);
                ladder.tag = "LadderRoot";
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            if (!activeLadders.Contains(other))
            {
                activeLadders.Add(other);
            }

            if (other.CompareTag("LadderRoot"))
            {
                isLadderRoot = true;
                ladderRootObject = other.gameObject;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            activeLadders.Remove(other);


            if (other.CompareTag("LadderRoot"))
            {
                isLadderRoot = false;
            }
        }
    }
}