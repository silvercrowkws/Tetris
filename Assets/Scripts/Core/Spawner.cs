using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject[] tetrominos;
    private List<int> bag = new List<int>();

    public Tetromino currentTetromino; // 현재 활성 Tetromino

    private void Awake()
    {
        // Resources/Tetrominos 폴더 안 프리팹 로드
        tetrominos = Resources.LoadAll<GameObject>("Tetrominos");
    }

    private void Start()
    {
        SpawnNext();
    }

    /// <summary>
    /// 테스트용 인덱스 배열을 받아서 bag을 초기화하는 함수
    /// </summary>
    /// <param name="customIndices"></param>
    public void SetCustomSequence(int[] customIndices)
    {
        bag.Clear();                        // 기존 내용 비우기
        bag.AddRange(customIndices);        // 넘겨받은 배열로 가방 채우기
    }

    public void SpawnNext()
    {
        if (bag.Count == 0)
            FillBag();

        int index = bag[0];
        bag.RemoveAt(0);

        GameObject newBlock = Instantiate(tetrominos[index]);

        Tetromino tetromino = newBlock.GetComponent<Tetromino>();

        // 🔥 중앙 X 위치
        int spawnX = Board.width / 2;

        // 🔥 블록의 가장 높은 cell.y 계산
        int maxCellY = GetMaxCellY(tetromino.cells);

        // 🔥 화면 위에서 정확히 시작하도록 보정
        int spawnY = Board.height - 1 - maxCellY;

        tetromino.tetrominoPosition = new Vector2Int(spawnX, spawnY);

        tetromino.UpdateVisualPosition();

        // 현재 블록으로 등록
        currentTetromino = tetromino;
    }

    private int GetMaxCellY(Vector2Int[] cells)
    {
        int max = cells[0].y;

        foreach (var cell in cells)
        {
            if (cell.y > max)
                max = cell.y;
        }

        return max;
    }

    void FillBag()
    {
        for (int i = 0; i < tetrominos.Length; i++)
            bag.Add(i);

        // Fisher-Yates Shuffle
        for (int i = 0; i < bag.Count; i++)
        {
            int randomIndex = Random.Range(i, bag.Count);
            int temp = bag[i];
            bag[i] = bag[randomIndex];
            bag[randomIndex] = temp;
        }
    }
}