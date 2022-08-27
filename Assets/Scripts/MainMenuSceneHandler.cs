using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuSceneHandler : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image bgImage;

    private void Start() {
        ChangeBGColor();
    }

    public void QuitApps() {
        Application.Quit();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void PlaySound(AudioClip sfx) {
        audioSource.clip = sfx;
        audioSource.Play();
    }

    public void ChangeBGColor() {

        bgImage.DOColor(RollColorSprite(), 10f).OnComplete( () => {
            ChangeBGColor();
        });

    }

    public Color RollColorSprite() {

        float red = Mathf.Clamp(Random.Range(0.2f, 0.8f), 0, 255);
        float green = Mathf.Clamp(Random.Range(0.2f, 0.8f), 0, 255);
        float blue = Mathf.Clamp(Random.Range(0.2f, 0.8f), 0, 255);

        Color rgbColor = new Color(red, green, blue, 1f);
        return rgbColor;
    }

}
