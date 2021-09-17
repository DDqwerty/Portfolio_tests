using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    protected Vector3 moveDelta;
    protected float ySpeed = 0.75f;
    protected float xSpeed = 1.0f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        moveDelta = new Vector3(input.x *xSpeed,input.y * ySpeed, 0);

        // Смена направления героя
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Отбрасывание
        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // Проверяем можно ли двигатся в заданном направлении
        hit = Physics2D.BoxCast(((Vector2)transform.position + boxCollider.offset), boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Движение
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(((Vector2)transform.position + boxCollider.offset), boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            // Движение
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        if (input.magnitude <= 0.1f)
            OnRunAnim(false);
        else
        {
            OnRunAnim(true);
        }
    }

    protected virtual void OnRunAnim(bool run)
    {
            GetComponent<Animator>().SetBool("Run", run);
    }
}
