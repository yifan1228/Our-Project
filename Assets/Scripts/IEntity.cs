using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    int GetHealth()
    {
        return 0;
    }
    int GetMaxHealth()
    {
        return 0;
    }
    void Damaged(int damage)
    {
        return;
    }
    string GetName()
    {
        return "";
    }
    bool IsDead()
    {
        return false;
    }
    GameObject GetGameObject()
    {
        return null;
    }
    bool IsTowardsLeft()
    {
        return false;
    }
}