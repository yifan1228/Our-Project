using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Activate()
    {
        return;
    }
    void Activate(IEntity speller)
    {
        return;
    }
    float GetCooldown()
    {
        return 0f;
    }
}