using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Block : ScriptableObject
{

    public Block n = null;
    public Block s = null;
    public Block e = null;
    public Block w = null;


    public Block prev = null;
    public Block next1 = null;
    public Block next2 = null;

    public int row;
    public int clm;

    public int sinceSplit = 0;
    public int sinceDead = 0;
    public int sinceStart = 0;

    public int status = 0; // 0 - unfilled, 1 = end, 2 = filled
    public bool success = false;

    public bool closedOff = false;

    public int[] dir = { 0, 0, 0, 0 };

}

