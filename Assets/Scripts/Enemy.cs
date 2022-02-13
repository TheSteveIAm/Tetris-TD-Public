using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{

    public class Enemy : MonoBehaviour
    {
        public delegate void EnemyEventDelegate(Enemy enemy);
        public static event EnemyEventDelegate OnEnemyKilled;

        protected Block targetBlock;
        public bool isGoingRight;
        public Vector2Int gridPosition;
        public float moveSpeed = 1f;
        public float attackRange = 0.5f;
        public float attackCooldown = 2f;
        protected float attackTimer = 0f;
        public int damage = 1;
        public int maxHealth;
        private int currentHealth;

        public void Init()
        {
            float positionX = isGoingRight ? GameGrid.gridObjects[0, 0].transform.position.x - 10f : GameGrid.gridObjects[GameGrid.Columns - 1, GameGrid.Rows - 1].transform.position.x + 10f;
            transform.position = new Vector2(positionX, GameGrid.gridObjects[0, gridPosition.y].transform.position.y);

            currentHealth = maxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Damage(int damageAmount)
        {
            currentHealth -= damageAmount;

            if(currentHealth <= 0)
            {
                if(OnEnemyKilled != null)
                {
                    OnEnemyKilled.Invoke(this);
                }

                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Cell cell = collision.GetComponent<Cell>();

            if (cell != null)
            {
                gridPosition = cell.coordinates;
            }
        }
    }

}