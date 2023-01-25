using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField] Character _character;
    public GameObject body;
    public RectTransform individualStatsPanel;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI healing;
    public LayerMask _groundLayerMask;
    public Vector3 adjustedPosition;
    public Vector2 boundaryMax;
    public bool canShow;
    public bool isInteracting;
    public bool isAction;

    private void Awake()
    {
        body = GameObject.FindGameObjectWithTag("StatsPanel");
    }

    public void Start()
    {
        individualStatsPanel = body.transform.GetChild(0).GetComponent<RectTransform>();
        icon = individualStatsPanel.gameObject.transform.GetChild(0).GetComponent<Image>();
        nameText = individualStatsPanel.gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        powerText = individualStatsPanel.gameObject.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        damage = individualStatsPanel.gameObject.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        healing = individualStatsPanel.gameObject.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        HideInfoPanel();
    }
    void HideInfoPanel()
    {
        icon.sprite = null;
        nameText.text = "";
        powerText.text = "";
        damage.text = "";
        healing.text = "";
        individualStatsPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (CharacterActionManager.Instance.GetSelectedCharacter() == _character)
        {
            isAction = true;
        }
        else
        {
            isAction = false;
        }
       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _groundLayerMask))
        {
            isInteracting = true;
           
            if (raycastHit.transform.GetComponent<Character>() == _character)
            {
                if (isAction)
                {
                    if (individualStatsPanel.gameObject.activeInHierarchy)
                    {
                        HideInfoPanel();
                        canShow = false;
                    }
                }
                else
                {
                    canShow = true;
                }

            }
        }
        else
        {
            isInteracting = false;
        }
        
        ShowInfoPanel();
    }

    void ShowInfoPanel()
    {
        if (!isInteracting)
        {
            if (canShow)
            {
                HideInfoPanel();
                canShow = false;
            }
            return;
        }
        
        if (canShow)
        {
            individualStatsPanel.gameObject.SetActive(true);
            icon.sprite = _character.characterStats.icon;
            nameText.text = _character.characterStats.characterName;
            powerText.text = _character.characterStats.initialLife.ToString();
            damage.text = _character.characterStats.powerAttack.ToString();
            healing.text = _character.characterStats.healing.ToString();
            MoveGraphic();
        }
    }

    void MoveGraphic()
    {
        Vector3 mousePosition = Input.mousePosition + adjustedPosition;
        Vector3 screenBounds = mousePosition;
        screenBounds.x = Mathf.Clamp(mousePosition.x, boundaryMax.x, boundaryMax.x * -1);
        screenBounds.y = Mathf.Clamp(mousePosition.y, boundaryMax.y, boundaryMax.y * -1);
        individualStatsPanel.transform.position = screenBounds;
    }
}
