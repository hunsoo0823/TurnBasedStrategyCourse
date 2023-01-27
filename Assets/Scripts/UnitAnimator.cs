using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTranform;


    private void Awake()
    {
        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.onStartMoving += MoveAction_OnstartMoving;
            moveAction.onStopMoving += MoveAction_OnstopMoving;

        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.onShoot += shootAction_onShoot;
        }
    }

    private void MoveAction_OnstartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnstopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void shootAction_onShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTranform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosion();
        targetUnitShootAtPosition.y = shootPointTranform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
}
