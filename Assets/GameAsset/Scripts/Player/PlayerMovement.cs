
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public Transform playerTF;
    public Transform playerCamHolder;
    public CharacterController characterController;


    [SerializeField] public float speed;
    public float walkSpeed;
    public float runSpeed;

    public float mouseSensitivity;
    float xRot = 0;
    float yRot = 0;

    public KeyCode JumpKey = KeyCode.Space;
    public float jumpForce = 8f; // 점프 힘
    public float gravity = -9.81f; // 중력 강도

    private Vector3 velocity;
    private bool isGrounded;

    public bool isGround;

    public CameraShake shake;

    public bool isWalk;
    public bool isRun;

    Quaternion curRot1;
    Quaternion curRot2;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
       if(GameManager.Instance.gameState == GameManager.GameState.Play && GameManager.Instance.playerState == GameManager.StateOfPlayer.Default)
       {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
                
            }

            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRun = false;
                //speed = 0;
                //shake.bobAmplitude = 0.05f;
                //shake.bobFrequency = 1.5f;
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
            {
                if(isRun)
                {
                    isWalk = true;
                    //Debug.Log(isWalk);
                    speed = runSpeed;
                    shake.bobAmplitude = 0.05f;
                    shake.bobFrequency = 20f;
                }

                else
                {
                    isWalk = true;
                    //Debug.Log(isWalk);
                    speed = walkSpeed;
                    shake.bobAmplitude = 0.1f;
                    shake.bobFrequency = 10f;
                }
                
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S))
            {
                //Debug.Log(isWalk);
                isWalk = false;
                speed = 0;
                shake.bobAmplitude = 0.05f;
                shake.bobFrequency = 1.5f;
            }


            if (GameManager.Instance.playerState == GameManager.StateOfPlayer.Default)
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y") * mouseSensitivity;

                Vector3 movement = Input.GetAxis("Horizontal") * playerCamHolder.right + Input.GetAxis("Vertical") * playerCamHolder.forward;

                if (isGround && velocity.y < 0)
                {
                    velocity.y = -2f;  // 지면에 붙도록 속도 초기화
                }

                if (movement != Vector3.zero)
                {
                    movement *= speed * Time.deltaTime;
                }

                characterController.Move(movement);

                yRot -= y;
                yRot = Mathf.Clamp(yRot, -70, 70);


                xRot += x;
                curRot1 = Quaternion.Euler(0, xRot, 0);

                //playerTF.rotation = Quaternion.Euler(0, xRot, 0);

                curRot2 = playerCamHolder.transform.rotation;

                transform.rotation = Quaternion.Slerp(transform.rotation, curRot1, Time.deltaTime * 3.5f);
                playerCamHolder.localRotation = Quaternion.Slerp(playerCamHolder.localRotation, Quaternion.Euler(yRot, 0, 0), Time.deltaTime * 3.5f);


                isGround = GroundCheck();
                if (isGround && Input.GetKey(JumpKey))
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);  // 점프 속도 계산
                }

                // 중력 적용
                velocity.y += gravity * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
            }



       }

       else
       {
            if(GameManager.Instance.playerState == GameManager.StateOfPlayer.Die)
            {
                StartCoroutine(GameManager.Instance.GameOverAndRespawn());
            }
            
            //GameManager.Instance.
       }

    }

    bool GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerTF.position, Vector3.down + playerTF.up * 0.2f, out hit, 2f)) 
        {
            return true;
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
