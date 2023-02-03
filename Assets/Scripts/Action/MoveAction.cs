using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler onStartMoving;
    public event EventHandler onStopMoving;

    [SerializeField] private Animator unitAnimator;
    [SerializeField] int maxMoveDistance = 4;

    private List<Vector3> PositionList;
    private int currentPostionIndex;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }
        Vector3 targetPosition = PositionList[currentPostionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPostionIndex++;
            if (currentPostionIndex >= PositionList.Count)
            {
                onStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPostionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int PathLength);


        currentPostionIndex = 0;
        PositionList = new List<Vector3>();

        foreach (GridPosition pathGridPostion in pathGridPostionList)
        {
            PositionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPostion));
        }

        onStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }


    public override List<GridPosition> GetVaildActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPostion = unit.GetGridPosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPostion = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPostion + offsetGridPostion;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(unitGridPostion == testGridPosition)
                {
                    // Same Grid Position where the unit is already at
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position already occupied with another unit
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPostion, testGridPosition))
                {
                    continue;
                }

                int pathfindingDistacneMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPostion, testGridPosition) > maxMoveDistance * pathfindingDistacneMultiplier)
                {
                    // Path length is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }

        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetActions<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction
        {
            gridpositon = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
