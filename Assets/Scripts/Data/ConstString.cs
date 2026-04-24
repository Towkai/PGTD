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
            public static readonly string[] MinionNames = new string[]{"Rose", "TikTok", "You're awesome", "Ice Cream Cone", "Rosa", "GG", "Thai Milk Tea", "Little Sakura", "Sakura Mochi", "Doughnut"};
            public const string MinionType = "n{0:00}_minion_type";
            public const string MinionKey = "{0:00}.TikTok.{1}.{2}"; //{00}.TikTok.{Side}.{Tpye}
            public const string Bullet = "Bullet";
            public const string Cube = "Cube";
            public const string Cone = "Cone";
            public const string Cylinder = "Cylinder";
            public const string S_Red_Cube = "S.Red.Cube";
            public const string S_Red_Cube2 = "S.Red.Cube2";
            public const string S_Blue_Cube = "S.Blue.Cube";
            public const string S_Blue_Cube2 = "S.Blue.Cube2";

            static string GetMinionType(string name)
            {
                int minionNum = System.Array.IndexOf(MinionNames, name);
                if (minionNum < 0)
                {
                    return string.Empty;
                }
                string DataKey = string.Format(MinionType, minionNum); //取出類型
                if (System.Enum.TryParse<DataKey>(DataKey, out var result))
                {
                    var type = PlayerPrefsHelper.GetString(result); //獲得內容
                    type = string.IsNullOrEmpty(type) ? Cube : type;
                    return type;
                }
                else
                    return string.Empty;            
            }
            public static string GetMinionKey(string name)
            {
                int minionNum = System.Array.IndexOf(MinionNames, name);
                if (minionNum < 0)
                {
                    return string.Empty;
                }
                string type = GetMinionType(name);
                if (string.IsNullOrEmpty(type))
                {
                    return string.Empty;
                }
                return string.Format(MinionKey, minionNum, GameManager.Instance.MySide, type);             
            }

        }
        public struct Scene
        {
            public const string Login = "Login";
            public const string InGame = "InGame";
        }
    }
}