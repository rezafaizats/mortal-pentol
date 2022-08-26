using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerIndicator : MonoBehaviour
{
    public List<RectTransform> infoPanel;

    private RectTransform playerUI;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchPanel(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchPanel(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchPanel(2);
    }

    public void SwitchPanel(int index) {
        playerUI.DOMoveX(infoPanel[index].position.x, 0.25f);
    }

}
