using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text playerServer; 
    public TMP_Text playerClient;
    public NetworkUIController networkUI;
}
