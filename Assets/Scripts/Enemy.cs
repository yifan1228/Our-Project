using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    [SerializeField] float jumpForce = 300f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth; // 初始化生命值
        tag = "Enemy"; // 设置标签为 Enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 内部逻辑
    private void Die()
    {
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // 停止角色移动
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false; // 隐藏角色
        Destroy(gameObject);
    }

    // 实现接口IPawn
    public void Move(float direction)
    {
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y);
        rb.velocity = movement; // 设置刚体的速度
    }

    // 实现接口IEntity
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health == 0)
        {
            // 处理角色死亡逻辑
            Die();
        }
    }
}
