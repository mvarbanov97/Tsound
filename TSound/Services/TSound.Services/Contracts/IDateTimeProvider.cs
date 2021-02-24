using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    }
}
