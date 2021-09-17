using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Player : Mover
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public int currentcharacter;
    private bool isAlive = true;
    public Weapon weapon;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SwapSprite(currentcharacter);
    }

    private void FixedUpdate()
    {
       

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
        animator.SetInteger("Skin", skinId);
        animator.SetTrigger("Update");
        currentcharacter = skinId;
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        animator.SetTrigger("Hit");
        GameManager.instance.OnHPChange();
    }

    public void OnLevelUp()
    {
        maxhitPoints+=2;
        hitPoints = maxhitPoints;
        GameManager.instance.OnHPChange();
    }

    public void SetLevel(int level)
    {
        for (int i = 1; i < level; i++)
            OnLevelUp();
    }

    public void Heal(int healingAmount)
    {
        if (hitPoints == maxhitPoints)
            return;

        hitPoints += healingAmount;
        if (hitPoints > maxhitPoints)
            hitPoints = maxhitPoints;
        GameManager.instance.ShowText("+" + healingAmount.ToString(), 30, Color.red, transform.position, Vector3.up, 1.0f);
        GameManager.instance.OnHPChange();
    }

    protected override void Death()
    {
        isAlive = false; 
        GameManager.instance.deathmenu.SetTrigger("Show");
    }

    public void Respawn()
    {
        isAlive = true;
        maxhitPoints = 10;
        Heal(maxhitPoints);
        lastimmune = Time.time;
        pushDirection = Vector3.zero;
        weapon.SetWeaponLevel(0);
        GameManager.instance.exp = 0;
        GameManager.instance.coins = 0;

    }
}
