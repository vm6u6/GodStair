using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainActor : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;              
    [SerializeField] private int cntFloorCertification = 0;   
    [SerializeField] private float jumpForce = 5f;             
    [SerializeField] private GameObject powerBar;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private int maxFloor = 0; 
    private float downTime, upTime, pressTime = 0;
    private int moveDirection = 1;            
    private bool isJumping = false;           
    private bool islanding = true; 
    private SpriteRenderer spriteRenderer;    
    private Rigidbody2D rb;                   
    private float currentPressTime = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        Camera mainCamera = Camera.main;
        MainCamera cameraFollow = mainCamera.GetComponent<MainCamera>();
        cameraFollow.target = transform;
    }

    private void Update()
    {   
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed *= 0.6f;
        }
        float movement = moveDirection * currentSpeed * Time.deltaTime;
        transform.Translate(movement, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && islanding)
        {
            downTime = Time.time;
            currentPressTime = 0;
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.Space) && islanding)
        {
            currentPressTime += Time.deltaTime;
            UpdatePowerBar(currentPressTime);
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
        CountFloor();
    }

    private void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + pressTime * 2f);
        islanding = false;
        pressTime = 0;
        jumpForce = 5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("right_border"))
        {
            moveDirection = -1;
            spriteRenderer.flipX = true;
        }
        else if (collision.gameObject.CompareTag("left_border"))
        {
            moveDirection = 1;
            spriteRenderer.flipX = false;
        }

        if ((collision.gameObject.CompareTag("floor") || 
             collision.gameObject.CompareTag("stair") || 
             collision.gameObject.CompareTag("acc_stair")) && 
            rb.velocity.y == 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);

            if (hit.collider != null)
            {
                isJumping = false;
                islanding = true;
                ResetPowerBar();
            }
        }
    }

    private void CountFloor()
    {
        int nowPos = Mathf.FloorToInt(transform.position.y);
        if (nowPos > cntFloorCertification)
        {
            maxFloor += 1;
            cntFloorCertification += 3;
            textMeshPro.text = maxFloor.ToString("D4") + "F";
        }
    }

    private void UpdatePowerBar(float pressTime_)
    {
        int L = powerBar.transform.childCount;
        int activateCount = Mathf.FloorToInt(pressTime_ / 0.25f);

        for (int i = 0; i < L; i++)
        {
            powerBar.transform.GetChild(i).gameObject.SetActive(i < activateCount);
        }
    }

    private void ResetPowerBar()
    {
        int L = powerBar.transform.childCount;
        for (int i = 0; i < L; i++)
        {
            powerBar.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}