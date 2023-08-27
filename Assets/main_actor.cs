using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0.8597
// 8.5774


public class main_actor : MonoBehaviour
{
    public float moveSpeed = 0.2f;            // 移動速度
    public int max_floor = 0;    
    public float jumpHoldDuration = 0.5f;     // 按住空白鍵的最大持續時間   
    public float downTime, upTime, pressTime = 0;
    public float jumpForce = 5f;              // 起始跳躍力量
    private int moveDirection = 1;            // 移動方向，1代表向右，-1代表向左
    private bool isJumping = false;           // 是否正在跳躍 Ready
    private bool islanding = true; 
    private SpriteRenderer spriteRenderer;    // SpriteRenderer組件
    private Rigidbody2D rb;                   // Rigidbody2D組件
    [SerializeField] private GameObject power_bar;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // 獲取SpriteRenderer和Rigidbody2D組件
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        Camera mainCamera = Camera.main;

        // 获取相机跟随脚本组件
        Main_camera cameraFollow = mainCamera.GetComponent<Main_camera>();

        // 设置相机跟随的目标为角色对象
        cameraFollow.target = transform;
    }

    // Update is called once per frame
    void Update()
    {   
        // 計算移動的位移
        float movement = moveDirection * moveSpeed * Time.deltaTime;

        // 更新角色位置
        transform.Translate(movement, 0f, 0f);

        // 檢查是否超出邊界
        if (transform.position.x < -5.0f )  // 超出左邊界
        {
            moveDirection = 1; // 改變移動方向為向右
        }
        else if (transform.position.x > 2.3f )  // 超出右邊界
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
            if (pressTime > 10)
                pressTime = 10;
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
        // 防止空中跳躍，挑一次後要碰到下一個物體才能再跳
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("stair") || collision.gameObject.CompareTag("acc_stair") && rb.velocity.y == 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);

            if (hit.collider != null)
            {
                isJumping = false;
                islanding = true;
            }
        }
    }

    private void cnt_floor(){
        int now_pos = Mathf.FloorToInt(transform.position.y);
        if (now_pos > max_floor)
            max_floor = now_pos;
    }

    // private void update_powerBar(){
    //     int L = power_bar.transform.childCOunt;

    // }
}