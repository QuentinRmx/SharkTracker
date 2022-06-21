#nullable enable

using SQLite;
using static SharkTracker.Data.ERepositoryResponse;


namespace SharkTracker.Data
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        public string? Message { get; private set; } = String.Empty;

        public bool IsReady { get; private set; }

        private readonly string _dbPath;

        private SQLiteAsyncConnection? _connection;

        public Repository(string connectionString)
        {
            _dbPath = connectionString;
        }

        public async Task InitConnection()
        {
            if (_connection != null && IsReady)
                return;

            try
            {
                _connection = new SQLiteAsyncConnection(_dbPath);
                await _connection.CreateTableAsync<T>();
                IsReady = true;
            }
            catch (Exception e)
            {
                Message += $"Error in Init: {e.Message} ; {e.Source}";
            }
        }

        public async Task<ERepositoryResponse> Add(T row)
        {
            ERepositoryResponse result = Failure;
            try
            {
                await InitConnection();

                if (_connection is null)
                    return Failure;

                result = (ERepositoryResponse)await _connection.InsertAsync(row);
            }
            catch (Exception e)
            {
                Message += $"Error in Add: {e.Message} ; {e.Source}";
            }

            return result;
        }

        public async Task<ERepositoryResponse> Delete(T row)
        {
            ERepositoryResponse result = Failure;
            try
            {
                await InitConnection();

                if (_connection is null)
                    return Failure;

                result = (ERepositoryResponse)await _connection.DeleteAsync(row);
            }
            catch (Exception e)
            {
                Message += $"Error in Delete: {e.Message} ; {e.Source}";
            }

            return result;
        }

        public async Task<List<T>> GetAll()
        {
            List<T> results = new List<T>();

            try
            {
                await InitConnection();

                if (_connection is null)
                    return results;

                results = await _connection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                Message += $"Error in GetAll: {e.Message} ; {e.Source}";
            }

            return results;
        }

        public async Task<ERepositoryResponse> Update(T row)
        {
            ERepositoryResponse result = 0;
            try
            {
                await InitConnection();

                if (_connection is null)
                    return Failure;

                result = (ERepositoryResponse)await _connection.UpdateAsync(row);
            }
            catch (Exception e)
            {
                Message += $"Error in Update: {e.Message} ; {e.Source}";
            }

            return result;
        }
    }
}
