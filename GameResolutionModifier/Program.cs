using System;
using GameResolutionModifier.Scripts;
using GameResolutionModifier.Scripts.Base;

namespace GameResolutionModifier
{
    internal static class Program
    {
        private static string _gameName;
        
        public static void Main()
        {
            Console.WriteLine("请输入要修改的游戏");
            Console.WriteLine("1.崩坏3 2.原神 3.战双帕弥什");
            int.TryParse(Console.ReadLine(), out var result);

            RegModifier controller = null;
            switch (result)
            {
                case 1:
                    controller = new HonkaiImpact3Controller();
                    _gameName = "崩坏3！";
                    break;
                case 2:
                    controller = new GenshinImpact();
                    _gameName = "原神！";
                    break;
                case 3:
                    controller = new PunishingGrayRaven();
                    _gameName = "战双！";
                    break;
                default:
                    Console.WriteLine(Environment.NewLine);
                    Main();
                    break;
            }

            if (controller != null)
            {
                Console.WriteLine(controller.Run() ? "使用配置修改成功" : "使用配置修改失败，已修改回1080P");
                Console.WriteLine(_gameName + "启动！");
                Console.WriteLine("按任意键退出");
                Console.ReadLine();
            }
        }
    }
}