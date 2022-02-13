using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//using Sirenix.OdinInspector;

namespace TheSteve.TTD {

	[System.Serializable]
	public class BlockArray {
		public Block[] blockRow = new Block[5];
	}

	public class BlockContainer {
		public Block block;
	}

	public class Piece : MonoBehaviour {

		public delegate void PieceEventDelegate (Piece piece);

		public static event PieceEventDelegate OnPieceLanded;

		//[TableMatrix(DrawElementMethod = "DrawBlocks")]
		//[TableMatrix(SquareCells = true)]
		//public int gridColumns, gridRows;

		//[ShowInInspector]
		//[SerializeField]
		private int[,] blockValue = new int[5, 5];

		//public bool isEven;
		private Vector2Int offset;

		//public BlockArray[] blockLayout = new BlockArray[5];
		public Block woodBlockPrefab;

		public Block selectedBlockTypePrefab;

		private List<Block> blocks = new List<Block> ();

		//public bool canRotate = true;
		private bool isLanded = false;

		private int gridPositionColumn, gridPositionRow;

		private float dropTimer = 0f;

		public enum PieceType { Wood, Shield, Sword, Arrow, Bank }

		public PieceType pieceType;

		private static bool DrawBlocks (Rect rect, bool value) {
			if (Event.current.type == EventType.MouseDown &&
				rect.Contains (Event.current.mousePosition)) {
				value = !value;
				GUI.changed = true;
				Event.current.Use ();
			}

			//editor

			return value;
		}

		private void OnEnable () {
			Block.OnBlockDestroyed += RemoveBlock;
		}

		private void OnDisable () {
			Block.OnBlockDestroyed -= RemoveBlock;
		}

		public void InitPiece (PieceShape shape, PieceType type) {
			blockValue = shape.blockValue;

			int blockCount = 0;

			foreach (int i in blockValue) {
				if (i.Equals (1)) {
					blockCount++;
					Debug.Log ("Counted " + blockCount);
				}
			}

			int selectedBlock = Random.Range (0, blockCount);
			int createdBlocks = 0;
			for (int column = 0; column < blockValue.GetLength (0); column++) {
				for (int row = 0; row < blockValue.GetLength (1); row++) {
					if (blockValue[column, row] == 1) {
						Block newBlock;
						if (createdBlocks == selectedBlock) {
							newBlock = Instantiate (selectedBlockTypePrefab, new Vector3 (-50f, -50f, 0), Quaternion.identity);
						} else {
							newBlock = Instantiate (woodBlockPrefab, new Vector3 (-50f, -50f, 0), Quaternion.identity);
						}

						blocks.Add (newBlock);
						newBlock.Init ();
						newBlock.relativeColumn = column;
						newBlock.relativeRow = row;
						newBlock.transform.parent = transform;

						createdBlocks++;
						//newBlock.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
					}
				}
			}

			PositionBlocks ();
		}

		private void Start () {
		}

		private bool PositionBlocks () {
			gridPositionRow = GameGrid.Rows - 1;
			gridPositionColumn = Mathf.RoundToInt (GameGrid.Columns / 2);
			transform.position = GameGrid.gridObjects[gridPositionColumn, gridPositionColumn].transform.position;

			foreach (var block in blocks) {
				int blockColumn = gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2);
				int blockRow = gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2);
				if (blockRow < GameGrid.Rows && blockColumn < GameGrid.Columns) {
					//MovePieceAndBlocks(gridPositionColumn, gridPositionRow);
					block.MoveBlock (blockColumn, blockRow);

					//if (!GameGrid.IsValidGridPosition(blockColumn, blockRow))
					//{
					//    block.transform.position = new Vector3(100f, 100f, 0);
					//}
				}
			}
			return true;
		}

		private void Update () {
			if (!isLanded) {
				dropTimer += Time.deltaTime;
				if (dropTimer >= PieceManager.rowDropTime) {
					Move (Vector2Int.down);
					dropTimer = 0f;
				}
			}
		}

		private bool ArePotentialBlockPositionsValid (int column, int row) {
			foreach (var block in blocks) {
				if (block != null && gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2) < GameGrid.Rows && gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2) < GameGrid.Columns) {
					if (block != null && !GameGrid.IsValidGridPosition (column - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2), row - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2))) {
						return false;
					}
				}
			}

			return true;
		}

		private void MovePieceAndBlocks (int column, int row) {
			gridPositionRow = row;
			gridPositionColumn = column;

			foreach (var block in blocks) {
				if (block != null && GameGrid.IsValidGridPosition (gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2), gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2))) {
					block.MoveBlock (gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2), gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2));
				}
			}
		}

		public void Move (Vector2Int delta) {
			int potentialRow = gridPositionRow + Mathf.RoundToInt (delta.y);
			int potentialColumn = gridPositionColumn + Mathf.RoundToInt (delta.x);

			if (ArePotentialBlockPositionsValid (potentialColumn, potentialRow)) {
				MovePieceAndBlocks (potentialColumn, potentialRow);
			} else if (delta == Vector2Int.down) {
				LandPiece ();
			}
		}

		public void Rotate () {
			bool canRotate = true;

			foreach (var block in blocks) {
				var rotatedBlockPos = block.GetBlockGridRotation (new Vector2Int (gridPositionColumn, gridPositionRow), new Vector2Int (blockValue.GetLength (0), blockValue.GetLength (1)), true);
				if (!GameGrid.IsValidGridPosition (rotatedBlockPos.x, rotatedBlockPos.y)) {
					canRotate = false;
					break;
				}
			}

			if (canRotate) {
				foreach (var block in blocks) {
					if (block != null) {
						var rotatedBlockPos = block.GetBlockGridRotation (new Vector2Int (gridPositionColumn, gridPositionRow), new Vector2Int (blockValue.GetLength (0), blockValue.GetLength (1)), true);
						Vector2Int relativePos = new Vector2Int (gridPositionColumn - rotatedBlockPos.x + Mathf.FloorToInt (blockValue.GetLength (0) / 2), gridPositionRow - rotatedBlockPos.y + Mathf.FloorToInt (blockValue.GetLength (1) / 2));

						block.relativeColumn = relativePos.x;
						block.relativeRow = relativePos.y;

						block.RotateBlock (true);

						//block.MoveBlock(gridPositionRow - block.relativeRow + 2, gridPositionColumn - block.relativeColumn + 2);
						//block.MoveBlock(rotatedBlockPos.x, rotatedBlockPos.y);
					}
				}
				MovePieceAndBlocks (gridPositionColumn, gridPositionRow);
			}
		}

		public void LandPiece () {
			List<Block> blocksToDestroy = new List<Block> ();
			foreach (var block in blocks) {
				if (GameGrid.IsValidGridPosition (gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2), gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2))) {
					GameGrid.gridObjects[gridPositionColumn - block.relativeColumn + Mathf.FloorToInt (blockValue.GetLength (0) / 2), gridPositionRow - block.relativeRow + Mathf.FloorToInt (blockValue.GetLength (1) / 2)].GetComponent<Cell> ().occupyingBlock = block;
					block.SetActive (true);
				} else {
					blocksToDestroy.Add (block);
				}
			}

			foreach (var block in blocksToDestroy) {
				block.DestroyBlock ();
			}

			if (OnPieceLanded != null) {
				OnPieceLanded (this);
			}

			isLanded = true;
		}

		public void RemoveBlock (Block block) {
			blocks.Remove (block);
		}
	}
}