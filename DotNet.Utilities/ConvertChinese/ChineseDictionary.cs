using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNet.Utilities.Properties;
using Newtonsoft.Json;

namespace DotNet.Utilities.ConvertChinese
{
    /// <summary>
    /// 汉子转拼音带声调
    /// </summary>
    public static class ChineseDictionary
    {
        public static List<WordInfo> Dictionary = JsonConvert.DeserializeObject<List<WordInfo>>(Resource.ChineseDictionaryJson);
        public static string ToPinyin(string content)
        {
            var stringBuilder = new StringBuilder();
            var charArray = content.ToCharArray();

            foreach (var c in charArray.Where(c => !String.IsNullOrWhiteSpace(c.ToString(CultureInfo.InvariantCulture))))
            {
                stringBuilder.Append(" " + (new Regex("^[\u4e00-\u9fa5]$").IsMatch(c.ToString(CultureInfo.InvariantCulture))
                                         ? GetPinyinWidthTonal(c) : c.ToString(CultureInfo.InvariantCulture)));
            }
            return stringBuilder.ToString();
        }

        private static String GetPinyinWidthTonal(Char chinese)
        {
            var wordInfo = Dictionary.FirstOrDefault(s => s.Chinese == chinese.ToString(CultureInfo.InvariantCulture));
            return wordInfo == null ? "" : wordInfo.Pinyin;
        }

        public class WordInfo
        {
            public String Chinese { get; set; }
            public String Pinyin { get; set; }
        }
    }
}
