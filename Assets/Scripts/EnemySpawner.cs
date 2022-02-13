using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD {

	public class EnemySpawner : MonoBehaviour {
		public Enemy[] enemy;
		public bool spawning;

		// Start is called before the first frame update
		private void Start () {
			spawning = true;
			StartCoroutine (SpawnBat ());
		}

		// Update is called once per frame
		private void Update () {
		}

		private IEnumerator SpawnBat () {
			while (spawning) {
				bool isGoingRight = Random.Range (0, 2) == 1 ? true : false;
				Enemy spawned = Instantiate (enemy[0], new Vector2 (-20f, 0), Quaternion.identity);
				spawned.gridPosition = new Vector2Int ((isGoingRight ? 0 : GameGrid.gridObjects.GetLength (0) - 1), Random.Range (0, GameGrid.gridObjects.GetLength (1)));
				spawned.isGoingRight = isGoingRight;
				spawned.Init ();
				yield return new WaitForSeconds (3f);
			}
		}
	}
}