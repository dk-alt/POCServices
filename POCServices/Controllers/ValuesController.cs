using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using POCDBAccess;
using POCEntities;
using POCServices.Filters;
using POCLogger;
namespace POCServices.Controllers
{
    
    public class ValuesController : ApiController
    {
        private readonly  IMongoConnect repository;
        public  ValuesController (IMongoConnect repository)
        {
            this.repository = repository;

        }
        // GET api/values
        [LogActionFilter]
        public IEnumerable<string> Get()
        {
            Logger.Info("Test");
            var test = repository.GetAll<Test>("TestData").Result.ToList();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
