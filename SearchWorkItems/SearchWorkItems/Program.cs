using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json; 

namespace SearchWorkItems
{
    class Program
    {

        static void Main(string[] args)
        {
            var e = new ExecuteQuery();
            var query = e.RunGetBugsQueryUsingClientLib();

            Console.WriteLine(query);
            Console.ReadKey();       

        }
    } 
}
