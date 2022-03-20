using System;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace Game_Resolution_Modifier.Scripts.Base
{
    public class RegeditControler
    {
        private const string Software = "SOFTWARE";
        protected string regPath;
        private const string regKeyNameHeight = "Screenmanager Resolution Height_h2627697771";
        private const string regKeyNameWeight = "Screenmanager Resolution Width_h182942802";
        
        protected string regValueNameScreenSettingData = "{\"width\":1920,\"height\":810,\"isfullScreen\":false}.";
        protected string regValueWeight = "1920";
        protected string regValueHeight = "810";
        protected string isFullScreen = "false";

        private readonly RegistryKey part = Registry.CurrentUser.OpenSubKey(Software, true);

        /// <summary>
        /// 数据进制
        /// </summary>
        protected enum Base
        {
            /// <summary>
            /// 二进制
            /// </summary>
            Bin,
            
            /// <summary>
            /// 十进制
            /// </summary>
            Dec,
            
            /// <summary>
            /// 十六进制
            /// </summary>
            Hex
        }
        
        /// <summary>
        /// 读取配置数据
        /// </summary>
        protected virtual void Init()
        {
            if (File.Exists(@".\屏幕分辨率配置.txt")) return;
            
            var fs = new FileStream(@".\屏幕分辨率配置.txt", FileMode.Create);
            var sw = new StreamWriter(fs);
            //开始写入
            sw.Write("//宽：//" + Environment.NewLine + "1920" + Environment.NewLine + "//高：//" + Environment.NewLine + "810" + Environment.NewLine + "//是否全屏：//" + Environment.NewLine + "false");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 开始根据配置数据修改游戏分辨率
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            Init();
            
            try
            {
                if (part is null)
                {
                    return false;
                }

                WriteRegeditTemplate();

                part.Close();

                Console.WriteLine("Weigth：" + regValueWeight);
                Console.WriteLine("Height：" + regValueHeight);
                Console.WriteLine("是否全屏：" + isFullScreen);
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误：" + e.Message);
            }
            
            return false;
        }

        protected virtual void WriteRegeditTemplate()
        {
            WriteRegedit(regKeyNameWeight, regValueWeight, Base.Hex);
            WriteRegedit(regKeyNameHeight, regValueHeight, Base.Hex);
        }

        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="baseOfData">写入数据的进制</param>
        protected void WriteRegedit(string key, string value, Base baseOfData)
        {
            RegistryKey subKey;
            
            switch (baseOfData)
            {
                case Base.Bin:
                    subKey = part.CreateSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    subKey?.SetValue(key, StringToBinary(value), RegistryValueKind.Binary);
                    break;
                case Base.Dec:
                    break;
                case Base.Hex:
                    subKey = part.CreateSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    subKey?.SetValue(key, value, RegistryValueKind.DWord);
                    break;
                default: //写入字符串
                    subKey = part.CreateSubKey(regPath);
                    subKey?.SetValue(key, value, RegistryValueKind.String);
                    break;
            }
        }
        
        /// <summary>
        /// 字符串转化为二进制字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private byte[] StringToBinary(string str)
        {
            var encoding = new ASCIIEncoding();
            var data = encoding.GetBytes(str);
            return data;
        }

        /// <summary>
        /// 字符串转换为十六进制字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StringToHex(string str)
        {
            // var datas = Encoding.Default.GetBytes(str); //如果是汉字
            var datas = Encoding.ASCII.GetBytes(str); //如果是数字或者字符
            var result = new StringBuilder();

            foreach (var data in datas)
            {
                result.Append(data.ToString("X2"));
            }

            return result.ToString();
        }
    }
}