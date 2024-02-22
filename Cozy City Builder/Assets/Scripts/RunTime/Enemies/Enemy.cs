using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float reachedPositionDistance = 1f;
    [SerializeField] private float moveSpeed;

    private List<Vector3> _pathVectorList;
    private int _pathIndex = -1;
    private Action _onReachedMovePosition;


    public void SetMovePosition(List<Vector3> pathVectorList, Action onReachedMovePosition)
    {
        this._onReachedMovePosition = onReachedMovePosition;
        _pathVectorList = pathVectorList;
        if (pathVectorList.Count > 0)
        {
            // Remove first position so he doesn't go backwards
            pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0)
        {
            _pathIndex = 0;
        }
        else
        {
            _pathIndex = -1;
            onReachedMovePosition();
        }
    }

    private void Update()
    {
        if (_pathIndex != -1)
        {
            // Move to next path position
            Vector3 nextPathPosition = _pathVectorList[_pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPositionDistance)
            {
                _pathIndex++;
                if (_pathIndex >= _pathVectorList.Count)
                {
                    // End of path
                    _pathIndex = -1;
                    _onReachedMovePosition();
                }
            }
        }
    }
}

