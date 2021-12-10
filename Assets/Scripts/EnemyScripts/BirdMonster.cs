using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMonster : EnemyController
{
    private PinkPlayerController thePlayer;
    [SerializeField] private float flightSpeed;
    [SerializeField] private float patrolRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool isPlayerInRange;


    protected override void Start() /// child override
    {
        base.Start(); /// ACCESS parent start method
        thePlayer = FindObjectOfType<PinkPlayerController>();
    }

    private void Update()
    {
        isPlayerInRange = Physics2D.OverlapCircle(transform.position, patrolRange, playerLayer);
        
        if (isPlayerInRange)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                thePlayer.transform.position, flightSpeed * Time.deltaTime);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, patrolRange);
    }
}
