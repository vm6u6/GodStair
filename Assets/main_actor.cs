using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main_actor : MonoBehaviour
{
    [SerializeField] private GameObject power_bar;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject abortButton;
    [SerializeField] private GameObject pauseLab;

    private float moveSpeed = 1.5f;              
    private int max_floor = 0; 
    private float cnt_floor_certification = 0.0f;   
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
    private float overJumpingTime = 0.1f;
    private float endGame_Line = - 7.7177f;
    private float fallMutiplier = 2.0f;
    // private float lowJumpMutiplier = 2.5f;
    private bool pause_init = true;

    

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Camera mainCamera = Camera.main;
        Main_camera cameraFollow = mainCamera.GetComponent<Main_camera>();
        cameraFollow.target = transform;

    }

    void Update(){  
        if (pause_init == false) {
            return;
        }
        endGame_Line = Camera.main.transform.position.y - 5.0277f;

        // { Movement }_________________________________________________________________________
        float currentSpeed = moveSpeed;
        if (rb.velocity.y == 0f){
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)){
                currentSpeed *= 0.6f;
            }
        }
        
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);

        // { Jumping }_________________________________________________________________________
        if (rb.velocity.y == 0f){
            Debug.Log("000");
            timeOnGround_end = Time.time;
            if (timeOnGround_end - timeOnGround_start > overJumpingTime){
                 if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) && islanding ){
                    currentPressTime += Time.deltaTime;
                    update_powerBar(currentPressTime);
                }
                if (Input.GetKeyDown(KeyCode.Space)  || Input.GetMouseButtonDown(0) && !isJumping && islanding){
                    isJumping = true;
                }

                if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0) && islanding ){
                    isJumping = false;
                    StartJump();
                }
            }
        }
        if (rb.velocity.y < 0){
            Debug.Log("Down");
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0){
            Debug.Log("Up");
            // TODO 補上跳躍物理引擎
        }
        cnt_floor();
        EndGmae();
    }

    private void EndGmae(){
        // Debug.Log(transform.position.y);
        if (transform.position.y < endGame_Line){
            Time.timeScale = 0;
        }
    }


    private void StartJump(){
        //Debug.Log(activateCount);
        rb.velocity = new Vector2(rb.velocity.x, (jumpForce + activateCount / 4)) ;
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
            cnt_floor_certification += 7.7177f/2;
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

    public void pause(){
        if (pause_init){
            pauseLab.SetActive(true);
            Time.timeScale = 0;
            pause_init = false;

        }
        else{
            pauseLab.SetActive(false);
            Time.timeScale = 1;
            pause_init = true;
        }
    }
}