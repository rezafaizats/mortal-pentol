using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum PlayerType
{
    PLAYER_ONE,
    PLAYER_TWO
}

public class GameUIHandler : MonoBehaviour
{
    public static GameUIHandler Instance {get; private set;}

    [SerializeField] private Slider player1HealthBar;
    [SerializeField] private Slider player2HealthBar;

    [SerializeField] private TextMeshProUGUI player1Ammo;
    [SerializeField] private TextMeshProUGUI player2Ammo;

    [SerializeField] private PlayerIndicator player1Indicator;
    [SerializeField] private PlayerIndicator player2Indicator;


    private void Awake() {
        
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerHealthBar(PlayerType player, float healthAmount) {
        if(player == PlayerType.PLAYER_ONE) {
            DOTween.Init();
            player1HealthBar.DOValue(healthAmount, 0.2f);
            player1HealthBar.transform.DOShakePosition(0.5f, 10f, 25, 90);

            if(player1HealthBar.value <= 0)
                GameHandler.Instance.GameOver(PlayerType.PLAYER_ONE);
        }
        if(player == PlayerType.PLAYER_TWO) {
            DOTween.Init();
            player2HealthBar.DOValue(healthAmount, 0.2f);
            player2HealthBar.transform.DOShakePosition(0.5f, 10f, 25, 90);
            
            if(player2HealthBar.value <= 0)
                GameHandler.Instance.GameOver(PlayerType.PLAYER_TWO);
        }
    }

    public void UpdatePlayerAmmo(PlayerType player, int amount) {
        if(player == PlayerType.PLAYER_ONE) {
            player1Ammo.text = amount.ToString();
        }
        if(player == PlayerType.PLAYER_TWO) {
            player2Ammo.text = amount.ToString();
        }
    }

    public void SwitchBulletUI(PlayerType player, int ammoType) {
        if(player == PlayerType.PLAYER_ONE) {
            player1Indicator.SwitchPanel(ammoType);
        }
        if(player == PlayerType.PLAYER_TWO) {
            player2Indicator.SwitchPanel(ammoType);
        }
    }

}
