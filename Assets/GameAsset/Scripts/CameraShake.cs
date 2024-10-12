using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraShake : MonoBehaviour
{
    public float bobFrequency = 1.5f;  // 흔들림 주기
    public float bobAmplitude = 0.05f; // 흔들림 크기
    public Transform playerTransform;  // 플레이어의 트랜스폼

    private float bobTimer = 0.0f;     // 내부 타이머
    private Vector3 originalCameraPosition;

    public AudioClip footstepSound;    // 발자국 소리
    public AudioSource audioSource;    // 소리 재생을 위한 AudioSource
    private bool footstepPlayed = false; // 발자국 소리가 이미 재생되었는지 체크하는 플래그

    public Transform lookAtTransform;

    void Start()
    {
        // 카메라의 초기 위치 저장
        originalCameraPosition = transform.localPosition;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // 플레이어가 움직이고 있을 때만 흔들림 적용
        if(GameManager.Instance.playerState == GameManager.StateOfPlayer.Default 
            && GameManager.Instance.playerTF.GetComponent<PlayerMovement>().isGround)
        {
            if (GameManager.Instance.gameState == GameManager.GameState.Play)
            {
                // 타이머 증가
                bobTimer += Time.deltaTime * bobFrequency;

                // 사인파를 이용해 위아래 흔들림 구현
                float newY = Mathf.Sin(bobTimer) * bobAmplitude;

                // 카메라 위치를 위아래로 이동
                transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y + newY, originalCameraPosition.z);

                // 사인파의 가장 아래쪽 값에 도달했을 때 발자국 소리 재생
                if (newY < -bobAmplitude * 0.95f && !footstepPlayed && playerTransform.GetComponent<PlayerMovement>().speed != 0)
                {
                    PlayFootstepSound();
                    footstepPlayed = true; // 소리가 재생되었음을 기록
                }

                // 사인파가 다시 위로 올라가기 시작하면 발자국 소리 다시 재생 가능
                if (newY > 0)
                {
                    footstepPlayed = false; // 소리 재생 플래그 초기화
                }
            }
            else
            {
                
                // 플레이어가 멈췄을 때는 카메라를 원래 위치로 되돌림
                bobTimer = 0;
                transform.localPosition = originalCameraPosition;
            }
        }

        else
        {
            if (GameManager.Instance.playerState == GameManager.StateOfPlayer.Die)
            {
                //GameManager.Instance.playerTF.GetComponent<MeshRenderer>().enabled = false;
                transform.position = lookAtTransform.position;
                transform.rotation = lookAtTransform.rotation;
            }
                //transform.LookAt(lookAtTransform);
        }
        

        void PlayFootstepSound()
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }
}
