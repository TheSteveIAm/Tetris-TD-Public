using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{
    public class Cell : MonoBehaviour
    {
        public Block occupyingBlock;
        public Vector2Int coordinates;

        private void OnEnable()
        {
            Block.OnBlockDestroyed += RemoveBlock;
        }

        private void OnDisable()
        {
            Block.OnBlockDestroyed -= RemoveBlock;
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if(occupyingBlock != null)
            //{
            //    GetComponent<SpriteRenderer>().color = Color.red;
            //}
        }

        void AddBlock(Block block)
        {
            occupyingBlock = block;
        }

        void RemoveBlock(Block block)
        {
            if(block == occupyingBlock)
            {
                occupyingBlock = null;
            }
        }
    }
}