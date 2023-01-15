using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAcition
{


    private float totalSpinAmount;

    private void Update()
    {
        if(isActive)
        {
            if(!isActive)
            {
                return;
            }
 
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount , 0);

            totalSpinAmount += spinAddAmount;
            if (totalSpinAmount > 360f)
            {
                isActive = false;
                onActionComplete();
            }
                
        }
    }

    public void Spin(Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
