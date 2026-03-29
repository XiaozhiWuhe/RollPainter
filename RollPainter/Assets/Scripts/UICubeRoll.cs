using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICubeRoll : MonoBehaviour
{
    public GameObject cube;

    void Update()
    {
        transform.rotation=cube.transform.rotation; 
    }
}
