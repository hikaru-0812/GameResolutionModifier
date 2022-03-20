using System;
using Game_Resolution_Modifier.Scripts;
using Game_Resolution_Modifier.Scripts.Base;

namespace Game_Resolution_Modifier
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("请输入要修改的游戏");
            Console.WriteLine("1.崩坏3 2.原神");
            int.TryParse(Console.ReadLine(), out var result);

            RegeditControler controler;
            if (result == 1)
            {
                controler = new HonkaiImpact3Controller();
            }
            else
            {
                controler = new GenshinImpactController();
            }

            Console.WriteLine(controler.Run() ? "修改成功" : "修改失败");

            Console.WriteLine("按任意键退出");
            Console.ReadLine();
        }
    }
}