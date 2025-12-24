using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float dashSpeed = 6.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 地面判定
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 地面に吸い付かせるための微小な下向き速度
        }

        // 左スティック左右
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // ダッシュ
        bool isDashing = Input.GetButton("Dash");

        // ジャンプ
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // 移動量
        float currentSpeed = isDashing ? dashSpeed : walkSpeed;
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // 移動
        if (move.magnitude > 0.1f)
        {
            controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        }

        // 重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}