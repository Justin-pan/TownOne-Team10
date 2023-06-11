using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWidePlatform : Platform
{
    protected override void DefineDimensions()
    {
        base.DefineDimensions();
        width = 2;
    }
}
