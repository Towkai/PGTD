namespace Data
{
    public enum DataKey { none, last_channel_string, last_role_int, last_ip_string, last_port_int, 
        n00_minion_type, n01_minion_type, n02_minion_type, n03_minion_type, n04_minion_type, n05_minion_type, n06_minion_type, n07_minion_type, n08_minion_type, n09_minion_type
    }
    public enum MinionType{ Cube, Cone, Cylinder }
    public static class ConstString
    {
        public struct PooledObject
        {
            public static readonly string[] MinionName = new string[]{"Rose", "TikTok", "You're awesome", "Ice Cream Cone", "Rosa", "GG", "Thai Milk Tea", "Little Sakura", "Sakura Mochi", "Doughnut"};
            public const string MinionTypeKey = "n{0:00}_minion_type";
            public const string Bullet = "Bullet";
            public const string Cube = "Cube";
            public const string Cone = "Cone";
            public const string Cylinder = "Cylinder";
            public const string S_Red_Cube = "S.Red.Cube";
            public const string S_Red_Cube2 = "S.Red.Cube2";
            public const string S_Blue_Cube = "S.Blue.Cube";
            public const string S_Blue_Cube2 = "S.Blue.Cube2";
        }
        public struct Scene
        {
            public const string Login = "Login";
            public const string InGame = "InGame";
        }
    }
}