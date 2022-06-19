using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;


namespace ServiceAppLab2
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ClientFunctionality : IFilter
    {

        public void FilterByInterchangeID(int id)
        {
            throw new NotImplementedException();
        }

        public void FilterByInterchangeIDandNode(int id, string node)
        {
            throw new NotImplementedException();
        }

        public void FilterByInterchangeNode(string node)
        {
            throw new NotImplementedException();
        }

        public void FilterByInterchangeNodeValue()
        {
            throw new NotImplementedException();
        }

        public void GetAllInterchanges()
        {
            throw new NotImplementedException();
        }

        public void GetTestData()
        {
            using(WebClient webClient = new WebClient())
            {
            webClient.DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL3ByaXZhdC5iYWhuaG9mLnNlL3diNzE0ODI5L2pzb24vdGVzdERhdGEuanNvbg ==")));
            }
          
        }

        public XElement Method1()
        {
            throw new NotImplementedException();
        }

        public XElement Method2()
        {
            throw new NotImplementedException();
        }
    }
}
