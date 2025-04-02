using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHomeTarget : MonoSingleton<TestHomeTarget>
{
    [field: SerializeField] public Transform Player { get; private set; }
}
