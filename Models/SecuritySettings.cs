using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace DailyInk.Models
{
    public class SecuritySettings
    {
        [PrimaryKey]
        public int Id { get; set; } = 1;

        public string PinHash { get; set; } = "";
    }
}
