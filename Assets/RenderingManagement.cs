using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RenderingManagement : MonoBehaviour
{
    // Update is called once per frame

    private void Start()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= 120f) 
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }


}
