using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum ErrorCode
{
    //other error codes can be aded for failed logins ect
    OperationDenied = -3,
    OperationInvalid = -2,
    InternalServerError = -1,
    OK = 0,
    UserNameInUse = 1,
    IncorectUserNameOrPassword = 2,
    UserCurrentlyLoggedIn = 3,
    NoProfileFound = 4,
    NotEnoughSpaceBux = 5,
}