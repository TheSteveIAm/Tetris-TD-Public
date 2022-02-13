using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSteve.TTD
{

    public class GameGrid : MonoBehaviour
    {

        public GameObject emptyCell;
        [Range(0, 100)]
        public int gridRows, gridColumns;
        public static int Rows, Columns;
        [Range(0, 10)]
        public float gridScale = 1f, cellSize = 1f;
        private Transform myTransform;
        //private bool gridValuesDirty;

        [Range(0, 1)]
        public float blueValue = 0.7f;

        public static GameObject[,] gridObjects;

        private void Awake()
        {
            myTransform = transform;
            InitGrid();
        }

        void InitGrid()
        {
            Rows = gridRows;
            Columns = gridColumns;
            if (gridObjects != null && gridObjects.Length > 0)
            {
                foreach (GameObject o in gridObjects)
                {
                    Destroy(o);
                }
            }

            GameObject[,] tempGrid = new GameObject[gridColumns, gridRows];
            gridObjects = tempGrid;

            for (int row = 0; row < gridRows; row++)
            {
                for (int column = 0; column < gridColumns; column++)
                {
                    gridObjects[column, row] = Instantiate(emptyCell, CalculateCellPosition(row, column), Quaternion.identity, myTransform);
                    var cell = gridObjects[column, row].AddComponent<Cell>();
                    cell.coordinates = new Vector2Int(column, row);
                }
            }
        }

        public static Cell GetCellAtCoordinate(int column, int row)
        {
            if (column >= Columns || column < 0 || row >= Rows || row < 0)
            {
                return null;
            }
            else
            {
                return gridObjects[column, row].GetComponent<Cell>();
            }
        }

        public static bool IsValidGridPosition(int potentialGridColumn, int potentialGridRow)
        {
            if (potentialGridRow >= Rows ||
                potentialGridColumn >= Columns ||
                potentialGridRow < 0 ||
                potentialGridColumn < 0)
            {
                return false;
            }

            if (GetCellAtCoordinate(potentialGridColumn, potentialGridRow).occupyingBlock != null)
            {
                return false;
            }

            return true;

        }

        public static void AddBlock(Block block, Vector2Int gridPosition)
        {
            gridObjects[gridPosition.x, gridPosition.y].GetComponent<Cell>().occupyingBlock = block;
        }

        // Update is called once per frame
        void Update()
        {
            if (gridObjects != null && gridObjects.Length != gridRows * gridColumns)
            {
                InitGrid();
            }

            //TODO: dirty flag

            //if (gridValuesDirty)
            //{
            for (int row = 0; row < gridRows; row++)
            {
                for (int column = 0; column < gridColumns; column++)
                {
                    gridObjects[column, row].transform.position = CalculateCellPosition(row, column);
                    gridObjects[column, row].transform.localScale = Vector3.one * gridScale * cellSize;
                    gridObjects[column, row].GetComponent<SpriteRenderer>().color = new Color((float)row / gridRows, (float)column / gridColumns, blueValue);
                }
            }
            //}
        }

        Vector2 CalculateCellPosition(int row, int column)
        {
            return new Vector2(myTransform.transform.position.x + (column + 0.5f - gridColumns / 2f) * gridScale, myTransform.transform.position.y + (row + 0.5f - gridRows / 2f) * gridScale);
        }


        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                for (int row = 0; row < gridRows; row++)
                {
                    for (int column = 0; column < gridColumns; column++)
                    {
                        var cell = gridObjects[column, row];
                        UnityEditor.Handles.Label(cell.transform.position, string.Format("{0},{1}", column, row));
                    }
                }
            }
        }
    }
}