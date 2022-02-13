using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{
    public class Bat : Enemy
    {
        private float timeout = 30f, timeoutTimer = 0f;

        // Update is called once per frame
        void Update()
        {
            if (targetBlock == null)
            {
                if (isGoingRight)
                {
                    if (gridPosition.x != GameGrid.Columns - 1)
                    {
                        for (int column = gridPosition.x; column < GameGrid.Columns; column++)
                        {
                            Block block = GameGrid.GetCellAtCoordinate(column, gridPosition.y).occupyingBlock;
                            if (block != null)
                            {
                                targetBlock = block;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (gridPosition.x != 0)
                    {
                        for (int column = gridPosition.x; column >= 0; column--)
                        {
                            Block block = GameGrid.GetCellAtCoordinate(column, gridPosition.y).occupyingBlock;
                            if (block != null)
                            {
                                targetBlock = block;
                                break;
                            }
                        }
                    }
                }

                if (targetBlock == null)
                {
                    transform.Translate((isGoingRight ? Vector2.right : Vector2.left) * moveSpeed * Time.deltaTime);

                    timeoutTimer += Time.deltaTime;
                    if (timeoutTimer > timeout)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                timeoutTimer = 0f;

                if (Vector2.Distance(transform.position, targetBlock.transform.position) <= attackRange)
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer >= attackCooldown)
                    {
                        targetBlock.Damage(damage);
                        attackTimer = 0f;
                    }
                }
                else
                {
                    transform.Translate((targetBlock.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime);
                    attackTimer = 0f;
                }
            }
        }
    }
}