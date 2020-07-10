using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    void Move(double vertialSpeed, double horizontalSpeed);

    void Move(double vertialSpeed, double horizontalSpeed, double time);

    void GetSlowEffect(double percent, double time);
}
