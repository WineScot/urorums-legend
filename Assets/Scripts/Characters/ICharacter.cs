using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void TakeDamage(IDamage damage);

    void TakeDamage(double value);
}