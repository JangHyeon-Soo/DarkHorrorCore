using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGlowBlinker : MonoBehaviour
{
    public Color glowColor = Color.yellow;  // 빛나는 노란색
    public float blinkSpeed = 2f;           // 반짝임 속도

    private Renderer meshRenderer;
    private Material material;
    private Color originalEmissionColor;    // 원래 Emission 색상 저장

    void Start()
    {
        // Renderer 컴포넌트 가져오기
        meshRenderer = GetComponent<Renderer>();

        // Renderer에서 사용하는 머티리얼을 가져옴
        if (meshRenderer != null)
        {
            material = meshRenderer.material;

            // Emission을 사용하려면 이 설정이 필요합니다.
            material.EnableKeyword("_EMISSION");

            // 머티리얼의 원래 Emission 색상을 저장
            originalEmissionColor = material.GetColor("_EmissionColor");
        }
    }

    void Update()
    {
        if (material != null)
        {
            // 시간에 따라 Emission 색상을 변경
            float lerp = Mathf.PingPong(Time.time * blinkSpeed, 1);

            // Emission 색상을 원래 색상과 노란색 사이에서 전환
            Color emissionColor = Color.Lerp(originalEmissionColor, glowColor, lerp);

            // Emission 색상을 설정
            material.SetColor("_EmissionColor", emissionColor * Mathf.LinearToGammaSpace(1.0f));
        }
    }
}

