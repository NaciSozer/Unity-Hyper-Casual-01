using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public GameObject playerPos;
    private Vector3 offset = new Vector3(0,5,-7);
    
    void Start()
    {
        
    }

    
    void LateUpdate()
    {
        transform.position = playerPos.transform.position + offset;
    }
}
