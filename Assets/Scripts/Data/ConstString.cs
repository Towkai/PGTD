namespace Data
{
    public enum DataKey { none, last_channel_string, last_role_int, last_ip_string, last_port_int, n1_Tiktok_minion_side}
    public static class ConstString
    {
        public struct PooledObject
        {
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