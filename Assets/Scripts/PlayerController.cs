using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private string horizontalAxisInput;
    [SerializeField] private string verticalAxisInput;

    private Rigidbody2D characterRB;
    private Vector2 movementDir;
    private Vector2 screenBounds;
    [SerializeField] private PlayerType playerStatus;
    [SerializeField] private float modelOffset;

    [SerializeField] private float characterHealth = 100f;
    [SerializeField] private int pentolAmmoCollected = 0;
    [SerializeField] private int sprayPentolAmount = 5;
    private bool isStunned;

    [Header("Bullet Options")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private BulletController normalPentolBulletPrefabs;
    [SerializeField] private GameObject heavyPentolBulletPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        characterRB = GetComponent<Rigidbody2D>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width), (Screen.height), Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if(isStunned)
            return;
        
        movementDir = new Vector2(Input.GetAxis(horizontalAxisInput), Input.GetAxis(verticalAxisInput));
    }

    void FixedUpdate() {
        MoveCharacter(movementDir);
    }

    void LateUpdate() {

        //Keep player position on screen and bound them on half of the screen
        Vector3 viewPos = transform.position;
        if(playerStatus == PlayerType.PLAYER_ONE)
            viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1, (screenBounds.x / 2) - modelOffset);
        if(playerStatus == PlayerType.PLAYER_TWO)
            viewPos.x = Mathf.Clamp(viewPos.x, (screenBounds.x / 2) - modelOffset, screenBounds.x);

        transform.position = viewPos;
    }

    public void MoveCharacter(Vector2 direction) {
        characterRB.velocity = direction * moveSpeed;
    }

    public void ShootPentol() {
        var bullet = Instantiate(normalPentolBulletPrefabs, bulletSpawnPosition.position, Quaternion.identity);
        
        if(playerStatus == PlayerType.PLAYER_ONE)
            bullet.GetComponent<BulletController>().InitBullet(new Vector2(1, 1));
        else
            bullet.GetComponent<BulletController>().InitBullet(new Vector2(-1, 1));
    }

    IEnumerator SprayPentol(int sprayAmount, float sprayInterval) {
        for (int i = 0; i < sprayAmount; i++)
        {                
            var bullet = Instantiate(normalPentolBulletPrefabs, bulletSpawnPosition.position, Quaternion.identity);
            
            if(playerStatus == PlayerType.PLAYER_ONE)
                bullet.GetComponent<BulletController>().InitBullet(new Vector2(1, 1));
            else
                bullet.GetComponent<BulletController>().InitBullet(new Vector2(-1, 1));
            
            yield return new WaitForSeconds(sprayInterval);
        }
    }

    public void ShootHeavyPentol() {
        var bullet = Instantiate(heavyPentolBulletPrefabs, bulletSpawnPosition.position, Quaternion.identity);
        
        if(playerStatus == PlayerType.PLAYER_ONE)
            bullet.GetComponent<BulletController>().InitBullet(new Vector2(1, 1));
        else
            bullet.GetComponent<BulletController>().InitBullet(new Vector2(-1, 1));
    }

    public void StunPlayer(float duration) {
        isStunned = true;

    }

    IEnumerator UnstunCharacter(float duration) {
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void SetDamage(float damage) {
        characterHealth -= damage;
        float healthConverted = characterHealth / 100f;
        GameUIHandler.Instance.SetPlayerHealthBar(playerStatus, healthConverted);
    }

}
