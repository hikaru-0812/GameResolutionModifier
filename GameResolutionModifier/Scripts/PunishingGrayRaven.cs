using System;
using System.IO;
using System.Text;
using GameResolutionModifier.Scripts.Base;

namespace GameResolutionModifier.Scripts
{
    public sealed class PunishingGrayRaven : RegModifier
    {
        private const string RegKeyNameFullScreen = "Screenmanager Fullscreen mode_h3630240806";
        
        public PunishingGrayRaven()
        {
            RegPath = "kurogame\\战双帕弥什";
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
                    RegValueIsFullScreen = sr.ReadLine() == "true" ? "1" : "3";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("读取配置文件错误：" + e + "，使用默认配置");
            }
        }

        protected override void WriteRegeditWeightAndHeight()
        {
            WriteRegedit(RegKeyNameFullScreen, RegValueIsFullScreen, Base.Hex);
            base.WriteRegeditWeightAndHeight();
        }

        #endregion
    }
}