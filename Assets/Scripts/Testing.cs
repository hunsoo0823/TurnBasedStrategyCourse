using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private void Start()
    {

    }


    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.T))
       {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startGridPostion = new GridPosition(0, 0);

            List<GridPosition> gridPostionList = Pathfinding.Instance.FindPath(startGridPostion, mouseGridPosition);

            for(int i = 0; i < gridPostionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPostionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPostionList[i + 1]),
                    Color.white,
                    10f
                );
            }
       }
    }


}
