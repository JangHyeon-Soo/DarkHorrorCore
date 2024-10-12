using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    public bool MapGeneratingOn;
    public NavMeshSurface navMeshSurface;

    public float xScale;
    public float yScale;

    public float FloorDistance;

    public GameObject Floor;
    public GameObject[] Wall;


    int[] Rotations = { 0, 90, 180, 360 };

    // Start is called before the first frame update
    void Start()
    {
        if(MapGeneratingOn)
        {
            GenerateFloor();
        }
        
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame


    public void GenerateFloor()
    {
        float currentYrot = 0 - yScale / 2 * FloorDistance;
        for (int i = 0; i < yScale; i++)
        {
            float currentXrot = 0 - xScale / 2 * FloorDistance;
            for (int j = 0; j < xScale; j++)
            {
                GameObject floor = Instantiate(Floor);
                floor.transform.position = new Vector3(currentXrot, 0, currentYrot);
                currentXrot += FloorDistance;

                int randomWall = Random.Range(0, 100);
                if (randomWall <= 8)
                {
                    Vector3 pos = floor.transform.position;

                    pos.y = 2;
                    
                    GameObject GO = Instantiate(Wall[Random.Range(0, Wall.Length)], pos , Quaternion.Euler(0,Rotations[Random.Range(0, Rotations.Length)], 0));
                    
                    GO.transform.localScale = new Vector3(1, 5, Random.Range(5, 12));

                    if(Vector3.Distance(pos, GameManager.Instance.playerTF.position) >= 120f)
                    {
                        GO.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
            currentYrot += FloorDistance;
        }


    }

    float GetRandomWithInterval(float minValue, float maxValue, int interval)
    {
        // 범위 내에서 난수 생성 후, 간격으로 나눈 값을 다시 간격으로 곱해줌
        float randomValue = Random.Range(minValue, maxValue + 1); // maxValue를 포함하기 위해 +1
        return (randomValue / interval) * interval;
    }

    bool IsObjectInRadius(Vector3 center, float radius, LayerMask layerMask)
    {
        // 해당 구체 내의 모든 콜라이더를 얻는다.
        Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask);

        // 콜라이더 배열이 비어 있지 않으면, 해당 레이어의 물체가 있다는 뜻

        if (colliders.Length > 0)
        {
            return true;
        }


        return false;
    }

}