using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Globals : MonoBehaviour
{
    public static Board Board = null;
    public static UserData UserData = null;
    public static SystemData SystemData = null;

    public static Color iconsColorTemp = new Color(0.8509804f, 0.7315621f, 0.2745098f, 1);
    public static Color iconsSelectedColorTemp = new Color(1f, 1f, 1f, 1f);
    public static Color bgColorTemp = new Color(0.8509804f, 0.7315621f, 0.2745098f, 1);
}
