using System.IO;
using UnityEngine;

public class SystemData
{
    public string PF2EFolderPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "PF2E";
    public string PF2ECampaignsPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "PF2E" + Path.DirectorySeparatorChar + "Campaigns";
    public string PF2EAdditionalContentPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "PF2E" + Path.DirectorySeparatorChar + "Additional Content";
    public string PF2ECampaignsPathSep = Application.persistentDataPath + Path.DirectorySeparatorChar + "PF2E" + Path.DirectorySeparatorChar + "Campaigns" + Path.DirectorySeparatorChar;
    public string PF2EAdditionalContentPathSpe = Application.persistentDataPath + Path.DirectorySeparatorChar + "PF2E" + Path.DirectorySeparatorChar + "Additional Content" + Path.DirectorySeparatorChar;

    public VideoSettings VideoSettings = null;
    public SoundSettings SoundSettings = null;
}
