using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Character _character;
    [SerializeField] Image _healthBarImage;

    private void Start()
    {
        _character.OnGetDamaged += Character_OnGetDamaged;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = _character.GetNormalizeHealth();
    }

    private void Character_OnGetDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

}
