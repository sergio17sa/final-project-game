using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;

    private BaseAction _baseAction;

    public void SetButtonAction(BaseAction baseAction)
    {
        _baseAction = baseAction;
        _buttonText.text = baseAction.GetActionName().ToUpper();
        _button.onClick.AddListener(() =>
        {
            CharacterActionManager.Instance.SetSelectedAction(baseAction);
        });
    }

    private void SetButtonColor()
    {
        BaseAction selectedBaseAction = CharacterActionManager.Instance.GetSelectedAction();

        if (selectedBaseAction == _baseAction)
        {
            _buttonImage.color = Color.green;
        }
        else
        {
            _buttonImage.color = Color.white;
        }
    }

    private void SetInteractable()
    {
        Character selectedCharacter = CharacterActionManager.Instance.GetSelectedCharacter();
        if (selectedCharacter.ActionsTaken.Contains(_baseAction))
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }

        /*if (selectedCharacter.ActionsTaken.Count > 0 && _baseAction is HealAction)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }

        if (selectedCharacter.ActionsTaken.Contains(selectedCharacter.GetComponent<HealAction>()) && _baseAction is not HealAction)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }*/
    }

    public void UpdateSelectedVisual()
    {
        SetInteractable();
        SetButtonColor();
    }

    
}
