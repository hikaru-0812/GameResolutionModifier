using System;
using System.IO;
using System.Text;
using Game_Resolution_Modifier.Scripts.Base;

namespace Game_Resolution_Modifier.Scripts
{
    public class HonkaiImpact3Controller : RegeditControler
    {
        private const string regKeyNameScreenSettingData = "GENERAL_DATA_V2_ScreenSettingData_h1916288658";
        private string regValueNameScreenSettingData = "{\"width\":1920,\"height\":810,\"isfullScreen\":false}.";

        public HonkaiImpact3Controller()
        {
            regPath = "miHoYo\\崩坏3";
        }

        protected override void Init()
        {
            base.Init();
            
            try
            {
                using (var sr = new StreamReader(@".\屏幕分辨率配置.txt", Encoding.UTF8))
                {
                    sr.ReadLine();
                    regValueWeight = sr.ReadLine();
                    sr.ReadLine();
                    regValueHeight = sr.ReadLine();
                    sr.ReadLine();
                    regValueIsFullScreen = sr.ReadLine();
                    regValueNameScreenSettingData = "{\"width\":" + regValueWeight + ",\"height\":" + regValueHeight + ",\"isfullScreen\":" + regValueIsFullScreen + "}.";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取配置文件错误：" + e + "，使用默认配置");
            }
        }

        protected override void WriteRegeditTemplate()
        {
            WriteRegedit(regKeyNameScreenSettingData, regValueNameScreenSettingData, Base.Bin);
            base.WriteRegeditTemplate();
        }
    }
}