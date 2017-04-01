using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDevelop.Model
{
    public struct ViewTarget
    {
        /// <summary>
        /// 有效
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 改修目标
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 二号舰名称
        /// </summary>
        public string No2 { get; set; }
        /// <summary>
        /// 此目标可用的日期
        /// </summary>
        public IEnumerable<int> Weekdays { get; set; }
        /// <summary>
        /// 消耗装备
        /// </summary>
        public IEnumerable<string> Costs { get; set; }
    }
}
