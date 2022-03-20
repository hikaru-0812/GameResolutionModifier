using System;
using System.IO;
using System.Text;
using Game_Resolution_Modifier.Scripts.Base;

namespace Game_Resolution_Modifier.Scripts
{
    public class GenshinImpactController : RegeditControler
    {
        private const string regKeyNameFullScreen = "Screenmanager Is Fullscreen mode_h3981298716";

        public GenshinImpactController()
        {
            regPath = "miHoYo\\原神";
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
                    regValueIsFullScreen = sr.ReadLine() == "true" ? "1" : "0";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取配置文件错误：" + e + "，使用默认配置");
            }
        }

        protected override void WriteRegeditTemplate()
        {
            WriteRegedit(regKeyNameFullScreen, regValueIsFullScreen, Base.Hex);
            base.WriteRegeditTemplate();
        }
    }
}