using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Grabacr07.KanColleWrapper.Models;
using WhatToDevelop.Model.Raw;

namespace WhatToDevelop
{
    public class WikiCreeper
    {
        public List<Item> List { get; private set; }

        public WikiCreeper()
        {
            List = new List<Item>();
        }

        public void CreateList(string page)
        {
            page = Regex.Match(page, @"<td valign=""top"" id=""rgn_content4"".+", RegexOptions.Singleline).Value;
            page = Regex.Match(page, @"<table class=""style_table"".+?</table>", RegexOptions.Singleline).Value;
            page = Regex.Match(page, @"<tbody>.+?</tbody>", RegexOptions.Singleline).Value;
            List<string> trs = new List<string>();
            foreach (Match i in Regex.Matches(page, @"<tr>.+?</tr>", RegexOptions.Singleline))
                trs.Add(i.Value);
            
            // 过程变量
            Item LastItem = null;
            string CurrentType = string.Empty;

            foreach (string i in trs)
            {
                Match th = Regex.Match(i, @"<th.+?</th>");
                if (th.Success)
                    CurrentType = DropTags(th.Value);
                List<string> tds = new List<string>();
                foreach (Match j in Regex.Matches(i, @"<td.+?</td>"))
                    tds.Add(j.Value);

                No2Ship no2 = new No2Ship();
                switch (tds.Count())
                {
                    case 10:    // 正常
                        // 开始处理新项目
                        if (LastItem != null)
                            List.Add(LastItem);
                        LastItem = new Item();
                        
                        // 初始化新项目
                        LastItem.Type = CurrentType;
                        LastItem.Name = DropTags(tds.First());
                        LastItem.No2Info = new List<No2Ship>();
                        LastItem.IsFavorite = false;

                        no2.Name = DropTags(tds.Skip(8).First(), false, true);
                        if (no2.Name == "-")
                            no2.Name = string.Empty;
                        no2.Weekdays = GetWeekdays(tds.Skip(1).Take(7));
                        no2.Target = DropTags(tds.Skip(12).First());
                        LastItem.No2Info.Add(no2);

                        break;
                    case 8:     // 更多的2号舰
                        no2.Name = DropTags(tds.Last(), false, true);
                        if (no2.Name == "-")
                            no2.Name = string.Empty;
                        no2.Weekdays = GetWeekdays(tds.Take(7));
                        no2.Target = LastItem.No2Info.Last().Target;
                        LastItem.No2Info.Add(no2);
                        break;
                    case 9:    // 更多的改修目标
                        no2.Name = DropTags(tds.Skip(7).First());
                        if (no2.Name == "-")
                            no2.Name = string.Empty;
                        no2.Weekdays = GetWeekdays(tds.Take(7));
                        no2.Target = DropTags(tds.Skip(11).First());
                        LastItem.No2Info.Add(no2);
                        break;
                }
            }
            List.Add(LastItem);
            // 错误
            if (List.Count == 1)
                List.Clear();
        }

        /// <summary>
        /// 获取包含星期的数组
        /// </summary>
        /// <param name="weekdays">待处理HTML标记集合</param>
        /// <returns>数组</returns>
        private int[] GetWeekdays(IEnumerable<string> weekdays)
        {
            List<int> result= new List<int>();
            string[] list = weekdays.ToArray();
            for (int i = 0; i < list.Length; i++)
                if (list[i].Contains('〇'))
                    result.Add(i);
            return result.ToArray();
        }

        /// <summary>
        /// 移除 HTML 标签
        /// </summary>
        /// <param name="str">待处理文本</param>
        /// <param name="newline">是否将br解释为换行（默认否）</param>
        /// <returns>不含标签的文本</returns>
        private string DropTags(string str, bool newline = false, bool space = false)
        {
            string strReplace = newline ? Environment.NewLine : "";
            strReplace = space ? " " : strReplace;

            foreach (Match i in Regex.Matches(str, "<.+?>"))
            {
                if(i.Value.Contains("br"))
                    str = str.Replace(i.Value, strReplace);
                else
                    str = str.Replace(i.Value, "");
            }
            Match match = Regex.Match(str, @"(.+)(\*\d+)");
            if (match.Success)
                str = str.Replace(match.Groups[2].Value, "");
            if (space & str.Contains('，'))
                str = str.Replace('，', ' ');
            return str;
        }

        public void WriteFile()
        {
            // 复制选中状态
            foreach (Item i in MainViewModel.Current.SourceList.Where(j => j.IsFavorite))
            {
                Item val = List.Single(j => j.Name == i.Name);
                val.IsFavorite = true;
            }

            MainViewModel.Current.SourceList = List;
            MainViewModel.Current.WriteBack();
        }
    }
}
