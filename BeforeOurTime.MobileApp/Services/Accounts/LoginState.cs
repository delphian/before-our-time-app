using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Services.Accounts
{
    public enum LoginState : int
    {
        Guest = 10,
        Authenticating = 20,
        Authenticated = 30,
        Attaching = 40,
        Attached = 50,
        Disconnected = 60
    }
}
