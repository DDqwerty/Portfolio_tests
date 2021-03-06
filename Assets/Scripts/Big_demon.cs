using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_demon : Enemy 
{
    public float fireballSpeed =  2.5f ;
    public float distance = 0.25f;
    public Transform[] fireball;

    private void Update()
    {
        for (int i = 0; i<fireball.Length; i++)
        {
            fireball[i].position = transform.position + new Vector3(-Mathf.Cos((Time.time-i) * fireballSpeed) * distance, Mathf.Sin((Time.time - i) * fireballSpeed) * distance, 0);
        }
    }
}
 