using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD {

	public class Block : MonoBehaviour {

		public delegate void BlockEventDelegate (Block block);

		public static event BlockEventDelegate OnBlockDestroyed;

		//public Sprite blockSprite;
		public int relativeColumn, relativeRow;

		public int absoluteColumn, absoluteRow;
		public Vector2 orientation;
		public int maxHealth = 1;
		private int currentHealth;

		protected bool isActive = false;

		// Start is called before the first frame update
		public void Init () {
			currentHealth = maxHealth;
		}

		public void Damage (int amount) {
			currentHealth -= amount;

			if (currentHealth <= 0) {
				DestroyBlock ();
			}
		}

		public void DestroyBlock () {
			if (OnBlockDestroyed != null) {
				OnBlockDestroyed.Invoke (this);
			}

			Destroy (gameObject);
		}

		public void MoveBlock (int gridColumn, int gridRow) {
			absoluteRow = gridRow;
			absoluteColumn = gridColumn;

			transform.position = GameGrid.gridObjects[gridColumn, gridRow].transform.position;
		}

		public void RotateBlock (bool clockwise) {
			transform.Rotate (new Vector3 (0, 0, (clockwise ? -90 : 90)));
		}

		public Vector2Int GetBlockGridRotation (Vector2Int originPos, Vector2Int gridSize, bool clockwise) {
			Vector2Int relativePos = new Vector2Int (originPos.x - relativeColumn + Mathf.FloorToInt (gridSize.x / 2), originPos.y - relativeRow + Mathf.FloorToInt (gridSize.y / 2)) - originPos;
			Vector2Int[] rotMatrix = clockwise ? new Vector2Int[2] { new Vector2Int (0, -1), new Vector2Int (1, 0) }
											   : new Vector2Int[2] { new Vector2Int (0, 1), new Vector2Int (-1, 0) };
			int newColumnPos = (rotMatrix[0].x * relativePos.x) + (rotMatrix[1].x * relativePos.y);
			int newRowPos = (rotMatrix[0].y * relativePos.x) + (rotMatrix[1].y * relativePos.y);
			Vector2Int newPos = new Vector2Int (newColumnPos, newRowPos);

			newPos += originPos;
			return newPos;
		}

		public void SetActive (bool active) {
			isActive = active;
		}

		private void OnDrawGizmos () {
			Gizmos.DrawWireCube (transform.position + (transform.up * 0.1f), Vector3.one * 0.1f);
		}
	}
}