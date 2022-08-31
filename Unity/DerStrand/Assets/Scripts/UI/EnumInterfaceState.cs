using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumInterfaceState : MonoBehaviour
{
    public enum InterfaceState
    {
        Inventory,
        Crafting,
        Map
    }
    
    public InterfaceState currentState;
    public InterfaceState nextState;
}
