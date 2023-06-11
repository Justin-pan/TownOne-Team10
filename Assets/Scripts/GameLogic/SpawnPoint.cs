using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform spawnCenter;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject playerParent;
    [SerializeField]
    private float spawnAreaWidth = 1f;
    [SerializeField]
    private float spawnAreaLength = 10f;

    private int numOfPlayers = 3;

    

    void Awake()
    {
        SpawnPlayers();
    }

    public void RespawnPlayers()
    {
        Vector3 spawnAreaCenter = spawnCenter.position;
        Vector3 spawnAreaSize = new Vector3(spawnAreaWidth, spawnAreaLength, 0f);
        Vector3 spawnPoint = Vector3.zero;

        bool validSpawnPointFound = false;
        int maxAttempts = 10;

        for (int j = 0; j < numOfPlayers; ++j)
        {
            for (int i = 0; i < maxAttempts; ++i)
            {
                spawnPoint = spawnAreaCenter + new Vector3(Random.Range(-spawnAreaLength / 2f, spawnAreaLength / 2f), Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f), 0f);
                Collider[] colliders = Physics.OverlapSphere(spawnPoint, 1f);

                if (colliders.Length == 0)
                {
                    validSpawnPointFound = true;
                    break;
                }
            }

            if (validSpawnPointFound)
            {
                List<Player> players = GameManager.Instance.Players;

                Player p = players[j];
                p.transform.position = spawnPoint;
                Debug.Log("Spawning player " + p.PlayerID);
            }
            else
            {
                Debug.LogError("Unable to find a valid spawn point for player " + j);
            }
        }
    }


    public void SpawnPlayers()
    {
        Vector3 spawnAreaCenter = spawnCenter.position;
        Vector3 spawnAreaSize = new Vector3(spawnAreaWidth, spawnAreaLength, 0f);
        Vector3 spawnPoint = Vector3.zero;

        bool validSpawnPointFound = false;
        int maxAttempts = 10;

        for (int j = 0; j < numOfPlayers; ++j)
        {
            for (int i = 0; i < maxAttempts; ++i)
            {
                spawnPoint = spawnAreaCenter + new Vector3(Random.Range(-spawnAreaLength / 2f, spawnAreaLength / 2f), Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f), 0f);
                Collider[] colliders = Physics.OverlapSphere(spawnPoint, 1f);

                if (colliders.Length == 0)
                {
                    validSpawnPointFound = true;
                    break;
                }
            }

            if (validSpawnPointFound)
            {
                GameObject player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

                player.transform.parent = this.transform;
                player.name = "Player " + (j + 1);
                
                Player playerObj = player.GetComponent<Player>();
                playerObj.PlayerID = j;
                playerObj.HeightReferencePoint = spawnCenter;
            }
            else
            {
                Debug.LogError("Unable to find a valid spawn point for player " + j);
            }
        }


    }
}
