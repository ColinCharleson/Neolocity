using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    void Update()
    {
        Debug.Log(Input.mousePosition.x + ", " + Input.mousePosition.y);
        this.transform.position = Vector3.Lerp(this.transform.position, 
            new Vector3((Input.mousePosition.x/5000), (Input.mousePosition.y/5000)+1, transform.position.z), Time.deltaTime);
    }
}
