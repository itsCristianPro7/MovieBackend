using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Domain
{
    public class MongoDBSettings
    {
        public string ConnectionString = "mongodb://localhost:27017";
        public string DatabaseName = "moviesuggestiondb";
        public string MovieCollection = "movies";
    }
}
