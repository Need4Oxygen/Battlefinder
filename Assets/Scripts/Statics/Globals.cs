using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Globals : MonoBehaviour
{
    // From brighter to dimmer, 1 is brighter than 2
    public static Dictionary<string, Color> Theme = new Dictionary<string, Color>()
    {
        {"untrained",new Color(0.8301887f,0.2310431f,0.2310431f,1f)},
        {"trained",new Color(0.3876017f,0.5849056f,0.3448736f,1f)},
        {"expert",new Color(0.3483447f,0.4861618f,0.7169812f,1f)},
        {"master",new Color(0.5815169f,0.3551086f,0.7169812f,1f)},
        {"leyend",new Color(0.8867924f,0.5815119f,0.3137237f,1f)},

        {"background_1",new Color(0.5215687f,0.2901961f,0.2901961f,1f)},
        {"background_2",new Color(0.245283f,0.1515664f,0.1515664f,1f)},
        {"text_1",new Color(0.945098f,0.8941177f,0.7921569f,1f)},
        {"text_2",new Color(0.8431373f,0.7607843f,0.6901961f,1f)}
    };

    public static UserData UserData = null;
    public static SystemData SystemData = new SystemData();
}
