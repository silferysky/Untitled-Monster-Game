using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbility : MonoBehaviour
{
    public abstract void CallAbility();

    public abstract void DoAbility();

    public abstract void Start();

    public abstract void Update();

    public abstract float GetAbilityCooldownAsFraction();

    public abstract float GetAbilityCooldownTimer();
}
