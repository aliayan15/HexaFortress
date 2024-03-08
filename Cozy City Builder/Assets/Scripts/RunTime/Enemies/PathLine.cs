using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathLine : MonoBehaviour
{
    [SerializeField, Self] public LineRenderer Line;

    private void OnValidate()
    {
        this.ValidateRefs();
    }
}

