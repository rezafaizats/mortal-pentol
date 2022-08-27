using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PentolType
{
    NORMAL,
    SPRAY,
    HEAVY
}

public class BulletController : MonoBehaviour
{
    [SerializeField] private float pentolDamage = 5f;
    [SerializeField] private float pentolForce = 350f;
    [SerializeField] private float pentolLifetime = 2f;
    [SerializeField] private int pentolCost = 1;
    [SerializeField] private PentolType pentolType;

    [SerializeField] private Vector2 pentolDirection;
    private Rigidbody2D pentolRB;

    // Start is called before the first frame update
    void Start()
    {
        pentolRB = GetComponent<Rigidbody2D>();

        //Add force so it shoots out diagonally
        pentolRB.AddForce(pentolDirection * pentolForce);

        //Destroy pentol after lifetime ended
        StartCoroutine(DestroyDelay(pentolLifetime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.GetComponent<PlayerController>().SetDamage(pentolDamage);
        
        if(pentolType == PentolType.HEAVY)
            other.GetComponent<PlayerController>().StunPlayer(0.75f);

        Destroy(this.gameObject);
    }

    public void InitBullet(Vector2 direction) {
        pentolDirection = direction;
    }

    IEnumerator DestroyDelay(float time) {
        yield return new WaitForSeconds(time);
        Destroy(this);
    }

    public int GetPentolCost() {
        return pentolCost;
    }
}
