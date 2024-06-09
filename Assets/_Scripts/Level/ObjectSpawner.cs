using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private CoinObject _coinPrefab;
    [SerializeField] private BombObject _bombPrefab;
    [SerializeField] private MedkitObject _medkitPrefab;

    private Grid _grid;
    private Tilemap _tilemap;

    private int _coinCount = 0;
    private int _maxCoins = 10;

    private Vector2 _spawnPoint;

    private void Start()
    {
        _grid = GetComponent<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();

        CheckGrid();
    }

    private void CheckGrid()
    {
        for (int x = -30; x < 30; x++)
        {
            for (int y = -15; y < 1; y++)
            {
                Vector2 temp = new Vector2(x, y);
                Vector3 position = _grid.GetCellCenterWorld(new Vector3Int(x, y));

                if (CheckTilePosition(temp) && _coinCount < _maxCoins)
                    SpawnCoin(position);
            }
        }
    }

    private void SpawnCoin(Vector3 position)
    {
        int chance = Random.Range(0, 100);

        if (chance <= 2)
        {
            CoinObject coinInst = Instantiate(_coinPrefab, position, Quaternion.identity);
            _coinCount++;
        }
    }

    private bool CheckTilePosition(Vector2 position)
    {
        Vector3Int gridPosition = _tilemap.WorldToCell(position);

        if (!_tilemap.HasTile(gridPosition))
            return true;

        return false;
    }
}
