using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] Character _character;
    [SerializeField] Image _healthBarImage;
    [SerializeField] TextMeshProUGUI _actionsNumber;
    public EventHandler updateGroupUI;

    private void Start()
    {
        SetColorBar(_character.characterStats.teamColor);
        _character.OnGetDamaged += Character_OnGetDamaged;
        _character.OnHeal += Character_OnHeal;

        BaseAction.OnActionPerformed += BaseAction_OnActionPerformed;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChanged;

        UpdateHealthBar();
        ResetActionsText();
    }

    public void SetColorBar(Color color)
    {
        _healthBarImage.color = color;
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _character.GetNormalizeHealth();
    }

    private void ResetActionsText()
    {
        _actionsNumber.text = _character.ActionsCounter.ToString();
    }


    private void Character_OnGetDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Character_OnHeal(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void BaseAction_OnActionPerformed(object sender, EventArgs e)
    {
        BaseAction baseAction = (BaseAction)sender;
        Character character = baseAction.GetComponent<Character>();


        if (character == _character) 
        { 
            _actionsNumber.text = _character.ActionsCounter.ToString();

        }
    }

    private void TurnSystemManager_OnTurnChanged(object sender, EventArgs e)
    {
        ResetActionsText();
    }

    private void OnDisable()
    {
        _character.OnGetDamaged -= Character_OnGetDamaged;
        _character.OnHeal -= Character_OnHeal;
        
        BaseAction.OnActionPerformed -= BaseAction_OnActionPerformed;
        TurnSystemManager.Instance.OnTurnChanged -= TurnSystemManager_OnTurnChanged;
    }

}
