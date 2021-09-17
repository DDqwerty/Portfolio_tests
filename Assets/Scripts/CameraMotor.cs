using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookat;
    public float boundx = 0.15f;
    public float boundy = 0.05f;

    private void Start()
    {
        lookat = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        // Проверка на границы камеры   
        float deltaX = lookat.position.x - transform.position.x;
        if (deltaX > boundx || deltaX <-boundx)
        {
            if (transform.position.x<lookat.position.x)
            {
                delta.x = deltaX - boundx;
            }
            else
            {
                delta.x = deltaX + boundx;
            }
        }

        float deltaY = lookat.position.y - transform.position.y;
        if (deltaY > boundy || deltaY < -boundy)
        {
            if (transform.position.y < lookat.position.y)
            {
                delta.y = deltaY - boundy;
            }
            else
            {
                delta.y = deltaY + boundy;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
