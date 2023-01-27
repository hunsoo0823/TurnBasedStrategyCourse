using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINTS_MAX = 2;


    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] private bool isEnemy;


    [SerializeField] private Animator unitAnimator;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private ShootAction shootAction;
    private HealthSystem healthSystem;
    private GridPosition gridPosition;
    private BaseAction[] baseAcitionArray;
    private int actionPoints = ACTION_POINTS_MAX;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseAcitionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddSetUnitAtGridPostion(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += healthSystem_onDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            // Unit Change Grid Position
            GridPosition oldGridPostion = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPostion, newGridPosition);
        }
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public ShootAction GetShootAction()
    {
        return shootAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseAcitionArray()
    {
        return baseAcitionArray;
    }

    public bool TrySpendActionPointsToTackeAciton(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTackeAciton(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPoinstCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTackeAciton(BaseAction baseAction)
    {
        if(actionPoints >= baseAction.GetActionPoinstCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        actionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnTurnChanged(object sneder, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || 
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public Vector3 GetWorldPosion()
    {
        return transform.position;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void healthSystem_onDead(object sneder, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPostion(gridPosition, this);
        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
}
