using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;

    private BaseAction _baseAction;

    public void SetButtonAction(BaseAction baseAction)
    {
        _baseAction = baseAction;
        _buttonText.text= baseAction.GetActionName().ToUpper();
        _button.onClick.AddListener(() =>
        {
            CharacterActionManager.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = CharacterActionManager.Instance.GetSelectedAction(); 
        if(selectedBaseAction == _baseAction)
        {
            _buttonImage.color = Color.green;
        }
        else 
        {
            _buttonImage.color = Color.white;
        }
        
    }
}
