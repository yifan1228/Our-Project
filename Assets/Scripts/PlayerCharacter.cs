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
    int faceDir = 1; //面朝方向：1为右；-1为左
    [SerializeField] float jumpForce = 300f; // 跳跃力度
    [SerializeField] float moveSpeed = 5f; // 移动速度
    [SerializeField] int maxHealth = 100; // 最大生命值
    [SerializeField] GameObject[] abilities; // 技能对象
    GameObject[] abilityInstances;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        health = maxHealth; // 初始化生命值
        tag = "Player"; // 设置标签为 Player
        // 实例化技能
        if (abilities != null)
        {
            abilityInstances = new GameObject[abilities.Length];
            for (int i = 0; i < abilities.Length; i++)
            {
                abilityInstances[i] = Instantiate(abilities[i], transform);
            }
        }
        isTowardsLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;

        AnimationController();
        FlipController();
    }

    //动画控制器
    private void AnimationController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }

    //翻转控制
    private void FlipController()
    {
        if (rb.velocity.x > 0 && faceDir == -1)
        {
            faceDir = faceDir * -1;
            transform.localScale = new Vector3(1,1,1);
        }
        else if (rb.velocity.x < 0 && faceDir == 1)
        {
            faceDir = faceDir * -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
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


    //实现接口IPawn
    public void Jump()
    {
        //跳跃
        if (rb != null && isGrounded) // 检查是否在地面上
        {
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }
    public void Move(float direction)
    {
        //移动
        Vector2 moveDirection = new Vector2(direction, 0);
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
        if (abilityInstances != null && abilityIndex >= 0 && abilityIndex < abilityInstances.Length)
        {
            abilityInstances[abilityIndex].GetComponent<IAbility>()?.Activate(GetComponent<IEntity>());
        }
    }

    //实现接口IEntity
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
            // 处理角色死亡逻辑
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
}
