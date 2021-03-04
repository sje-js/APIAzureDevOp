using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SearchWorkItems
{
    public class WorkItems
    {
        [BsonId()]
        public int Id { get; set; }

        [BsonElement("title")]
        [BsonRequired()]
        public string Title { get; set; }

        [BsonElement("state")]
        [BsonRequired()]
        public string State { get; set; }

        [BsonElement("created_date")]
        [BsonRequired()]
        public DateTime CreatedDate { get; set; }

        [BsonElement("workitem_type")]
        [BsonRequired()]
        public string WorkItemType { get; set; }
    
    }
}
