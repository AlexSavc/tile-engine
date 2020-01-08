using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosive))]
public class Grenade : MonoBehaviour, IThrowable
{
    Explosive explosive;
}
