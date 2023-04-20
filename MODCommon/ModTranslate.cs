using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCommon
{
    public static class ModTranslate
    {
        private static Dictionary<string,string> TranslateDict = new Dictionary<string,string>();

        public static string getTranslate(this string s)
        {
            if(Localization.language!=Language.zhCN&&TranslateDict.ContainsKey(s))
            {
                return TranslateDict[s];
            }
            else
            {
                return s;
            }
        }

        public static void Init()
        {
            TranslateDict.Clear();
            TranslateDict.Add("辅助面板错误提示","Auxilaryfunction Error");
        }
    }
}