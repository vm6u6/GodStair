using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class main_ai : Agent
{
    [SerializeField] private GameObject power_bar;
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
    public int pre_max_floor = 0;
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
    private bool steady = true;
    private float steadyTimer = 0.0f;
    private float steadyThreshold = 0.5f;
  
    private float currentSpeed = 0.0f;
    private Rigidbody2D myBody;

    List<Vector2> positions_stair; 
    List<Vector2> positions_jump_stair;
    List<Vector2> positions_acc_left_stair;
    List<Vector2> positions_acc_rigth_stair;
    List<Vector2> positions_jump_stair_move;
    List<Vector2> positions_acc_left_stair_move; 
    List<Vector2> positions_acc_right_stair_move;

    void config_set(){
        moveSpeed = 1.5f;              
        max_floor = 0; 
        pre_max_floor = 0;
        cnt_floor_certification = 0.0f;   
        activateCount = 0;
        jumpForce = 5f;              
        moveDirection = 1;            
        isJumping = false;           
        islanding = true; 
        currentPressTime = 0;
        timeOnGround_start = 0f;
        timeOnGround_end = 0f;
        overJumpingTime = 0.1f;
        endGame_Line = - 7.7177f;
        // private float fallMutiplier = 2.0f;
        // private float lowJumpMutiplier = 2.5f;   
        steady = true;
        steadyTimer = 0.0f;
        currentSpeed = 0.0f;
    }

    List<Vector2> get_floor_pos(string targetTag)
    {
        List<Vector2> positions = new List<Vector2>();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            Vector2 objPosition = new Vector2(obj.transform.position.x, obj.transform.position.y);
            positions.Add(objPosition);
        }

        return positions;
    }

    void Update(){  
        if (pause_init == false) {
            return;
        }
        endGame_Line = Camera.main.transform.position.y - 5.0277f;

        if (!steady)
        {
            steadyTimer += Time.deltaTime;
            if (steadyTimer >= steadyThreshold){
                steady = true;
            }
        }
        // { Get floor position }______________________________________________________________
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        positions_stair = get_floor_pos("stair");
        if (levelManager.level_option_cnt == 1 || levelManager.level_option_cnt == 2){
            positions_jump_stair = get_floor_pos( "jump_stair" );
            positions_acc_left_stair = get_floor_pos( "acc_left_stair" );
            positions_acc_rigth_stair = get_floor_pos( "acc_rigth_stair" );
        }
        if (levelManager.level_option_cnt == 2){
            positions_jump_stair_move = get_floor_pos( "jump_stair_move" );
            positions_acc_left_stair_move = get_floor_pos( "acc_left_stair_move" );
            positions_acc_right_stair_move = get_floor_pos( "acc_right_stair_move" );
        }
    

        // { Movement }________________________________________________________________________
        currentSpeed = moveSpeed;
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);
 
        // { Jumping }_________________________________________________________________________
        cnt_floor();
        EndGmae();
        }

    private void FixedUpdate()
    {
        float yVelocity = rb.velocity.y;
        rb.velocity = new Vector2(rb.velocity.x, yVelocity);
    }

    public override void Initialize(){
        Time.timeScale = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        myBody = GetComponent<Rigidbody2D>();
        
    }



    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(gameObject.transform.position.x);
        sensor.AddObservation(gameObject.transform.position.y);
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        foreach (Vector2 pos in positions_stair){
            sensor.AddObservation(pos.x);
            sensor.AddObservation(pos.y);
        }
        if (levelManager.level_option_cnt == 1 || levelManager.level_option_cnt == 2){
            foreach (Vector2 pos in positions_jump_stair){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
            foreach (Vector2 pos in positions_acc_left_stair){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
            foreach (Vector2 pos in positions_acc_rigth_stair){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
        }
        if (levelManager.level_option_cnt == 2){
            foreach (Vector2 pos in positions_jump_stair_move){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
            foreach (Vector2 pos in positions_acc_left_stair_move){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
            foreach (Vector2 pos in positions_acc_right_stair_move){
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
        }
        sensor.AddObservation(jumpForce);              
        sensor.AddObservation(moveDirection);            
        sensor.AddObservation(currentPressTime);
    }

    public override void OnEpisodeBegin(){
        config_set();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(Time.fixedDeltaTime);


        if (rb.velocity.y == 0f){
            if (actionBuffers.ContinuousActions[0] == 1){
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

        if ( steady ){
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
            if (hitDown.collider != null){
                timeOnGround_end = Time.time;
                if (timeOnGround_end - timeOnGround_start > overJumpingTime){
                    if (actionBuffers.ContinuousActions[0] == 1 && islanding ){
                        currentPressTime += Time.deltaTime;
                    }
                    
                    if (actionBuffers.ContinuousActions[1] == 1 && !isJumping && islanding){
                        isJumping = true;
                    }

                    if (actionBuffers.ContinuousActions[2] == 1 && islanding ){
                        isJumping = false;
                        StartJump();
                    }
                }
            }
        }
    }
    

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
        continuousActionsOut[1] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        continuousActionsOut[2] = Input.GetKeyUp(KeyCode.Space) ? 1 : 0;
    }


    private void EndGmae(){
        // Debug.Log(transform.position.y);
        if (transform.position.y < endGame_Line){
            Time.timeScale = 0;
            SetReward(-1f);
            EndEpisode();
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
            steady = false;
            steadyTimer = 0.0f;
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
            }
        }
    }

    private void cnt_floor(){
        int now_pos = Mathf.FloorToInt(transform.position.y);
        if (now_pos > cnt_floor_certification)
        {
            max_floor += 1;
            cnt_floor_certification += 7.7177f/2;
            moveSpeed += 0.2f;
            SetReward(1f);
        }
    }
}