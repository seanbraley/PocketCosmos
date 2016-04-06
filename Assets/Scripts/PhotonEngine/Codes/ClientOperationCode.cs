using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags]
public enum ClientOperationCode
{
    Chat = 0x1,
    Login = 0x2,
    Region = 0x4
}