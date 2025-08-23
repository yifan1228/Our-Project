using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DefaultBullet : MonoBehaviour, IEntity
{
    Rigidbody2D rb;
    [SerializeField] int damage = 10;
    [SerializeField] bool removeOnHit = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Åö×²
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(tag))
        {
            var target = collision.gameObject.GetComponent<IEntity>();
            float relativeVelocity = collision.relativeVelocity.magnitude;
            float energy = math.pow(relativeVelocity, 2.0f) * rb.mass * 0.5f;
            if (target != null && energy > 15 && !target.IsDead())
            {
                target.Damaged(damage); // ×Óµ¯Ôì³ÉÉËº¦
                if (removeOnHit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
