using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance {get; private set;}

    [SerializeField] private AmmoController pentolAmmo;
    [SerializeField] private float spawnInterval;

    [SerializeField] List<PlayerController> players;

    private Vector2 screenBounds;

    private void Awake() {
        
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {        
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width), (Screen.height), Camera.main.transform.position.z));
        StartCoroutine(SpawnPentolAlways(spawnInterval));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnPentolAlways(float interval) {
        while (true)
        {
            SpawnPentol();
            yield return new WaitForSeconds(interval);
        }
    }

    public void SpawnPentol() {
        float randomXPos = Random.Range(-screenBounds.x, screenBounds.x);
        Vector2 spawnPos = new Vector2(randomXPos, screenBounds.y);
        Instantiate(pentolAmmo.gameObject, spawnPos, Quaternion.identity);
        Debug.Log("Spawned ammo");
    }

    public void GameOver(PlayerType loser) {
        StopAllCoroutines();
        foreach (var player in players)
        {
            player.StunPlayer();
        }
    }

}
