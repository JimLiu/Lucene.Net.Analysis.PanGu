#if NETCORE
namespace Microsoft.VisualBasic
{

    public enum VbStrConv
    {
        TraditionalChinese,
        SimplifiedChinese
    }

    public static class Strings
    {

        public static string StrConv(string str, VbStrConv strConv, int flag)
        {
            return str;
        }

    }

}
#endif