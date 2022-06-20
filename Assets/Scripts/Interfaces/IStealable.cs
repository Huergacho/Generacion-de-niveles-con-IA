using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStealable
{
    GameObject gameObject { get; set; }
    Transform transform { get; set; }
}
