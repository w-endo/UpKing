using UnityEngine;
using UnityEngine.InputSystem.XR;

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
    float ladderRootY = 0f;

    public int ladderCount = 0; // 接触している梯子の数
    public bool isOnLadder => ladderCount > 0; // プロパティにすると便利です

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Ground"), false);

    }

    void Update()
    {
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
                //ladderCount--;

                Debug.Log("Stop Climbing");
            }

            // はしご中の垂直移動
            velocity.y = moveY * ladderSpeed;

            controller.Move(velocity * Time.deltaTime); 


            if(moveY < 0 && isLadderRoot && transform.position.y < ladderRootY)
            {
                isClimbing = false;
                Vector3 pos = transform.position;
                pos.z = 0;
                controller.enabled = false;
                transform.position = pos;
                controller.enabled = true;
                ladderCount--;

                Debug.Log("Stop Climbing at Ladder Root");
            }
        }


        //地面の上
        else
        {   
           if(isOnLadder)
            {
                if(Mathf.Abs( moveY) > 0.1)
                {
                    Vector3 pos = transform.position;
                    pos.z = -1;

                    controller.enabled = false;
                    transform.position = pos;
                    controller.enabled = true;
                    isClimbing = true;
                    ladderCount--;

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

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            ladderCount++;
            if (other.CompareTag("LadderRoot"))
            {
                isLadderRoot = true;
                ladderRootY = other.transform.position.y;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder") || other.CompareTag("LadderRoot"))
        {
            ladderCount--;

            // 負の数にならないよう念のためクランプ（予期せぬ挙動対策）
            if (ladderCount < 0) ladderCount = 0;

            // 全ての梯子から離れた時のみ、速度をリセット
            if (ladderCount == 0)
            {
                velocity.y = 0;
            }

            if (other.CompareTag("LadderRoot"))
            {
                isLadderRoot = false;
            }
        }
    }
}