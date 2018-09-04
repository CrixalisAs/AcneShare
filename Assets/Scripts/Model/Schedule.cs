using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Date { get;private set; }
        public byte IsNotice { get; set; }
        public string Content { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int HourStart { get; set; }
        public int MinuteStart { get; set; }
        public int HourEnd { get; set; }
        public int MinuteEnd { get; set; }

        public Schedule(int id,string date, byte isNotice, string content)
        {
            Id = id;
            Date = date;

            string[] dates = date.Split('|');
            string[] time = dates[0].Split('/');
            Month = int.Parse(time[0]);
            Day = int.Parse(time[1]);
            Year = int.Parse(time[2]);
            HourStart = int.Parse(dates[1]);
            MinuteStart = int.Parse(dates[2]);
            HourEnd = int.Parse(dates[3]);
            MinuteEnd = int.Parse(dates[4]);
            IsNotice = isNotice;
            Content = content;
        }
    }
}
