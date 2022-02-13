using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{
    [System.Serializable]
    public class HealthBlock
    {
        //public Block block;
        public Vector2Int gridPosition;
    }

    public class Player : MonoBehaviour
    {

        public Piece.PieceType selectedPieceType;

        public Piece piecePrefab;

        public SwordBlock swordBlockPrefab;

        public Block heartBlock;
        private Piece activePiece;
        private PieceManager pieceManager;
        public HealthBlock[] healthBlocks;
        private List<Block> instantiatedHealthBlocks = new List<Block>();

        public int Gold = 5;
        //private float dropCooldown = 0.1f, dropTimer = 0f;

        private void OnEnable()
        {
            Piece.OnPieceLanded += RemoveActivePiece;
            Block.OnBlockDestroyed += CheckHealthBlocks;
        }

        private void OnDisable()
        {
            Piece.OnPieceLanded -= RemoveActivePiece;
            Block.OnBlockDestroyed -= CheckHealthBlocks;
        }

        // Start is called before the first frame update
        void Start()
        {
            pieceManager = FindObjectOfType<PieceManager>();

            foreach(HealthBlock block in healthBlocks)
            {
                Block newBlock = Instantiate(heartBlock, new Vector3(-50f, -50f, 0), Quaternion.identity);
                instantiatedHealthBlocks.Add(newBlock);
                newBlock.Init();
                newBlock.MoveBlock(block.gridPosition.x, block.gridPosition.y);
                GameGrid.AddBlock(newBlock, block.gridPosition);
                //newBlock.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (activePiece != null)
            {
                Vector2Int input = new Vector2Int();

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    //activePiece.Move(Vector2Int.left);
                    input += Vector2Int.left;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    //activePiece.Move(Vector2Int.right);
                    input += Vector2Int.right;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    activePiece.Rotate();
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    //dropTimer += Time.smoothDeltaTime;
                    //if (dropTimer >= dropCooldown)
                    //{
                    //activePiece.Move(Vector2Int.down);
                    input += Vector2Int.down;
                    //dropTimer = 0f;
                    //}
                }

                activePiece.Move(input);
                //if (Input.GetKeyUp(KeyCode.DownArrow))
                //{
                //    dropTimer = 0f;
                //}
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (activePiece != null)
                {
                    activePiece.LandPiece();
                }
                activePiece = Instantiate(piecePrefab);
                activePiece.selectedBlockTypePrefab = SelectBlockType();
                activePiece.InitPiece(pieceManager.GetTetromino(), selectedPieceType);
            }


        }

        private Block SelectBlockType()
        {
            if(selectedPieceType == Piece.PieceType.Sword)
            {
                return swordBlockPrefab;
            }

            return null;
        }

        void CheckHealthBlocks (Block block)
        {

            if (instantiatedHealthBlocks.Contains(block))
            {
                instantiatedHealthBlocks.Remove(block);
            }


            if (instantiatedHealthBlocks.Count == 0)
            {
                Debug.Log("You Lose!");
            }
        }

        void RemoveActivePiece(Piece piece)
        {
            if (piece == activePiece)
            {
                activePiece = null;
            }
        }
    }
}
