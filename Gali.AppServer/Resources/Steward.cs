using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{
    public class Steward<T>
    {
        static string conString = AppSettings.ConnStringMongoDBServer;
        static string dbString = AppSettings.ConnStringMongoDBDatabase;

        internal static MongoClient server = new MongoClient(conString);
        internal static IMongoDatabase database = server.GetDatabase(dbString);
        internal IMongoCollection<T> Collection;

        public Steward(string collectionName)
        {
            this.Collection = database.GetCollection<T>(collectionName);
        }

        public Result<List<T>> GetAll()
        {
            try
            {
                var docs = this.Collection.Find<T>(Builders<T>.Filter.Empty);
                var list = docs.ToList();

                return new Result<List<T>>() { State = ResultsStates.success, Data = list };
            }
            catch (Exception ex)
            {
                return new Result<List<T>>() { State = ResultsStates.error, Exception = ex };
            }
        }

        public Result<List<T>> GetBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var filter = Builders<T>.Filter.Where(predicate);
                var docs = Collection.Find<T>(filter);
                Result<List<T>> response = null;
                if (docs.Any())
                {
                    var list = docs.ToList();
                    return new Result<List<T>>() { State = ResultsStates.success, Data = list };
                }
                else
                {
                    response = new Result<List<T>>() { State = ResultsStates.empty };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new Result<List<T>>() { State = ResultsStates.error, Exception = ex };
            }
        }

        public Result<List<T>> GetById(Guid id, string idName)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(idName, id);
                var docs = Collection.Find<T>(filter);
                Result<List<T>> response = null;
                if (docs.Any())
                {
                    var list = docs.ToList();
                    return new Result<List<T>>() { State = ResultsStates.success, Data = list };
                }
                else
                {
                    response = new Result<List<T>>() { State = ResultsStates.empty };
                }

                return response;
            }
            catch (Exception ex)
            {
                return new Result<List<T>>() { State = ResultsStates.error, Exception = ex };
            }
        }
               
        public Result Insert(T data)
        {
            try
            {
                Collection.InsertOne(data);

                return new Result { State = ResultsStates.success };
            }
            catch (Exception ex)
            {
                return new Result() { State = ResultsStates.error, Exception = ex };
            }
        }

        public Result Update(T data, Guid id, string idName)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(idName, id);

                Collection.ReplaceOne(filter, data);
                return new Result { State = ResultsStates.success };
            }


            catch (Exception ex)
            {
                return new Result()
                {
                    State = ResultsStates.error,
                    Exception = ex
                };
            }
        }
    }
}
