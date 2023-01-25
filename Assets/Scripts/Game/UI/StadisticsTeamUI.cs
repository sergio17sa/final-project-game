using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StadisticsTeamUI : MonoBehaviour
{
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    [SerializeField] List<Holder> statsHolder = new List<Holder>();
    [SerializeField] GameObject holderPrefab;
    [SerializeField] GameObject holderLayaut;
    [SerializeField] Team team;

    private void Start()
    {
        BaseAction.OnActionPerformed += BaseAction_OnActionPerformedGroup;
        TurnSystemManager.Instance.OnTurnChanged += TurnSystemManager_OnTurnChangedGroup;
        Character.OnDead += grupalCharacterStats_OnDead;

        if (team == Team.Team1)
        {
            characters = SpawnManager.Instance.SpawnedMedievalTeam;
        }
        else
        {
            characters = SpawnManager.Instance.SpawnedFutureTeam;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            Holder newHolder = new Holder();
            newHolder.characterPrefab = characters[i].GetComponent<Character>();
            newHolder.body = Instantiate(holderPrefab, transform.position, Quaternion.identity, holderLayaut.transform);
            newHolder.icon = newHolder.body.transform.GetChild(0).GetComponentInChildren<Image>();
            newHolder.icon.sprite = newHolder.characterPrefab.characterStats.icon;
            newHolder.healSlider = newHolder.body.GetComponentInChildren<Slider>();
            newHolder.healSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = newHolder.characterPrefab.characterStats.teamColor;
            newHolder.healSlider.maxValue = 1;
            newHolder.actionsNumber = newHolder.body.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            newHolder.actionsNumber.text = newHolder.characterPrefab.ActionsCounter.ToString();
            newHolder.characterPrefab.OnGetDamaged += Character_OnGetDamaged;
            newHolder.characterPrefab.OnHeal += Character_OnHeal;
            statsHolder.Add(newHolder);
        }

        for (int i = 0; i < statsHolder.Count; i++)
        {
            UpdateHealthBar(statsHolder[i].characterPrefab);
            ResetActionsText(statsHolder[i].characterPrefab);
        }

    }

    private void UpdateHealthBar(Character character)
    {
        for (int i = 0; i < statsHolder.Count; i++)
        {
            if (character == statsHolder[i].characterPrefab)
            {
                statsHolder[i].healSlider.value = statsHolder[i].characterPrefab.GetNormalizeHealth();
            }
        }
    }

    private void ResetActionsText(Character character)
    {
        for (int i = 0; i < statsHolder.Count; i++)
        {
            if (character == statsHolder[i].characterPrefab)
            {
                statsHolder[i].actionsNumber.text = character.ActionsCounter.ToString();
            }
        }
    }

    private void Character_OnGetDamaged(object sender, EventArgs e)
    {
        Character character = (Character)sender;
        UpdateHealthBar(character);
    }

    private void Character_OnHeal(object sender, EventArgs e)
    {
        Character character = (Character)sender;
        UpdateHealthBar(character);
    }

    private void BaseAction_OnActionPerformedGroup(object sender, EventArgs e)
    {
        BaseAction baseAction = (BaseAction)sender;
        Character character = baseAction.GetComponent<Character>();

        for (int i = 0; i < statsHolder.Count; i++)
        {
            if (character == statsHolder[i].characterPrefab)
            {
                statsHolder[i].actionsNumber.text = statsHolder[i].characterPrefab.ActionsCounter.ToString();
            }
        }
    }
    private void TurnSystemManager_OnTurnChangedGroup(object sender, EventArgs e)
    {
        for (int i = 0; i < statsHolder.Count; i++)
        {
                ResetActionsText(statsHolder[i].characterPrefab);
        }
    }

    private void grupalCharacterStats_OnDead(object sender, EventArgs e)
    {
        Character character = (Character)sender;

        for (int i = 0; i < statsHolder.Count; i++)
        {
            if (character == statsHolder[i].characterPrefab)
            {
                Destroy(statsHolder[i].body);
                statsHolder.Remove(statsHolder[i]);
                
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < statsHolder.Count; i++)
        {
            statsHolder[i].characterPrefab.OnGetDamaged -= Character_OnGetDamaged;
            statsHolder[i].characterPrefab.OnHeal -= Character_OnHeal;
        }

        Character.OnDead -= grupalCharacterStats_OnDead;
    }
}

[Serializable]
public class Holder
{
    public Character characterPrefab;
    public GameObject body;
    public Image icon;
    public Slider healSlider;
    public TextMeshProUGUI actionsNumber;
}