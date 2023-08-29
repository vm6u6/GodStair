using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 0.8597
// 8.5774


public class main_actor : MonoBehaviour
{
    [SerializeField] private GameObject power_bar;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private float moveSpeed = 1.5f;              
    private int max_floor = 0; 
    private int cnt_floor_certification = 0;   
    private int activateCount = 0;
    private float jumpForce = 5f;              
    private int moveDirection = 1;            
    private bool isJumping = false;           
    private bool islanding = true; 
    private SpriteRenderer spriteRenderer;    
    private Rigidbody2D rb;                   
    private float currentPressTime = 0;
    private float timeOnGround_start = 0f;
    private float timeOnGround_end = 0f;
    private float overJumpingTime = 0.3f;

    

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Camera mainCamera = Camera.main;
        Main_camera cameraFollow = mainCamera.GetComponent<Main_camera>();
        cameraFollow.target = transform;
    }

    void Update(){   

        // { Movement }_________________________________________________________________________
        float currentSpeed = moveSpeed;
        if (rb.velocity.y == 0f){
            if (Input.GetKey(KeyCode.Space)){
                currentSpeed *= 0.6f;
            }
        }
        
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);

        // { Jumping }_________________________________________________________________________
        if (rb.velocity.y == 0f){
            timeOnGround_end = Time.time;
        
            if (Input.GetKey(KeyCode.Space) && islanding){
                currentPressTime += Time.deltaTime;
                update_powerBar(currentPressTime);
            }
            if (timeOnGround_end - timeOnGround_start > overJumpingTime){
                if (Input.GetKeyDown(KeyCode.Space) && !isJumping && islanding){
                    isJumping = true;
                }

                if (Input.GetKeyUp(KeyCode.Space) && islanding){
                    isJumping = false;
                    StartJump();
                }
            }
        }
        
        cnt_floor();
    }

    private void StartJump(){
        Debug.Log(activateCount);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + activateCount / 2);
        islanding = false;
        jumpForce = 5f;
        activateCount = 0;
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
            timeOnGround_start = Time.time;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
            if (hit.collider != null)
            {
                currentPressTime = 0;
                isJumping = false;
                islanding = true;
                reset_powerBar(); 
            }
        }
    }

    private void cnt_floor(){
        int now_pos = Mathf.FloorToInt(transform.position.y);
        if (now_pos > cnt_floor_certification)
        {
            max_floor += 1;
            cnt_floor_certification += 3;
            textMeshPro.text = max_floor.ToString("D4") + "F";

            moveSpeed += 0.2f;
        }
    }

    private void update_powerBar(float pressTime_)
    {
        int L = power_bar.transform.childCount;
        activateCount = Mathf.FloorToInt(pressTime_ / 0.04f);
        if (activateCount > 12){
            activateCount = activateCount % 12;
        }

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