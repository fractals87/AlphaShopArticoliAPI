using System;

namespace AlphaShopArticoliAPI.Models
{
    public class InfoMsg
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }

        public InfoMsg(DateTime date, string message)
        {
            this.Date = date;
            this.Message = message;
        }
    }
}
