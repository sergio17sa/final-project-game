using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text player1Name; 
    public TMP_Text player2Name;
    public NetworkUIController networkUI;

}
