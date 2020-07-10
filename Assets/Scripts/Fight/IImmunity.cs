using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImmunity
{

}

abstract class Immunity
{
    protected IImmunity nextImmunity;
}