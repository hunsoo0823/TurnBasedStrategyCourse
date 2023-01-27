using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnbutton;
    [SerializeField] private GameObject enemyTurnVisualgameObject;
    // Start is called before the first frame update

    private void Start()
    {

        endTurnbutton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
            // code
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UPdateTurnText();
        UpdateEnemyTurnVisual();
    }

    private void TurnSystem_OnTurnChanged(object sneder, EventArgs e)
    {
        UPdateTurnText();
        UpdateEnemyTurnVisual(); 
    }

    private void UPdateTurnText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualgameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
}
