using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTranform;
    [SerializeField] private Transform RiflePointTranform;
    [SerializeField] private Transform SwordTranform;


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
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.onSwordActionStarted += SwordAction_onSwordActionStarted;
            swordAction.onSwordActionCompleted += SwordAction_onSwordActionCompleted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_onSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_onSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
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

    private void EquipSword()
    {
        SwordTranform.gameObject.SetActive(true);
        RiflePointTranform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        SwordTranform.gameObject.SetActive(false);
        RiflePointTranform.gameObject.SetActive(true);
    }
}
