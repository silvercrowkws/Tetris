using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameObject[] tetrominos;
    private Queue<GameObject> bag = new Queue<GameObject>();

    private void Awake()
    {
        tetrominos = Resources.LoadAll<GameObject>("Tetrominos");
    }

    void Start()
    {
        SpawnNext();
    }

    public void SpawnNext()
    {
        int index = Random.Range(0, tetrominos.Length);
        Instantiate(tetrominos[index], transform.position, Quaternion.identity);
    }
}
