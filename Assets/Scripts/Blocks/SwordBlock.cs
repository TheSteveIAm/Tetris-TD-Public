using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{
    public class SwordBlock : Block
    {
        public float attackRange = 1f;
        public int damage = 1;

        public float attackCooldown = 3f;
        private float attackTimer = 3f;

        private CircleCollider2D col;

        private Enemy targetEnemy;

        //public MMFeedback swordBlockFeedbacks;
        private SpriteRenderer sprite;

        private void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            col = GetComponent<CircleCollider2D>();
            attackRange = col.radius;

            //swordBlockFeedbacks = GetComponent<MMFeedback>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                if (targetEnemy != null)
                {

                    if (attackTimer >= attackCooldown)
                    {
                        targetEnemy.Damage(damage);
                        attackTimer = 0f;
                        //swordBlockFeedbacks.Play(transform.position);
                        sprite.color = Color.red;
                    }

                    if (Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRange)
                    {
                        targetEnemy = null;
                    }
                }

                if (attackTimer < attackCooldown)
                {
                    attackTimer += Time.deltaTime;
                    sprite.color = Color.Lerp(Color.black, Color.white, attackTimer / attackCooldown);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (targetEnemy == null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                if (enemy != null)
                {
                    targetEnemy = enemy;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

}