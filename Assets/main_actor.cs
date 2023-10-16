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
    [SerializeField] private GameObject pannel;
    [SerializeField] private GameObject font;
    [SerializeField] private GameObject end;
    [SerializeField] private GameObject regist;
    [SerializeField] private GameObject level;
    [SerializeField] private GameObject start_game;

    private float moveSpeed = 1.5f;              
    public int max_floor = 0; 
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
    // private float fallMutiplier = 2.0f;
    // private float lowJumpMutiplier = 2.5f;
    private bool pause_init = true;
    private int floor_type = 0; // {0: normal, 1: jump, 2:acc}
    private int acc_dir = 0; // 0:right , 1:left
    

    void Start(){
        Time.timeScale = 0;
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

            if (floor_type == 2){
                if (acc_dir == 1 && moveDirection == 1){
                    currentSpeed *= 1.3f;
                }
                if (acc_dir == 1 && moveDirection == -1){
                    currentSpeed *= 0.3f;
                }
                if (acc_dir == -1 && moveDirection == 1){
                    currentSpeed *= 0.3f;
                }
                if (acc_dir == -1 && moveDirection == -1){
                    currentSpeed *= 1.3f;
                }
            }
        }

    
        
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);


        // { Jumping }_________________________________________________________________________
        if (rb.velocity.y == 0f){
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
        // if (rb.velocity.y < 0){
        //     Debug.Log("Down");
        //     rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        // }
        // if (rb.velocity.y > 0){
        //     Debug.Log("Up");
        //     // TODO 補上跳躍物理引擎
        // }
        cnt_floor();
        EndGmae();
    }

    private void EndGmae(){
        // Debug.Log(transform.position.y);
        if (transform.position.y < endGame_Line){
            Time.timeScale = 0;
            pannel.SetActive(true);
            font.SetActive(true);
            end.SetActive(true);
            regist.SetActive(true);
            level.SetActive(true);
            start_game.SetActive(true);
            SceneManager.LoadScene(0);
        }
    }

    private void StartJump(){
        // Debug.Log("FLOOR" + floor_type);
        if (floor_type == 0){
            rb.velocity = new Vector2(rb.velocity.x, (jumpForce + activateCount / 4)) ;
            islanding = false;
            jumpForce = 5f;
            activateCount = 0;

        }else if (floor_type == 1){
            rb.velocity = new Vector2(rb.velocity.x, (jumpForce + activateCount)) ;
            islanding = false;
            jumpForce = 5f;
            activateCount = 0;

        }else if (floor_type == 2){
            rb.velocity = new Vector2(rb.velocity.x, (jumpForce + activateCount / 4)) ;
            islanding = false;
            jumpForce = 5f;
            activateCount = 0;
        }

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
            collision.gameObject.CompareTag("jump_stair") ||
            collision.gameObject.CompareTag("acc_stair_right") ||
            collision.gameObject.CompareTag("acc_stair_left") ||
            collision.gameObject.CompareTag("stair_move") || 
            collision.gameObject.CompareTag("jump_stair_move") ||
            collision.gameObject.CompareTag("acc_stair_right_move") ||
            collision.gameObject.CompareTag("acc_stair_left_move")){

            timeOnGround_start = Time.time;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f);
            // Debug.Log(hit.collider.tag);
        
            if (hit.collider != null)
            {
                Vector2 collisionNormal = hit.normal;
                // Check if the collision normal is pointing upwards
                if (collisionNormal == Vector2.up)
                {
                    // Debug.Log( Vector2.up );
                    // Debug.Log("Collision detected with tag: " + collision.gameObject.tag);
                    if (collision.gameObject.tag == "floor" || 
                        collision.gameObject.tag == "stair" || 
                        collision.gameObject.tag == "stair_move"){
                        floor_type = 0;
                    }else if (collision.gameObject.tag == "jump_stair" || collision.gameObject.tag == "jump_stair_move"){
                        floor_type = 1;
                    }
                    else if (collision.gameObject.tag == "acc_stair_right" ||     
                             collision.gameObject.tag == "acc_stair_left" || 
                             collision.gameObject.tag == "acc_stair_right_move" ||     
                             collision.gameObject.tag == "acc_stair_left_move"){
                        if (collision.gameObject.tag == "acc_stair_right" || collision.gameObject.tag == "acc_stair_right_move"){
                            acc_dir = 1;
                        }else{
                            acc_dir = -1;
                        }
                        floor_type = 2;
                    }
                }
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

    public void abort(){
        Time.timeScale = 0;
        SceneManager.LoadScene(0);

        pannel.SetActive(true);
        font.SetActive(true);
        end.SetActive(true);
        regist.SetActive(true);
        level.SetActive(true);
        start_game.SetActive(true);
    }

    public void start_gmae(){
        Time.timeScale = 1;
        pannel.SetActive(false);
        font.SetActive(false);
        end.SetActive(false);
        regist.SetActive(false);
        level.SetActive(false);
        start_game.SetActive(false);
    }
}