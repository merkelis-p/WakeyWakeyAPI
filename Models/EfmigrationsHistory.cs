using System;
using System.Collections.Generic;

#nullable disable

namespace WakeyWakeyAPI.Models
{
    public partial class EfmigrationsHistory
    {
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
}
