using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    public int damage;
    public float pushForce;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name == "Player")
        {
            Damage dmg = new Damage
            {
                points = damage,
                origin = transform.position,
                force = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
