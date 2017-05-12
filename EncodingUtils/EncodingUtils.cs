using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingUtils
{
    public class BasicEncoding
    {
        public static string nativeToascii(string srcStr)
        {
            StringBuilder sb = new StringBuilder(srcStr.Length * 5);
            CharEnumerator ce = srcStr.GetEnumerator();
            while (ce.MoveNext())
            {
                sb.Append(Convert.ToString(sb.Replace("ffff", "\\u")));
            }
            return sb.ToString();
        }

        public static string EncodingConvert(string srcStr, Encoding srcEncoding, Encoding dstEncoding)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] dstBytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes, 0, srcBytes.Length);

            return dstEncoding.GetString(dstBytes, 0, dstBytes.Length);
        }

        #region UTF8 GB2312
        public static string Utf8ToGB2312(string src)
        {
            return EncodingConvert(src, Encoding.GetEncoding("gb2312"), Encoding.Default);
        }

        public static string GB2312ToUtf8(string src)
        {
            return EncodingConvert(src, Encoding.Default, Encoding.UTF8);
        }
        #endregion

        #region Chinese Unicode
        /// <summary>
        /// Chinese translate to unicode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ChineseToUnicode(string input)
        {
            string output = "";
            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    output += @"\u" + ((int)input[i]).ToString("x");
                }
            }
            return output;
        }

        /// <summary>
        /// Unicode translate to Chinese
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UnicodeToChinese(string input)
        {

            string output = "";
            if (!string.IsNullOrEmpty(input))
            {
                string[] strlist = input.Replace(@"\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        output += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    output = ex.Message;
                }
            }
            return output;
        }
        #endregion

        #region Chinese Unicode with js rule
        /// <summary>
        /// Unicode translate to Chinese（Match js rule)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UnicodeToChineseJS(string input)
        {
            string outStr = "";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(input, delegate (System.Text.RegularExpressions.Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            return outStr;
        }

        /// <summary>
        /// Chinese translate to  Unicode（Match js rule)
        /// </summary>
        /// <returns></returns>
        public static string ChineseToUnicodeJS(string input)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(input[i].ToString(), @"[\u4e00-\u9fa5]")) { outStr += "\\u" + ((int)input[i]).ToString("x"); }
                    else { outStr += input[i]; }
                }
            }
            return outStr;
        }
        #endregion

        #region HexString String
        /// <字符转16进制>
        /// Text to Hex
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return "0x" + result;
        }
        /// <16进制转字符>
        /// Hex to Text
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }
        #endregion

        #region ASCII UNICODE
        /// <字符转ASC>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TextToAsc(string str)
        {
            if (str == "") return str;
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append("&#" + Convert.ToString((int)c) + ";");
            }
            return sb.ToString();
        }
        /// <ASC转字符>
        /// </summary>
        /// <param name="asciiCode"></param>
        /// <returns></returns>
        private static string asciipattern = "\\&\\#(\\d+)";
        public static string asciiToText(string str)
        {
            if (str == "") return str;
            StringBuilder sb = new StringBuilder();
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(str, asciipattern);
            if (matches.Count < 1) return "";
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                sb.Append((Char)Int32.Parse(match.Groups[1].Value));
            }
            return sb.ToString();
            //string temp = "";
            //string[] s = str.Split(new char[] { ' ' });
            //for (int i = 0; i < s.Length; i++)
            //{
            //    if (Convert.ToInt32(s[i]) >= 0 && Convert.ToInt32(s[i]) <= 255)
            //    {
            //        string a = ((char)byte.Parse(s[i], System.Globalization.NumberStyles.Any)).ToString();
            //        temp += a;
            //    }

            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("ASCII Code is not valid.");
            //        //throw new System.ArgumentException("Error input ASC code between 0-255");
            //        //throw new Exception("ASCII Code is not valid.");
            //    }
            //}
            //return temp;

        }
        #endregion

        #region Chinese UNICODE
        /// <中文转为UNICODE字符>
        /// //中文转为UNICODE字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TextToUnicode(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符  
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }

            }
            return outStr;
        }
        /// <//UNICODE字符转为中文 >
        /// UNICODE字符转为中文 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnicodeToText(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }

                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }

            }
            return outStr;
        }
        #endregion

        #region Base64 encode decode
        /// <base64算法加密> 
        /// 将字符串使用base64算法加密 
        /// </summary> 
        /// <param name="code_type">编码类型（编码名称） 
        /// * 代码页 名称 
        /// * 1200 "UTF-16LE"、"utf-16"、"ucs-2"、"unicode"或"ISO-10646-UCS-2" 
        /// * 1201 "UTF-16BE"或"unicodeFFFE" 
        /// * 1252 "windows-1252" 
        /// * 65000 "utf-7"、"csUnicode11UTF7"、"unicode-1-1-utf-7"、"unicode-2-0-utf-7"、"x-unicode-1-1-utf-7"或"x-unicode-2-0-utf-7" 
        /// * 65001 "utf-8"、"unicode-1-1-utf-8"、"unicode-2-0-utf-8"、"x-unicode-1-1-utf-8"或"x-unicode-2-0-utf-8" 
        /// * 20127 "us-ascii"、"us"、"ascii"、"ANSI_X3.4-1968"、"ANSI_X3.4-1986"、"cp367"、"csASCII"、"IBM367"、"iso-ir-6"、"ISO646-US"或"ISO_646.irv:1991" 
        /// * 54936 "GB18030"    
        /// </param> 
        /// <param name="code">待加密的字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);  //将一组字符编码为一个字节序列. 
            try
            {
                encode = Convert.ToBase64String(bytes);  //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式. 
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        /// <base64算法解密> 
        /// 将字符串使用base64算法解密 
        /// </summary> 
        /// <param name="code_type">编码类型</param> 
        /// <param name="code">已用base64算法加密的字符串</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);  //将2进制编码转换为8位无符号整数数组. 
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);  //将指定字节数组中的一个字节序列解码为一个字符串。 
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
        #endregion

        #region CHR
        /// <OracleToCHR>
        /// Oracle to CHR
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string OracleToCHR(string str)
        {
            string temp = "";
            char[] s = str.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                temp += "Chr(" + TextToAsc(s[i].ToString()).Trim() + ")||";
            }
            temp = temp.Substring(0, temp.Length - 2);
            return temp;
        }
        /// <Ms&MyToCHR>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MsMyToCHR(string str)
        {
            string temp = "";
            char[] s = str.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                temp += "ChAr(" + TextToAsc(s[i].ToString()).Trim() + ")||";
            }
            temp = temp.Substring(0, temp.Length - 2);
            return temp;
        }
        #endregion
    }
    public class Native2AsciiUtils
    {

        /** 
         * prefix of ascii string of native character 

         */
        private static String PREFIX = "\\u";

        /** 
         * Native to ascii string. It's same as execut native2ascii.exe. 
         * @param str native string 
         * @return ascii string 
         */
        //public static String native2Ascii(String str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        sb.Append(char2Ascii(str[i]));
        //    }
        //    return sb.ToString();
        //}

        /** 
         * Native character to ascii string. 
         * @param c native character 
         * @return ascii string 
         */
        //private static String char2Ascii(char c)
        //{
        //    if (c > 255)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(PREFIX);
        //        int code = (c >> 8);
        //        String tmp =  Integer.toHexString(code);
        //        if (tmp.Length == 1)
        //        {
        //            sb.Append("0");
        //        }
        //        sb.Append(tmp);
        //        code = (c & 0xFF);
        //        tmp = Integer.toHexString(code);
        //        if (tmp.Length == 1)
        //        {
        //            sb.Append("0");
        //        }
        //        sb.Append(tmp);
        //        return sb.ToString();
        //    }
        //    else
        //    {
        //        return Character.toString(c);
        //    }
        ////}

        /** 
          * Ascii to native string. It's same as execut native2ascii.exe -reverse. 
          * @param str ascii string 
          * @return native string 
         */

        public static String ascii2Native(String str)
        {
            StringBuilder sb = new StringBuilder();
            int begin = 0;
            int index = str.IndexOf(PREFIX);
            while (index != -1)
            {
                sb.Append(str.Substring(begin, index));
                sb.Append(ascii2Char(str.Substring(index, index + 6)));
                begin = index + 6;
                index = str.IndexOf(PREFIX, begin);
            }
            sb.Append(str.Substring(begin));
            return sb.ToString();
        }

        /** 
         * Ascii to native character. 
         * @param str ascii string 
         * @return native character 
         */
        private static char ascii2Char(String str)
        {
            if (str.Length != 6)
            {
                throw new System.ArgumentException(
                "Ascii string of a native character must be 6 character.");
            }
            if (!PREFIX.Equals(str.Substring(0, 2)))
            {
                throw new System.ArgumentException(
                "Ascii string of a native character must start with \"\\u\".");
            }
            String tmp = str.Substring(2, 4);
            int code = Convert.ToInt32(tmp, 16) << 8;
            tmp = str.Substring(4, 6);
            code += Convert.ToInt32(tmp, 16);
            return (char)code;
        }
    }
}
