using System;
using UnityEngine;
using UnityEngine.AI;
using Work.JW.Code.Entities;

public class EntityMover : MonoBehaviour, IEntityComponent
{
    [SerializeField] private float toleranceValue = 0.5f;
    
    private NavMeshAgent _navAgent;
    private Entity _entity;
    
    public void Initialize(Entity entity)
    {
        _entity = entity;
        _navAgent = entity.GetComponent<NavMeshAgent>();

        _navAgent.updateRotation = false;
        
        SetSpeed(toleranceValue);
    }

    public void SetMovement(Vector3 movement)
    {
        _navAgent.SetDestination(movement);
    }

    public void WarpTo(Vector3 point)
    {
        if (NavMesh.SamplePosition(point, out NavMeshHit hit, 25, _navAgent.areaMask))
        {
            _entity.transform.position = hit.position;
        }
    }

    public void ForceWarpTo(Vector3 point)
    {
        _navAgent.enabled = false;
        _entity.transform.position = point;
        _navAgent.enabled = true;
    }
    
    public void SetSpeed(float speed) => _navAgent.speed = speed;
    
    public bool IsMoveStopped()
    {
        return !_navAgent.pathPending &&
               _navAgent.remainingDistance <= _navAgent.stoppingDistance &&
               (!_navAgent.hasPath || _navAgent.velocity.sqrMagnitude == 0f);
    }

    public void StopMovement(bool isStop, bool isReset = false)
    {
        _navAgent.isStopped = isStop;
        
        if(isReset)
            _navAgent.ResetPath();
    }
}
