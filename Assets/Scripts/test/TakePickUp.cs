using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoreBourne
{
    public enum PickUpType
    {
        ammo,
        health,
        unique,
    };

    public struct TakePickUp
    {
        public int value;
        public PickUpType type;

    }
}

