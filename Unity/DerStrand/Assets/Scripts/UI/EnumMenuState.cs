using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumMenuState : MonoBehaviour
{
    public enum MenuState
    {
        MainMenu,
        Options,
        Game,
        Controls,
        Graphics,
        Audio,
    }
    public MenuState currentMenuState;
    public MenuState nextMenuState;
}
