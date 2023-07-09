using System;
using System.IO;
using System.Text;
using GameResolutionModifier.Scripts.Base;

namespace GameResolutionModifier.Scripts
{
    public sealed class HonkaiImpact3Controller : RegModifier
    {
        private const string RegKeyNameScreenSettingData = "GENERAL_DATA_V2_ScreenSettingData_h1916288658";
        private string _regValueNameScreenSettingData = "{\"width\":1920,\"height\":810,\"isfullScreen\":false}.";

        public HonkaiImpact3Controller()
        {
            RegPath = "miHoYo\\崩坏3";
        }

        #region 重写虚函数

        protected override void Init()
        {
            base.Init();
            
            try
            {
                using (var sr = new StreamReader(@".\屏幕分辨率配置.txt", Encoding.UTF8))
                {
                    SkipFileHeader(sr);
                    sr.ReadLine();
                    RegValueWeight = sr.ReadLine();
                    sr.ReadLine();
                    RegValueHeight = sr.ReadLine();
                    sr.ReadLine();
                    RegValueIsFullScreen = sr.ReadLine();
                    _regValueNameScreenSettingData = "{\"width\":" + RegValueWeight + ",\"height\":" + RegValueHeight + ",\"isfullScreen\":" + RegValueIsFullScreen + "}.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取配置文件错误：" + e + "，使用默认配置");
            }
        }

        protected override void WriteRegeditWeightAndHeight()
        {
            WriteRegedit(RegKeyNameScreenSettingData, _regValueNameScreenSettingData, Base.Bin);
            base.WriteRegeditWeightAndHeight();
        }

        #endregion
    }
}