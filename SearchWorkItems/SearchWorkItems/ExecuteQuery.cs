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
using MongoDB.Driver;
using SearchWorkItems;
using System.Globalization;

public class ExecuteQuery
{
    readonly string _uri;
    readonly string _personalAccessToken;
    readonly string _project;
    readonly IMongoClient _client;
    readonly IMongoDatabase _database;
    readonly IMongoCollection<WorkItems> _colNews;
    readonly WorkItems _doc;

    /// <summary>
    /// Constructor. Manually set values to match yourorganization. 
    /// </summary>
    public ExecuteQuery()
    {
        _uri = "https://dev.azure.com/42digial";
        _personalAccessToken = "";
        _project = "";
        _client = new MongoClient("mongodb://localhost");
        _database = _client.GetDatabase("azure_dev_ops");
        _colNews = _database.GetCollection<WorkItems>("workitems");
        _doc = new WorkItems();

    }

    /// <summary>
    /// Execute a WIQL query to return a list of bugs using the .NET client library
    /// </summary>
    /// <returns>List of Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem</returns>
    public async Task<List<WorkItem>> RunGetBugsQueryUsingClientLib()
    {
        Uri uri = new Uri(_uri);
        string personalAccessToken = _personalAccessToken;
        string project = _project;

        VssBasicCredential credentials = new VssBasicCredential("", _personalAccessToken);

        Wiql wiql = new Wiql()
        {
            Query = "Select [State], [Title], [Created Date], [Work Item Type]" +
            "From WorkItems " +
            "Where [System.TeamProject] = '" + project + "' "
        };
        //create instance of work item tracking http client
        using (WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(uri, credentials))
        {
            //execute the query to get the list of work items in the results
            WorkItemQueryResult workItemQueryResult = await workItemTrackingHttpClient.QueryByWiqlAsync(wiql);

            //some error handling                
            if (workItemQueryResult.WorkItems.Count() != 0)
            {
                //need to get the list of our work item ids and put them into an array
                List<int> list = new List<int>();
                foreach (var item in workItemQueryResult.WorkItems)
                {
                    list.Add(item.Id);
                }
                int[] arr = list.ToArray();

                //build a list of the fields we want to see
                string[] fields = new string[5];
                fields[0] = "System.Id";
                fields[1] = "System.Title";
                fields[2] = "System.State";
                fields[3] = "System.CreatedDate";
                fields[4] = "System.WorkItemType";

                //get work items for the ids found in query
                var workItems = await workItemTrackingHttpClient.GetWorkItemsAsync(arr, fields, workItemQueryResult.AsOf);

                Console.WriteLine("Query Results: {0} items found", workItems.Count);

                _colNews.Database.DropCollection("workitems");

                //loop though work items and write to console
                foreach (var workItem in workItems)
                {
                    _doc.Id = Convert.ToInt32(workItem.Id);
                    _doc.Title = workItem.Fields["System.Title"].ToString();
                    _doc.State = workItem.Fields["System.State"].ToString();
                    _doc.CreatedDate = DateTime.Parse(workItem.Fields["System.CreatedDate"].ToString());
                    _doc.WorkItemType = workItem.Fields["System.WorkItemType"].ToString();

                    _colNews.InsertOne(_doc);

                    Console.WriteLine("Id:{0} - Titulo:{1} - State:{2} Date:{3} - Type:{4}", _doc.Id, _doc.Title, _doc.State, _doc.CreatedDate, _doc.WorkItemType);
                }

                return workItems;
            }

            return null;
        }
    }
}