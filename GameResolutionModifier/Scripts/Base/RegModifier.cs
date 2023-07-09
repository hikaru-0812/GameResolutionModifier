using System;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace GameResolutionModifier.Scripts.Base
{
    public abstract class RegModifier
    {
        private const string Software = "SOFTWARE";
        protected string RegPath;
        private const string RegKeyNameHeight = "Screenmanager Resolution Height_h2627697771";
        private const string RegKeyNameWeight = "Screenmanager Resolution Width_h182942802";
        
        protected string RegValueWeight = "1920";
        protected string RegValueHeight = "810";
        protected string RegValueIsFullScreen = "false";

        private readonly RegistryKey _part = Registry.CurrentUser.OpenSubKey(Software, true);

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
            var newLine = Environment.NewLine;
            //开始写入
            sw.Write("/*****************************************" + newLine);
            sw.Write("参考分辨率：" + newLine);
            sw.Write("16:9  1920X1080 2560X1440 3840X2160" + newLine);
            sw.Write("21:9  1920X810 2560X1090 3840X1646" + newLine);
            sw.Write("/*****************************************" + newLine);
            sw.Write("//宽：//" + newLine + "1920" + newLine + "//高：//" + newLine + "810" + newLine + "//是否全屏：//" + newLine + "false");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        protected void SkipFileHeader(StreamReader streamReader)
        {
            for (var i = 0; i < 5; i++)
            {
                streamReader.ReadLine();
            }
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
                if (_part is null)
                {
                    return false;
                }

                WriteRegeditWeightAndHeight();

                _part.Close();

                Console.WriteLine("Weight：" + RegValueWeight);
                Console.WriteLine("Height：" + RegValueHeight);
                Console.WriteLine("是否全屏：" + RegValueIsFullScreen);
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("出现错误：" + e.Message);
            }
            
            return false;
        }

        protected virtual void WriteRegeditWeightAndHeight()
        {
            WriteRegedit(RegKeyNameWeight, RegValueWeight, Base.Hex);
            WriteRegedit(RegKeyNameHeight, RegValueHeight, Base.Hex);
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
                    subKey = _part.CreateSubKey(RegPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    subKey?.SetValue(key, StringToBinary(value), RegistryValueKind.Binary);
                    break;
                case Base.Dec:
                    break;
                case Base.Hex:
                    subKey = _part.CreateSubKey(RegPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    subKey?.SetValue(key, value, RegistryValueKind.DWord);
                    break;
                default: //写入字符串
                    subKey = _part.CreateSubKey(RegPath);
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