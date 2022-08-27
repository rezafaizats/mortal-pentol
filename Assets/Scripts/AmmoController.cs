using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private int pentolAmount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDelay(2f)); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.GetComponent<PlayerController>().AddPentolAmmo(pentolAmount);
        Destroy(this);
    }

    IEnumerator DestroyDelay(float time) {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
