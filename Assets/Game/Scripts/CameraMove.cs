using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    void Update ()
    {
        if(Input.mousePosition.x <= 0)
        {
            transform.position -= new Vector3(25f, 0f, 0f) * Time.deltaTime;
        }else if (Input.mousePosition.x >= Screen.width-1)
        {
            transform.position += new Vector3(25f, 0f, 0f) * Time.deltaTime;
        }

        if (Input.mousePosition.y <= 0)
        {
            transform.position -= new Vector3(0f, 0f, 25f) * Time.deltaTime;
        }
        else if (Input.mousePosition.y >= Screen.height-1)
        {
            transform.position += new Vector3(0f, 0f, 25f) * Time.deltaTime;

        }
    }
}
