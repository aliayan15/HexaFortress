using System;
using UnityEngine;

/// <summary>
/// Expand class fields in another class. (Class MUST HAVE custom Editor sc)
/// </summary>
[AttributeUsage(AttributeTargets.Field)]  
public class ExpandableAttribute : PropertyAttribute
{
    
}

