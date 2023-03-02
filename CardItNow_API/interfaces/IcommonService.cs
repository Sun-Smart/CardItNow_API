using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carditnow.Models;
using nTireBO.Models;
using Microsoft.AspNetCore.Mvc;
using CardItNow.Models;

namespace CardItNow.Services
{
    public interface IcommonService
    {
        dynamic Getdocumenttype();
        dynamic GetBankList(); 
        dynamic GetPurposeList();
        dynamic Sociallogin(sociallogin model);
        dynamic SaveSocial(Savesocial model);
        string forgotpass(Savesocial model);
        dynamic Customerauth(customerauth model);
        string ChangePass(changepasscode model);
        string otpvalidate(verify_otp model);

        dynamic GetPrivacyclause();

        string duplicatetransactionvalidation(duplicatetransactionvalidation model);
    }
}
