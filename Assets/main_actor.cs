using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_actor : MonoBehaviour
{
    public float moveSpeed = 0.8f;              // 移動速度
    public float jumpForce = 5f;              // 起始跳躍力量
    public float jumpHoldDuration = 0.5f;     // 按住空白鍵的最大持續時間   
    public float downTime, upTime, pressTime = 0;

    private int moveDirection = 1;            // 移動方向，1代表向右，-1代表向左
    private bool isJumping = false;           // 是否正在跳躍 Ready
    private bool islanding = true; 
    private SpriteRenderer spriteRenderer;    // SpriteRenderer組件
    private Rigidbody2D rb;                   // Rigidbody2D組件

    // Start is called before the first frame update
    void Start()
    {
        // 獲取SpriteRenderer和Rigidbody2D組件
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
void Update()
    {   
        // 計算移動的位移
        float movement = moveDirection * moveSpeed * Time.deltaTime;

        // 更新角色位置
        transform.Translate(movement, 0f, 0f);

        // 檢查是否超出邊界
        if (transform.position.x < -4.3f )  // 超出左邊界
        {
            moveDirection = 1; // 改變移動方向為向右
        }
        else if (transform.position.x > 4.4f )  // 超出右邊界
        {
            moveDirection = -1; // 改變移動方向為向左
        }

        // 根據水平輸入翻轉圖片
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false; // 不翻轉圖片
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true; // 翻轉圖片
        }

        // 處理跳躍
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && islanding)
        {
            downTime = Time.time;
            isJumping = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && islanding)
        {
            upTime = Time.time;
            pressTime = upTime - downTime;
            isJumping = false;
            StartJump();
        }
    }

    // 開始跳躍
    private void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + pressTime * 2f);
        islanding = false;
        pressTime = 0;
        jumpForce = 5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("stair") || collision.gameObject.CompareTag("acc_stair") && rb.velocity.y == 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f); // 往下發射一個長度為1的射線

            if (hit.collider != null) // 如果射線有碰撞到其他物體
            {
                isJumping = false;
                islanding = true;
            }
        }
    }
}