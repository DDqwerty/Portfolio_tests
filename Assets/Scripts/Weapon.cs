using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    public int[] damage = { 1,2,4,10};
    public float[] pushForce = { 2.0f, 2.2f, 3.0f, 3.0f };

    private float cooldown = 0f;
    private float lastSwing;
    private Animator anim;

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                return;
            }

            Damage dmg = new Damage
            {
                points = damage[weaponLevel],
                origin = transform.position,
                force = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
