using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 0.8597
// 8.5774


public class main_actor : MonoBehaviour
{
    public float moveSpeed = 1f;              // 移動速度

    public int max_floor = 0;    
    public float downTime, upTime, pressTime = 0;
    public float jumpForce = 5f;              // 起始跳躍力量
    private int moveDirection = 1;            // 移動方向，1代表向右，-1代表向左
    private bool isJumping = false;           // 是否正在跳躍 Ready
    private bool islanding = true; 
    private SpriteRenderer spriteRenderer;    // SpriteRenderer組件
    private Rigidbody2D rb;                   // Rigidbody2D組件
    private float currentPressTime = 0;
    [SerializeField] private GameObject power_bar;
    

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Camera mainCamera = Camera.main;
        Main_camera cameraFollow = mainCamera.GetComponent<Main_camera>();
        cameraFollow.target = transform;
    }

    void Update(){   
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed *= 0.6f;
        }
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);

        // 處理跳躍
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && islanding)
        {
            downTime = Time.time;
            currentPressTime = 0;
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.Space) && islanding)
        {
            currentPressTime += Time.deltaTime;
            update_powerBar(currentPressTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && islanding)
        {
            upTime = Time.time;
            pressTime = upTime - downTime;
            if (pressTime > 3)
                pressTime = 3;
            isJumping = false;
            StartJump();
        }
    }

    private void StartJump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + pressTime * 2f);
        islanding = false;
        pressTime = 0;
        jumpForce = 5f;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("right_border")){
            moveDirection = -1;
            spriteRenderer.flipX = true;
        }
        else if(collision.gameObject.CompareTag("left_border")){
            moveDirection = 1;
            spriteRenderer.flipX = false;
        }

        if (collision.gameObject.CompareTag("floor") || 
            collision.gameObject.CompareTag("stair") || 
            collision.gameObject.CompareTag("acc_stair") && 
            rb.velocity.y == 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);

            if (hit.collider != null)
            {
                isJumping = false;
                islanding = true;
                reset_powerBar(); // 重置power_bar
            }
        }
    }

    private void cnt_floor(){
        int now_pos = Mathf.FloorToInt(transform.position.y);
        if (now_pos > max_floor)
            max_floor = now_pos;
    }

    private void update_powerBar(float pressTime_)
    {
        int L = power_bar.transform.childCount;
        int activateCount = Mathf.FloorToInt(pressTime_ / 0.25f);

        for (int i = 0; i < L; i++)
        {
            power_bar.transform.GetChild(i).gameObject.SetActive(i < activateCount);
        }
    }

    private void reset_powerBar()
    {
        int L = power_bar.transform.childCount;
        for (int i = 0; i < L; i++)
        {
            power_bar.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}