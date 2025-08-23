using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    Animator anim;
    int health;
    bool isGrounded;
    bool isTowardsLeft;
    GameObject triggeringObject;
    public GameObject wall;
    List<GameObject> abilityInstances;
    [SerializeField] float jumpForce = 300f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值
    [SerializeField] GameObject[] abilities; // 技能对象

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        health = maxHealth;
        tag = "Player";
        // 实例化技能
        if (abilities != null)
        {
            abilityInstances = new List<GameObject>();
            for (int i = 0; i < abilities.Length; i++)
            {
                abilityInstances.Add(Instantiate(abilities[i], transform));
            }
        }
        isTowardsLeft = false;
    }

    //碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            // 处理与墙体的碰撞逻辑
            var contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (var contact in contacts)
            {
                if (Mathf.Abs(contact.normal.y) < 0.1f)
                {
                    wall = collision.gameObject;
                    break;
                }

            }
        }
        
        // 处理与其他物体的碰撞逻辑
        var contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        foreach (var contact in contactPoints)
        {
            if (contact.normal.x < 0.2f)
            {
                isGrounded = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == wall)
        {
            wall = null;
        }
    }

    // 触发
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(tag))
        {
            triggeringObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggeringObject == collision.gameObject)
        {
            triggeringObject = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;
    }


    //内部逻辑
    private void Die()
    {
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // 停止角色移动
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false; // 隐藏角色
        Destroy(gameObject);
    }
    private void Clamber()
    {
        rb.velocity = new Vector2(0f, moveSpeed * 0.45f);
    }

    //实现接口IPawn
    public void Jump()
    {
        if (rb != null && IsOnTheWall())
        {
            Clamber();

            return;// 贴墙时不进行正常跳跃
        }
        //跳跃（不贴墙）
        if (rb != null && isGrounded) // 检查是否在地面上
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isGrounded = false;
        }
    }
    public void Move(float direction)
    {
        Vector2 moveDirection = new Vector2(direction, 0);
        // 贴墙
        if (rb != null && IsOnTheWall())
        {
            rb.AddForce(moveDirection * 10f);

            return;// 贴墙时不进行正常移动
        }
        // 移动（不贴墙）
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        if (direction < 0)
        {
            isTowardsLeft = true;
        }
        else if (direction > 0)
        {
            isTowardsLeft = false;
        }
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public void UseAbility(int abilityIndex)
    {
        if (abilityInstances != null && abilityIndex >= 0 && abilityIndex < abilityInstances.Count)
        {
            abilityInstances[abilityIndex].GetComponent<IAbility>()?.Activate(GetComponent<IEntity>());
        }
    }
    public bool IsOnTheWall()
    {
        return wall != null;
    }

    // 实现接口IEntity
    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public string GetName()
    {
        return tag;
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health == 0)
        {
            // 处理角色死亡流程
            Die();
        }
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public bool IsTowardsLeft()
    {
        return isTowardsLeft;
    }
    public GameObject TriggeringObject()
    {
        return triggeringObject;
    }
    public void AddAbility(GameObject ability)
    {
        abilityInstances ??= new List<GameObject>();
        abilityInstances.Add(Instantiate(ability, transform));
    }
    public bool IsCreature()
    {
        return true;
    }
}
