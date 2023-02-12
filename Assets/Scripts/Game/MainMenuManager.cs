using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public void BTN_SinglePlayer()
    {
        GameManager.instance.ChangeScene("TestingScene");
    }

    public void BTN_QuitConfirm()
    {
        Application.Quit();
    }

}
