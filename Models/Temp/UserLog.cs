using System;
using System.Collections.Generic;

namespace ENROLLMENTSYSTEMBACKEND.Models.Temp;

public partial class UserLog
{
    public int UserLogId { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime UserLogTimeStamp { get; set; }

    public string UserLogActivity { get; set; } = null!;

    public string UserProfileImagePath { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
