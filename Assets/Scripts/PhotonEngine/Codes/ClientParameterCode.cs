using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ClientParameterCode : byte
{
    SubOperationCode = 0,
    PlayerId,
    PeerId,
    Email,
    Password,
    UserName,
    UserId
}