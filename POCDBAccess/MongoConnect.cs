using MongoDB.Driver;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace POCDBAccess
{

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public class MongoConnect : IMongoConnect
    {
        private readonly IMongoDatabase database = null;

        #region Constructor
        public MongoConnect()
        {
            database = Connection.GetMongoConnection();
        }
        #endregion
        /// <summary>
        /// Method to fetch all the records from the Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll<T>(string collectionName) where T : class, new()
        {
            return await database.GetCollection<T>(collectionName).AsQueryable<T>().ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Overloaded Method to fetch the records from the database based on ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Find<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            return await data.Find<T>(expression).ToListAsync().ConfigureAwait(false);

        }


        public Task<T> FindOneAndDelete<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);

            return data.FindOneAndDeleteAsync<T>(expression);

        }







        /// <summary>
        /// Find one Comment 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<T> FindOne<T>(string collectionName, Expression<Func<T, bool>> expression, string orderBy) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            var sort = Builders<T>.Sort.Descending(orderBy);
            return data.Find<T>(expression).Sort(sort).Limit(1).SingleOrDefaultAsync();

        }

        public Task<T> FindOneWithoutSort<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);

            return data.Find<T>(expression).SingleOrDefaultAsync();

        }

        /// <summary>
        /// Get the documents count from the given collection based on given expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<long> CollectionCount<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            return await data.Find<T>(expression).CountAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Overloaded Method to fetch the records from the database based on ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindSelectedFields<T>(string collectionName, Expression<Func<T, bool>> expression, ProjectionDefinition<T> builders) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            return await data.Find<T>(expression).Project<T>(builders).ToListAsync().ConfigureAwait(false);
        }

        //    /*var result3 = collection.AsQueryable<Part>().OfType<Part>().Where(part =>
        //    part.ReferenceNumber.StartsWith(searchValue) ||
        //part.OemReferences.Any(oem => oem.ReferenceNumber.StartsWith(searchValue)) ||
        //part.AltReferences.Any(alt => alt.ReferenceNumber.StartsWith(searchValue)) ||
        //part.CrossReferences.Any(crs => crs.ReferenceNumber.StartsWith(searchValue)) ||
        //part.FormerReferences.Any(old => old.ReferenceNumber.StartsWith(searchValue))
        //);*/
        ///// <summary>   Get Starts with
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="collectionName"></param>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        public async Task<IQueryable<T>> GetRegex<T>(string collectionName, Expression<Func<T, bool>> filterExpression, Expression<Func<T, string>> sortExpression, int skipCount, int limitCount) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            IQueryable<T> query = data.AsQueryable<T>().Where(filterExpression).OrderBy(sortExpression).Skip(skipCount).Take(limitCount);
            return await Task.FromResult(query);
        }
        /// <summary>
        /// Overloaded Method to fetch the records from the database using collation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindUsingCollation<T>(string collectionName, Expression<Func<T, bool>> filterExpression, string orderBy, int skipCount, int limitCount) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            var sort = Builders<T>.Sort.Ascending(orderBy);
            return await data.Find<T>(filterExpression, new FindOptions() { Collation = new Collation("en", strength: CollationStrength.Secondary) }).Sort(sort).Skip(skipCount).Limit(limitCount).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IQueryable<T>> GetRegexOne<T>(string collectionName, Expression<Func<T, bool>> filterExpression, Expression<Func<T, string>> sortExpression) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            IQueryable<T> query = data.AsQueryable<T>().Where(filterExpression).OrderByDescending(sortExpression).Take(1);
            return await Task.FromResult(query);
        }

        /// <summary>
        /// Sorts documents and limit the no of documents to be returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="expression"></param>
        /// <param name="sort"></param>
        /// <returns>IEnumerable of type T</returns>
        public async Task<IEnumerable<T>> FindSelectedFields<T>(string collectionName, int? count, Expression<Func<T, bool>> expression, ProjectionDefinition<T> builders, string sortCol) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            var sort = Builders<T>.Sort.Descending(sortCol);
            var doc = data.Find(expression).Project<T>(builders).Sort(sort).Limit(count);
            return await doc.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get Comment trail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <param name="skipCount"></param>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Get<T>(string collectionName, Expression<Func<T, bool>> expression, int skipCount, int limitCount, string orderBy) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            var sort = Builders<T>.Sort.Descending(orderBy);
            return await data.Find<T>(expression).Sort(sort).Skip(skipCount).Limit(limitCount).ToListAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Method to Add the single collection in the Database database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task InsertOne<T>(string collectionName, T item) where T : class, new()
        {
            await database.GetCollection<T>(collectionName).InsertOneAsync(item).ConfigureAwait(false);
        }

        /// <summary>
        /// Method to insert the list of collection in the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task InsertMany<T>(string collectionName, List<T> item) where T : class, new()
        {
            await database.GetCollection<T>(collectionName).InsertManyAsync(item).ConfigureAwait(false);
        }
        /// <summary>
        /// Method to Update the sinlge collection in the Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateOne<T>(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update) where T : class, new()
        {
            var collection = database.GetCollection<BsonDocument>(collectionName);
            return await collection.UpdateOneAsync(filter, update).ConfigureAwait(false);

        }

        /// <summary>
        /// Sorts documents and limit the no of documents to be returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="expression"></param>
        /// <param name="sort"></param>
        /// <returns>IEnumerable of type T</returns>
        public async Task<IEnumerable<T>> Find<T>(string collectionName, int? count, Expression<Func<T, bool>> expression, string sortCol) where T : class, new()
        {
            var data = database.GetCollection<T>(collectionName);
            var sort = Builders<T>.Sort.Descending(sortCol);
            var doc = data.Find(expression)
                       .Sort(sort)
                       .Limit(count);
            return await doc.ToListAsync().ConfigureAwait(false);

        }

        /// <summary>
        /// Method to Find the specific document data and update it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <param name="update"></param>
        public async Task FindOneAndUpdate<T>(string collectionName, Expression<Func<T, bool>> expression, UpdateDefinition<T> update) where T : class, new()
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.FindOneAndUpdateAsync(expression, update).ConfigureAwait(false);
        }

        public async Task FindOneAndUpdateSync<T>(string collectionName, Expression<Func<T, bool>> expression, UpdateDefinition<T> update) where T : class, new()
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.FindOneAndUpdateAsync(expression, update);
        }
        public async Task<ReplaceOneResult> FindOneAndReplace<T>(string collectionName, Expression<Func<T, bool>> expression, T updatedDocument) where T : class, new()
        {
            var collection = database.GetCollection<T>(collectionName);
            return await collection.ReplaceOneAsync(expression, updatedDocument, new UpdateOptions { IsUpsert = true }).ConfigureAwait(false);
        }


        /// <summary>
        /// Overloaded method Get to fetch the data 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="expression"></param>
        /// <param name="skipCount"></param>
        /// <param name="limitCount"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Get<T>(string collectionName, Expression<Func<T, bool>> expression, int skipCount, int limitCount, string orderBy, bool isAscending)
        {
            var data = database.GetCollection<T>(collectionName);
            SortDefinition<T> sort = null;
            if (isAscending) sort = Builders<T>.Sort.Ascending(orderBy);
            else sort = Builders<T>.Sort.Descending(orderBy);
            var doc = data.Find(expression).Sort(sort).Skip(skipCount).Limit(limitCount);

            return await doc.ToListAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Delete Method
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="contentCommentId"></param>
        /// <returns></returns>
        public async Task<bool> Delete<T>(string collectionName, FilterDefinition<T> filterDefinition)
        {
            bool isSuccess = false;
            try
            {
                var collection = database.GetCollection<T>(collectionName);
                var deleteStatus = collection.DeleteMany(filterDefinition);
                if (deleteStatus.DeletedCount > 0) isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return await Task.FromResult(isSuccess);
        }

        /// <summary>
        /// Methods to check if the colleciton Exists or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public async Task<bool> CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            //filter by collection name
            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter }).Result.ToListAsync();
            //check for existence
            return collections.Any();
        }

    }
}