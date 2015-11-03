
using System;
using System.Dynamic;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //dynamic proquestResult = HDAS.Client.Search.GetProquestSearchDynamic("test").DynamicObject;

            

            //var p = proquestResult;

            ////FSharp.Data.Runtime.BaseTypes.IJsonDocument test = HDAS.Client.Search.proquestSearch("test");
            //var test = new HDAS.Client.Search.testDynamic();
            //var dynObj = test.DynamicObject;


            //var x = dynObj;
            
            dynamic query = HDAS.Client.Search.testDataObject;
            foreach (var item in query.GetDynamicMemberNames())
            {
                Console.WriteLine(query[item]);
            }
            var y = query;
        }
    }
}
