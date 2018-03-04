using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameOfLife3D : GameOfLife {

    [SerializeField]
    private GameObject cellPrefab;
    private Vector2 CELL_PIVOT_OFFSET = new Vector2(0.5f, 0.5f);
    private GameObject[,] grid;
    private int width = 100;
    private int height = 100;
    private int generation;
    private float tickInterval = 1.0f;

    private void Awake() {
        grid = new GameObject[width, height];
    }

    private void Start() {
        pauseStateChanged(gamePaused = true);
        generationChanged(generation = 0);
        elapsedTime = 0f;
    }

    private void Update() {
        if (!gamePaused) {
            elapsedTime += Time.deltaTime;
            int ticksPassed = Mathf.FloorToInt(elapsedTime / tickInterval);
            elapsedTime -= ticksPassed * tickInterval;
            for (int i = 0; i < ticksPassed; i++) {
                Tick();
            }
        } else {
            bool mouseClicked = Input.GetMouseButtonDown(0);
            if (mouseClicked) {
                Vector2 pos = gameCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int boardPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
                ChangeCell(boardPos);
            }
        }
    }

    public void Pause() {
        gamePaused = !gamePaused;
        pauseStateChanged(gamePaused);
    }

    public void SetTickInterval(float tickInterval) {
        this.tickInterval = tickInterval;
    }

    public virtual void Tick() {
        generationChanged(++generation);
        List<Vector2Int> toBringToLife = new List<Vector2Int>();
        List<Vector2Int> toBeKilled = new List<Vector2Int>();
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                bool cellIsAlive = grid[i, j] != null;
                int numNeighbours = GetNumNeighbours(i, j);
                if (cellIsAlive) {
                    if (numNeighbours < 2 || numNeighbours > 3) {
                        toBeKilled.Add(new Vector2Int(i, j));
                    }
                } else {
                    if (numNeighbours == 3) {
                        toBringToLife.Add(new Vector2Int(i, j));
                    }
                }
            }
        }
        foreach (Vector2Int cell in toBringToLife) {
            CreateCell(cell);
        }
        foreach (Vector2Int cell in toBeKilled) {
            DestroyCell(cell);
        }
    }

    public virtual void Restart() {
        pauseStateChanged(gamePaused = true);
        foreach (GameObject cell in grid) {
            if (cell != null) {
                Destroy(cell);
            }
        }
        Array.Clear(grid, 0, grid.Length);
        generationChanged(generation = 0);
    }

    private int GetNumNeighbours(int x, int y) {
        int numNeighbours = 0;

        int minXRange = x > 0 ? -1 : 0;
        int maxXRange = x < width - 1 ? 1 : 0;
        int minYRange = y > 0 ? -1 : 0;
        int maxYRange = y < height - 1 ? 1 : 0;

        for (int i = minXRange; i <= maxXRange; i++) {
            for (int j = minYRange; j <= maxYRange; j++) {
                if (i == 0 && j == 0) { // Don't count ourselves
                    continue;
                }
                bool neighbourIsAlive = grid[x + i, y + j] != null;
                numNeighbours += neighbourIsAlive ? 1 : 0;
            }
        }
        return numNeighbours;
    }

    private void ChangeCell(Vector2Int cellPos) {
        try {
            GameObject cell = grid[cellPos.x, cellPos.y];
            if (cell == null) {
                CreateCell(cellPos);
            } else {
                DestroyCell(cellPos);
            }
        }
        catch {
            return;
        }
    }

    private void CreateCell(Vector2Int cellPos) {
        GameObject newCell = Instantiate(cellPrefab);
        newCell.transform.SetParent(gameBoard);
        newCell.transform.position = cellPos + CELL_PIVOT_OFFSET;
        grid[cellPos.x, cellPos.y] = newCell;
    }

    private void DestroyCell(Vector2Int cellPos) {
        GameObject deadCell = grid[cellPos.x, cellPos.y];
        if (deadCell != null) {
            Destroy(deadCell);
        }
        grid[cellPos.x, cellPos.y] = null;
    }

}
