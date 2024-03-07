using System;
using System.Collections.Generic;
[Serializable]
public class PlayerDatum
{
    // public string _id;
    public string screenName;
    public string firstName;
    public string lastName;
    public int date;
    public int score;
    public int __v;
}

[Serializable]
public class Root
{
    public PlayerDatum[] playerdata;
}