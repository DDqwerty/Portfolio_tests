using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoints = 10;
    public int maxhitPoints = 10;
    public float pushRecoverySpeed = 0.1f;

    protected float immmuneTime = 0.3f;
    protected float lastimmune;

    protected Vector3 pushDirection;

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if(Time.time-lastimmune>immmuneTime)
        {
            lastimmune = Time.time;
            hitPoints -= dmg.points;

            //Vector3.magnitude - модуль вектора m = sqrt(x ^ 2 + y ^ 2 + Z ^ 2);
            //Vector3.Dot(Vector1, Vector2) скалярное произведение 2х векторов
            //Vector3.Normalized - Вектор, сонаправленный с исходным, но имеющий модуль = 1
            pushDirection = (transform.position - dmg.origin).normalized * dmg.force;

            GameManager.instance.ShowText(dmg.points.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitPoints <= 0)
            {
                hitPoints = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}
