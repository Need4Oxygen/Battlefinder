public class PF2E_Armor
{
    public int level;
    public int name;
    public string description;

    public int ac_bonus;                // 0, 1, 2...
    public float bulk;                  // 0, 0.1, 1, 2...
    public int dex_cap;                 // 0 to 5, maximum amount DEX adds to AC while wearing this armor

    public int check_strength;          // 12, 14, 16, 18
    public int check_penalty;           // 0, -1, -2, -3 to STR and DEX if check_strength > STR

    public string category;             // unarmored, light armor, medium armor, heavy armor
    public string group;                // leather, composite, plate

    public string speed_penalty;        // 0, -5, -10, speed
    public float price_gp;              // 15.32

    public string[] traits;
    public PF2E_Source source;
}
