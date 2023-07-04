using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_actor : MonoBehaviour
{
    public float moveSpeed = 5f;              // 移動速度
    public float jumpForce = 5f;              // 起始跳躍力量
    public float jumpHoldForce = 2.5f;        // 按住空白鍵時的額外跳躍力量
    public float jumpHoldDuration = 0.5f;     // 按住空白鍵的最大持續時間
    

    public float downTime, upTime, pressTime = 0;
    public float countDown = 2.0f;

    private float jumpTimeCounter;            // 跳躍計時器
    private bool isJumping = false;           // 是否正在跳躍 Ready
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
        // 獲取玩家的輸入
        float horizontalInput = Input.GetAxis("Horizontal");

        // 計算移動的方向
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;

        // 更新角色位置
        transform.position += movement;

        // 根據水平輸入翻轉圖片
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; // 不翻轉圖片
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; // 翻轉圖片
        }

        // 按住空白鍵時跳得更高
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                ContinueJump();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        // 處理跳躍
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            downTime = Time.time;
            pressTime = downTime + countDown;
            ready = true;
            
        }
        if (Input.GetKeyUp (KeyCode.Space)) {
            ready = false;
            
        }
        if (Time.time >= pressTime && ready == true) {
            isJumping = false;
            StartJump();
        }	

        // 跳躍計時器歸零
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    // 開始跳躍
    private void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        jumpTimeCounter = jumpHoldDuration;
    }

    // 繼續跳躍（按住空白鍵）
    private void ContinueJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + jumpHoldForce);
    }

    // 檢測地面碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("stair") || collision.gameObject.CompareTag("acc_stair"))
        {
            isJumping = false;
        }
    }
}