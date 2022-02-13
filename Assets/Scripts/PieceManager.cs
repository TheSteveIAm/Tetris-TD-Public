using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheSteve.TTD {

	//NOTE: This was originally defined using Odin Inspector, it got VERY ugly due to fixing it in 10m after stripping Odin out, so I can show it publicly.
	public class PieceShape {
		public int[,] blockValue = new int[5, 5];
	}

	public class PieceManager : MonoBehaviour {
		public static float rowDropTime = 1f;

		private List<PieceShape> tetrisPieces = new List<PieceShape> ();

		private void Awake () {
			//The ugly part, it's late and this is a throw-away chunk of code in a prototype I'm no longer working on.
			PieceShape newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,1,0,0},
											   {0,0,1,0,0},
											   {0,0,1,1,0},
											   {0,0,0,0,0} };

			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,1,0,0},
											   {0,0,1,0,0},
											   {0,1,1,0,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,0,0,0},
											   {0,0,1,1,0},
											   {0,0,1,1,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,0,0,0},
											   {0,1,1,0,0},
											   {0,0,1,1,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,0,0,0},
											   {0,0,1,1,0},
											   {0,1,1,0,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,1,0,0},
											   {0,0,1,0,0},
											   {0,0,1,0,0},
											   {0,0,1,0,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);

			newShape = new PieceShape ();
			newShape.blockValue = new int[,] { {0,0,0,0,0},
											   {0,0,1,0,0},
											   {0,1,1,1,0},
											   {0,0,0,0,0},
											   {0,0,0,0,0} };
			tetrisPieces.Add (newShape);
		}

		public PieceShape GetTetromino () {
			return tetrisPieces[UnityEngine.Random.Range (0, tetrisPieces.Count)];
		}

		//public Block
	}
}