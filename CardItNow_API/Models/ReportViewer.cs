using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Dynamic;
using nTireBO.Models;
using carditnow.Models;

namespace nTireBO.Models
{
    public class DeleteView
    {
        public string reportcode;
        public string pk;
    }
    public class SequenceView
    {

        public SequenceInfo[] data;
        public boreport report;
    }
    public class SequenceInfo
    {
        public int pk;
        public int? sequence;
        public bool change;
    }
    public class ProcedureInParam
    {
        public string ids;
        public string pk;
        public int companyid;
        public string menuid;
        public int userid;
        public string module;
        public int formid { get; set; }
        public string fkname { get; set; }
        public dynamic fk { get; set; }
        public string fkname1 { get; set; }
        public dynamic fk1 { get; set; }

        public dynamic dialogdata { get; set; }
    }
    public class ProcedureOutParam
    {
        public string outputparam;
        public string gotopage;
        public string gotoid;
        public string outputsql;
    }
    public class TableInfo
    {
        public string Alias;
        public IEnumerable<dynamic> colconfigs;
    }

    public class UpdateData
    {
        public string field;
        public string pk;
        public string val;
    }
    public class Upload
    {
        public int reportid;
        public dynamic data;
    }

    public class saveview
    {
        public int reportid;
        public string view;

        public string filters;
    }

    public class ReportSection
    {
        public dynamic Row;
        public dynamic header1;
        public dynamic footer1;
        public dynamic header1results;
    }
    public class ReportParam
    {
        public string id { get; set; }
        public dynamic formid { get; set; }
        public string SessionUser { get; set; }
        // public dynamic SessionUser;
        public dynamic parameters { get; set; }
        public dynamic addparams { get; set; }
        public string status { get; set; }
        public string modulename { get; set; }
        public string modulepkcol { get; set; }
        public string fkname { get; set; }
        public string fk { get; set; }

        public string fkname1 { get; set; }
        public string fk1 { get; set; }

        public string key { get; set; }
        public int? pkvalue { get; set; }
    }


}

