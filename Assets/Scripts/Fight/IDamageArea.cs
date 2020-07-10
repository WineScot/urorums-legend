using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageArea
{
    void DealDamage();
    void DestroyAfterTime(double time);
    void IntervalDamage(double intervalTime);
}
