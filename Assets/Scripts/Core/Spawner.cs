using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class Spawner : MonoBehaviour
{
    private GameObject[] tetrominos;
    public List<int> bag = new List<int>();

    public Tetromino currentTetromino; // 현재 활성 Tetromino

    GameReadyPanel gameReadyPanel;

    public Image nextTetrominoImage;   // Next 테트로미노 표시용

    public Sprite[] tetrominoSprites;   // 테트로미노 스프라이트들 미리 찾기

    GameManager gameManager;

    private void Awake()
    {
        // Resources/Tetrominos 폴더 안 프리팹 로드
        tetrominos = Resources.LoadAll<GameObject>("Tetrominos");

        Addressables.LoadAssetsAsync<Sprite>("TetrominoSprite", null)
            .Completed += OnSpritesLoaded;
    }

    private void Start()
    {
        //SpawnNext(); => GameReadyPanel의 StartButton을 받아서 실행으로 변경
        //gameReadyPanel = FindAnyObjectByType<GameReadyPanel>(); => 게임매니저와 연결로 변경
        //gameReadyPanel.onGameReadyPanelGameStart += OnGameStart;

        gameManager = GameManager.Instance;
        gameManager.onGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        //gameReadyPanel.onGameReadyPanelGameStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        // 게임 시작
        Debug.Log("게임 시작!");
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

        DefControl(tetromino);       // 테트로미노의 속도 조절

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

        UpdateNextTetrominoImage(); // NEXT UI 갱신
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

    void UpdateNextTetrominoImage()
    {
        if (bag.Count == 0)
            FillBag();

        int nextIndex = bag[0];

        /*SpriteRenderer sr = tetrominos[nextIndex].GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            nextTetrominoImage.sprite = sr.sprite;
            // 스프라이트들 준비 됬으니까, Addressables? 로 해보자
        }*/
        nextTetrominoImage.sprite = tetrominoSprites[nextIndex];
        //Debug.Log(nextIndex);
    }

    void OnSpritesLoaded(AsyncOperationHandle<IList<Sprite>> handle)
    {
        /*tetrominoSprites = new Sprite[handle.Result.Count];

        for (int i = 0; i < handle.Result.Count; i++)
        {
            tetrominoSprites[i] = handle.Result[i];
        }

        Debug.Log("Loaded sprite count : " + tetrominoSprites.Length);*/

        tetrominoSprites = handle.Result
        .OrderBy(sprite => sprite.name)
        .ToArray();

        //Debug.Log("Loaded sprite count : " + tetrominoSprites.Length);
    }

    /// <summary>
    /// 레벨에 따라 난이도 조절 함수
    /// </summary>
    private void DefControl(Tetromino tetromino)
    {
        float interval = 1;
        switch (gameManager.Level)
        {
            case 1:
                interval = 1;
                break;
            case 2:
                interval = 0.75f;
                break;
            case 3:
                interval = 0.5f;
                break;
            case 4:
                interval = 0.25f;
                break;
            case 5:
                interval = 0.125f;
                break;
            default:
                interval = 0.125f;
                break;
        }
        tetromino.fallInterval = interval;
    }
}