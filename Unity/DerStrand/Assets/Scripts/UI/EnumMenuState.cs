using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumMenuState : MonoBehaviour
{
    public enum MenuState
    {
        MainMenu,
        Main,
        Load,
        Options,
        Game,
        Controls,
        Audio,
        Credits
    }
    public MenuState currentMenuState;
    public MenuState nextMenuState;
}
