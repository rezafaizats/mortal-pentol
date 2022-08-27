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
    private PentolType currentBulletType = PentolType.NORMAL;

    [Header("Bullet Options")]
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private BulletController normalPentolBulletPrefabs;
    [SerializeField] private BulletController heavyPentolBulletPrefabs;

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

        if(Input.GetKeyDown(KeyCode.Alpha1) && playerStatus== PlayerType.PLAYER_ONE){
            currentBulletType = PentolType.NORMAL;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && playerStatus== PlayerType.PLAYER_ONE){
            currentBulletType = PentolType.SPRAY;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && playerStatus== PlayerType.PLAYER_ONE){
            currentBulletType = PentolType.HEAVY;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }

        if(Input.GetKeyDown(KeyCode.Keypad1) && playerStatus== PlayerType.PLAYER_TWO){
            currentBulletType = PentolType.NORMAL;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }

        if(Input.GetKeyDown(KeyCode.Keypad2) && playerStatus== PlayerType.PLAYER_TWO){
            currentBulletType = PentolType.SPRAY;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }

        if(Input.GetKeyDown(KeyCode.Keypad3) && playerStatus== PlayerType.PLAYER_TWO){
            currentBulletType = PentolType.HEAVY;
            GameUIHandler.Instance.SwitchBulletUI(playerStatus, (int)currentBulletType);
        }
        
        movementDir = new Vector2(Input.GetAxis(horizontalAxisInput), 0f);

        if(Input.GetKeyDown(KeyCode.W) && playerStatus == PlayerType.PLAYER_ONE) {
            if(currentBulletType == PentolType.NORMAL)
                ShootPentol();
            else if(currentBulletType == PentolType.SPRAY)
                StartCoroutine(SprayPentol(sprayPentolAmount, 0.25f));
            else if(currentBulletType == PentolType.HEAVY)
                ShootHeavyPentol();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && playerStatus == PlayerType.PLAYER_TWO) {
            if(currentBulletType == PentolType.NORMAL)
                ShootPentol();
            else if(currentBulletType == PentolType.SPRAY)
                StartCoroutine(SprayPentol(sprayPentolAmount, 0.25f));
            else if(currentBulletType == PentolType.HEAVY)
                ShootHeavyPentol();
        }
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

        if(pentolAmmoCollected <= 0)
            return;

        var bullet = Instantiate(normalPentolBulletPrefabs.gameObject, bulletSpawnPosition.position, Quaternion.identity).GetComponent<BulletController>();
        
        if(playerStatus == PlayerType.PLAYER_ONE)
            bullet.InitBullet(new Vector2(1, 1));
        else
            bullet.InitBullet(new Vector2(-1, 1));

        pentolAmmoCollected -= bullet.GetPentolCost();
    }

    IEnumerator SprayPentol(int sprayAmount, float sprayInterval) {

        if(pentolAmmoCollected <= 5)
            yield return null;

        pentolAmmoCollected -= 5;

        for (int i = 0; i < sprayAmount; i++)
        {                
            var bullet = Instantiate(normalPentolBulletPrefabs.gameObject, bulletSpawnPosition.position, Quaternion.identity).GetComponent<BulletController>();
            
            if(playerStatus == PlayerType.PLAYER_ONE)
                bullet.InitBullet(new Vector2(1, 1));
            else
                bullet.InitBullet(new Vector2(-1, 1));
            
            yield return new WaitForSeconds(sprayInterval);
        }

    }

    public void ShootHeavyPentol() {

        if(pentolAmmoCollected <= 10)
            return;

        var bullet = Instantiate(heavyPentolBulletPrefabs.gameObject, bulletSpawnPosition.position, Quaternion.identity).GetComponent<BulletController>();
        
        if(playerStatus == PlayerType.PLAYER_ONE)
            bullet.InitBullet(new Vector2(1, 1));
        else
            bullet.InitBullet(new Vector2(-1, 1));

        pentolAmmoCollected -= bullet.GetPentolCost();
    }

    public void StunPlayer() {
        isStunned = true;
    }

    public void StunPlayer(float duration) {
        isStunned = true;

        StartCoroutine(UnstunCharacter(duration));
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

    public void AddPentolAmmo(int amount) {
        pentolAmmoCollected += amount;
        GameUIHandler.Instance.UpdatePlayerAmmo(playerStatus, pentolAmmoCollected);
    }

}
