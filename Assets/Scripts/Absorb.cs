using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour, IAbility
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(IEntity speller)
    {
        if (speller == null)
        {
            Debug.LogError("speller is null");
            return;
        }
        var triggeringObj = speller.TriggeringObject();
        if (triggeringObj == null)
        {
            Debug.LogError("speller.TriggeringObject() is null");
            return;
        }
        var entity = triggeringObj.GetComponent<IEntity>();
        if (entity == null)
        {
            Debug.LogError("triggeringObj.GetComponent<IEntity>() is null");
            return;
        }
        if (entity.GetGameObject() == null)
        {
            Debug.LogError("entity.GetGameObject() is null");
            return;
        }
        if (!entity.GetGameObject().activeInHierarchy)
        {
            Debug.LogError("entity.GetGameObject() is not active in hierarchy");
            return;
        }
        if (entity != null && entity.GetGameObject().activeInHierarchy && entity.IsDead())
        {
            var abilities = entity.GetAbilities();
            foreach (var ability in abilities)
            {
                speller.AddAbility(ability);
            }
            Destroy(entity.GetGameObject());
        }
    }
}
