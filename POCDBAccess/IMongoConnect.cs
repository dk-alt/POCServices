using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace POCDBAccess
{ // public async Task<IQueryable<T>> GetStartsWith<T>(string collectionName, Expression<Func<T, bool>> expression, int skipCount, int limitCount) where T : class, new()
    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IMongoConnect
    {
        Task<IQueryable<T>> GetRegex<T>(string collectionName, Expression<Func<T, bool>> filterExpression, Expression<Func<T, string>> sortExpression, int skipCount, int limitCount) where T : class, new();
        Task<IQueryable<T>> GetRegexOne<T>(string collectionName, Expression<Func<T, bool>> filterExpression, Expression<Func<T, string>> sortExpression) where T : class, new();
        Task<IEnumerable<T>> GetAll<T>(string collectionName) where T : class, new();
        Task<IEnumerable<T>> Find<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new();
        Task<IEnumerable<T>> FindUsingCollation<T>(string collectionName, Expression<Func<T, bool>> filterExpression, string orderBy, int skipCount, int limitCount) where T : class, new();
        Task<IEnumerable<T>> Get<T>(string collectionName, Expression<Func<T, bool>> expression, int skipCount, int limitCount, string orderBy) where T : class, new();
        Task<T> FindOne<T>(string collectionName, Expression<Func<T, bool>> expression, string orderBy) where T : class, new();
        Task<T> FindOneAndDelete<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new();
        Task<T> FindOneWithoutSort<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new();
        Task<long> CollectionCount<T>(string collectionName, Expression<Func<T, bool>> expression) where T : class, new();
        Task InsertOne<T>(string collectionName, T item) where T : class, new();
        Task InsertMany<T>(string collectionName, List<T> item) where T : class, new();
        Task<UpdateResult> UpdateOne<T>(string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update) where T : class, new();
        Task<IEnumerable<T>> Find<T>(string collectionName, int? count, Expression<Func<T, bool>> expression, string sortCol) where T : class, new();
        Task FindOneAndUpdate<T>(string collectionName, Expression<Func<T, bool>> expression, UpdateDefinition<T> update) where T : class, new();
        Task FindOneAndUpdateSync<T>(string collectionName, Expression<Func<T, bool>> expression, UpdateDefinition<T> update) where T : class, new();
        Task<ReplaceOneResult> FindOneAndReplace<T>(string collectionName, Expression<Func<T, bool>> expression, T updatedDocument) where T : class, new();
        
       
        Task<IEnumerable<T>> FindSelectedFields<T>(string collectionName, Expression<Func<T, bool>> expression, ProjectionDefinition<T> builders) where T : class, new();
        Task<IEnumerable<T>> FindSelectedFields<T>(string collectionName, int? count, Expression<Func<T, bool>> expression, ProjectionDefinition<T> builders, string sortCol) where T : class, new();
       
        
        Task<IEnumerable<T>> Get<T>(string collectionName, Expression<Func<T, bool>> expression, int skipCount, int limitCount, string orderBy, bool isAscending);
        Task<bool> Delete<T>(string collectionName, FilterDefinition<T> filterDefinition);
        Task<bool> CollectionExists(string collectionName);

     
    }
}