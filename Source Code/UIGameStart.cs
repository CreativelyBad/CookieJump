using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameStart : MonoBehaviour
{
    public GameObject menu;
    public GameObject hud;
    public GameObject lose;

    public bool gameStarted;
    protected bool isMenuShowing;

    private void Start()
    {
        isMenuShowing = true;
        gameStarted = false;
        hud.SetActive(false);
        lose.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMenuShowing == true)
        {
            isMenuShowing = !isMenuShowing;
            gameStarted = true;
            menu.SetActive(isMenuShowing);
            hud.SetActive(true);
        }
    }
}
