using carditnow.Models;
using System;
using System.Collections.Generic;

namespace carditnow.Services
{
    public interface IsystemtableService
    {

        dynamic Get_systemtables();
        IEnumerable<Object> GetList(string key);
        IEnumerable<Object> GetListBy_tableid(int tableid);
        IEnumerable<Object> GetListBy_tablename(string tablename);
        dynamic Get_systemtable(string sid);
        dynamic Get_systemtable(int id);
        dynamic Save_systemtable(string token, systemtable obj_systemtable);
        dynamic Delete(int id);
    }
}
