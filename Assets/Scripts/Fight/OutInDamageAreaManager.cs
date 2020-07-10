using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutInDamageAreaManager : MonoBehaviour, IDamageArea
{
    public HashSet<ICharacter> characters = new HashSet<ICharacter>();

    public void DealDamage()
    {
        //throw new NotImplementedException();
        foreach(ICharacter character in characters)
        {
            
        }
    }

    public void DestroyAfterTime(double time)
    {
        throw new NotImplementedException();
    }

    public void IntervalDamage(double intervalTime)
    {
        throw new NotImplementedException();
    }

    void OnTriggerEnter2D(Collider2D other) // object enter damage area
    {              
        ;
        ICharacter character;
        if (other.gameObject.TryGetComponent<ICharacter>(out character)) characters.Add(character);
    }

    void OnTriggerExit2D(Collider2D other) // object exit damage area
    {
        ICharacter character;
        if (other.gameObject.TryGetComponent<ICharacter>(out character)) characters.Remove(character);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
