using CardItNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.interfaces
{
    public interface IpayerpayeeprivateService
    {
        //dynamic Save_payerpayeprivate(string token, payerpayeeprivate obj_payerpayeeprivate);
        dynamic Save_payerpayeprivate(payerpayeeprivate obj_payerpayeeprivate);
        //dynamic Save_payerpayeprivateDocument(payerpayeeprivate obj_payerpayeeprivate); 
        dynamic Get_rawresult();
        dynamic MaskedNumber(String source);
    }
}
