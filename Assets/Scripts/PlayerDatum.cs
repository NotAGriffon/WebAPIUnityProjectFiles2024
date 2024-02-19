using System;
using System.Collections.Generic;
[Serializable]
public class PlayerDatum
{
    public string _id;
    public int level;
    public double timeElapsed;
    public string name;
    public int __v;
}
[Serializable]
public class Root
{
    public PlayerDatum[] playerdata;
}