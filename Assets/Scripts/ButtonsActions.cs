using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsActions : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _turnText;
    public void EndTurn()
    {
        if(ActionManager.Instance.gameState == GameState.Player1Turn)
        {
            ActionManager.Instance.gameState = GameState.Player2Turn;
            _turnText.text = "Player 2 turn";
        }
        else if(ActionManager.Instance.gameState == GameState.Player2Turn)
        {
            ActionManager.Instance.gameState = GameState.Player1Turn;
            _turnText.text = "Player 1 turn";
        }
    }


}
