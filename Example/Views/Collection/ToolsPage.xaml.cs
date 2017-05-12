using EncodingUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        public BindingList<string> EncodingList { get; private set; }
        public ToolsPage()
        {
            InitializeComponent();
            EncodingList = new BindingList<string>()
            {
                "gb2312",
                "utf8",
                "utf7",
                "utf32",
                "unicode",
                "ascii",
                "default"
            };
            encodingCb.DataContext = EncodingList;
            encodingCb.SelectedIndex = 0;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            string cmd = (sender as Button).CommandParameter.ToString();
            switch (cmd)
            {
                case "ClearInput":
                    {
                        SourceTextBox.Clear();
                    }
                    break;
                case "ClearResult":
                    {
                        ResultTextBox.Clear();
                    }
                    break;
                default: break;
            }
        }

        private void Func_Click(object sender, RoutedEventArgs e)
        {
            string cmd = (sender as Button).CommandParameter.ToString();
            switch (cmd)
            {
                case "UTC":
                    {
                        ResultTextBox.Text = BasicEncoding.UnicodeToChinese(SourceTextBox.Text);
                    }
                    break;
                case "CTU":
                    {
                        ResultTextBox.Text = BasicEncoding.ChineseToUnicode(SourceTextBox.Text);
                    }
                    break;
                case "ATU":
                    {
                        ResultTextBox.Text = BasicEncoding.asciiToText(SourceTextBox.Text);
                    }
                    break;
                case "UTA":
                    {
                        ResultTextBox.Text = BasicEncoding.TextToAsc(SourceTextBox.Text);
                    }
                    break;
                case "CTUTF8":
                    {
                        ResultTextBox.Text = BasicEncoding.GB2312ToUtf8(SourceTextBox.Text);
                    }
                    break;
                case "UTF8TC":
                    {
                        ResultTextBox.Text = BasicEncoding.Utf8ToGB2312(SourceTextBox.Text);
                    }
                    break;
                case "Encode":
                    {
                        ResultTextBox.Text = HttpUtility.UrlEncode(SourceTextBox.Text, getEncoding(selectedValue));
                    }
                    break;
                case "Decode":
                    {
                        ResultTextBox.Text = HttpUtility.UrlDecode(SourceTextBox.Text, getEncoding(selectedValue));
                    }
                    break;
                case "ATN":
                    {
                        //ResultTextBox.Text = BasicEncoding.UnicodeToASCII(SourceTextBox.Text);
                    }
                    break;
                case "NTA":
                    {
                        ResultTextBox.Text = BasicEncoding.nativeToascii(SourceTextBox.Text);
                    }
                    break;
                default: break;
            }
        }

        private Encoding getEncoding(string value)
        {
            switch (value)
            {
                case "utf8": return Encoding.UTF8;
                case "utf7": return Encoding.UTF7;
                case "utf32": return Encoding.UTF32;
                case "unicode": return Encoding.Unicode;
                case "ascii": return Encoding.ASCII;
                case "default": return Encoding.Default;
                default: return Encoding.GetEncoding(value);
            }
        }

        private string selectedValue = "gb2312";
        private void encodingCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0].ToString() != selectedValue)
            {
                selectedValue = e.AddedItems[0].ToString();
            }
        }

        #region Countor

        public string CharCount
        {
            get { return charRun.Text; }
            set { charRun.Text = value; }
        }
        public string DigitCount
        {
            get { return digitRun.Text; }
            set { digitRun.Text = value; }
        }
        public string Punctuation
        {
            get { return puncRun.Text; }
            set { puncRun.Text = value; }
        }
        public string LetterCount
        {
            get { return letterRun.Text; }
            set { letterRun.Text = value; }
        }
        public string ChineseCount
        {
            get { return chineseRun.Text; }
            set { chineseRun.Text = value; }
        }
        public string WordsCount
        {
            get { return wordRun.Text; }
            set { wordRun.Text = value; }
        }
        public string ContentString
        {
            get { return countContent.Text; }
            set { countContent.Text = value; }
        }
        private string zeroString = "0";
        
        private void CountInvoke()
        {
            if (ContentString.Length < 1) return;
            int digitC = 0;
            int punctuation = 0;
            int letterC = 0;
            int chineseC = 0;
            int charC = ContentString.Length;
            int wordsC = System.Text.RegularExpressions.Regex.Matches(ContentString, "[A-Za-z]+").Count;
            foreach (char c in ContentString.ToCharArray())
            {
                if (Char.IsDigit(c)) digitC++;
                if (Char.IsPunctuation(c)) punctuation++;
                if (Char.IsLetter(c)) letterC++;
                if (c >= 0x4e00 && c <= 0x9fbb)
                {
                    chineseC++;
                }
            }
            CharCount = charC.ToString();
            DigitCount = digitC.ToString();
            Punctuation = punctuation.ToString();
            LetterCount = letterC.ToString();
            ChineseCount = chineseC.ToString();
            WordsCount = wordsC.ToString();
        }
        
        private void ClearAll()
        {
            ContentString = "";
            CharCount = zeroString;
            DigitCount = zeroString;
            Punctuation = zeroString;
            LetterCount = zeroString;
            ChineseCount = zeroString;
            WordsCount = zeroString;
        }

        private void CountBtnClick(object sender, RoutedEventArgs e)
        {
            CountInvoke();
        }

        private void ClearBtnClick(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        #endregion

        #region ASCII
        
        public string AsciiSource
        {
            get { return asciiSourceTB.Text; }
            set { asciiSourceTB.Text = value; }
        }
        
        public string AsciiResult
        {
            get { return asciiStringTB.Text; }
            set { asciiStringTB.Text = value; }
        }

        public string AsciiToString(int[] asciiCodes)
        {
            var builder = new StringBuilder(asciiCodes.Length);
            for (int i = 0; i < asciiCodes.Length; ++i)
            {
                int asciiCode = asciiCodes[i];
                if (asciiCode >= 0 && asciiCode <= 255)
                {
                    var asciiEncoding = new ASCIIEncoding();
                    byte[] byteArray = new byte[] { (byte)asciiCode };
                    string strCharacter = asciiEncoding.GetString(byteArray);
                    builder.Append(strCharacter);
                }
                else
                {
                    builder.Append(" ");
                }
            }
            return builder.ToString();
        }

        public string StringToHexAscii(string characters)
        {
            if (string.IsNullOrEmpty(characters)) return string.Empty;
            var builder = new StringBuilder(characters.Length);
            for (int i = 0; i < characters.Length; ++i)
            {
                string chara = characters[i].ToString();
                var asciiEncoding = new ASCIIEncoding();
                int asciicode = (int)asciiEncoding.GetBytes(chara)[0];
                builder.Append(asciicode.ToString("X2") + " ");
            }
            if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public string StringToAscii(string characters)
        {
            if (string.IsNullOrEmpty(characters)) return string.Empty;
            var builder = new StringBuilder(characters.Length);
            for (int i = 0; i < characters.Length; ++i)
            {
                string chara = characters[i].ToString();
                var asciiEncoding = new ASCIIEncoding();
                int asciicode = (int)asciiEncoding.GetBytes(chara)[0];
                builder.Append(asciicode + " ");
            }
            return builder.ToString();
        }

        private void Ascii2StringClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AsciiSource)) return;
            string[] codes = AsciiSource.Split(' ');
            List<int> asciicodelist = new List<int>(codes.Length);
            for (int i = 0; i < codes.Length; ++i)
            {
                if (string.IsNullOrEmpty(codes[i])) continue;
                int hexCode = 0;
                if (int.TryParse(codes[i], System.Globalization.NumberStyles.HexNumber, null, out hexCode))
                {
                    asciicodelist.Add(hexCode);
                }
            }
            if (asciicodelist.Count < 1) return;
            AsciiResult = AsciiToString(asciicodelist.ToArray());
        }

        private void String2AsciiClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AsciiResult)) return;
            AsciiSource = StringToHexAscii(AsciiResult);
        }
        #endregion

    }

    public class ChineseUnicode
    {
        public static string GetTranslate(string input, string cmd)
        {
            switch (cmd)
            {
                case "TextToAsc": return BasicEncoding.TextToAsc(input);
                case "ChrToText": return BasicEncoding.asciiToText(input);
                case "MsMyToCHR": return BasicEncoding.MsMyToCHR(input);
                case "OracleToCHR": return BasicEncoding.OracleToCHR(input);
                case "TextToUnicode": return BasicEncoding.TextToUnicode(input);
                case "UnicodeToText": return BasicEncoding.UnicodeToText(input);
                case "EncodeBase64": return BasicEncoding.EncodeBase64("windows-1252", input);
                case "DecodeBase64": return BasicEncoding.DecodeBase64("windows-1252", input);
                case "HexStringToString": return BasicEncoding.HexStringToString(input.Substring(2), Encoding.Default);
                case "UrlEncode": return HttpUtility.UrlEncode(input, Encoding.GetEncoding("GB2312"));
                case "UrlDecode": return HttpUtility.UrlDecode(input, Encoding.GetEncoding("GB2312"));
                default: return "";
            }
        }
    }
}
