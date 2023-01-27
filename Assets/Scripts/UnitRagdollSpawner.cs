using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragdollprefab;
    [SerializeField] private Transform OriginRootBone;

    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollprefab, transform.position, transform.rotation);
        UnitRagdoll UnitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        UnitRagdoll.Setup(OriginRootBone);
    }
}
