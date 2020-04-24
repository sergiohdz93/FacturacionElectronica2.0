using System.IO;
using SAPbobsCOM;
using System.Reflection;

namespace AddOn_FE_DIAN.Resources
{
    public class DBResourceExtension
    {

        public Company oCompany;
        private static string dbType = null;

        public DBResourceExtension(Company oCompany)
        {

            this.oCompany = oCompany;

        }

        public string GetSQL(string resource)
        {
            var ns = typeof(Program).Namespace;
            if (dbType == null)
                dbType = (oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB) ? "hana" : "sql";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ns + ".Resources." + dbType + "." + resource))
            {
                if (stream != null)
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

    }
}
