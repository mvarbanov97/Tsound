using System;
using System.Collections.Generic;
using System.Text;
using TSound.Services.Contracts;

namespace TSound.Services.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
