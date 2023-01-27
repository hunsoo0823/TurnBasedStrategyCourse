using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> onShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit ShootingUnit;
    }

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private int maxShootDistance = 7;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosion() - unit.GetWorldPosion()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, rotateSpeed);
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            ShootingUnit = unit
        });
        targetUnit.Damage(40);
    }

    private void NextState()
    {
        switch(state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float CooloffStateTime = 0.5f;
                stateTimer = CooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }

    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetVaildActionGridPositionList()
    {
        GridPosition unitGridPostion = unit.GetGridPosition();
        return GetVaildActionGridPositionList(unitGridPostion);
    }

    public  List<GridPosition> GetVaildActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPostion;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if ( testDistance > maxShootDistance)
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the unit is already at
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }

        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridpositon = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1- targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetVaildActionGridPositionList(gridPosition).Count;
    }
}
