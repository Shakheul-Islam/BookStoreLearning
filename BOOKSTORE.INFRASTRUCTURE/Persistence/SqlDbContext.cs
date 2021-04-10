using BOOKSTORE.DOMAIN.Common;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BOOKSTORE.INFRASTRUCTURE.Persistence
{
    public abstract class SqlDbContext<TEntity> where TEntity : class
    {
        private  SqlConnection _con;
        private SqlTransaction _trans;

        protected SqlDbContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _con = new SqlConnection(connectionString);
        }

        private async Task ConnectionOpenAsync()
        {
            if (_con.State == ConnectionState.Closed)
                await _con.OpenAsync();
        }

        private void ConnectionClose()
        {
            if (_con.State == ConnectionState.Open)
                _con.Close();
        }

        //private SqlConnection InitializeConnection() {

        //    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    _con = new SqlConnection(connectionString);
        //    return _con;
        //}

        public async Task<(Task<IEnumerable<T1>>, Task<IEnumerable<T2>>, Task<IEnumerable<T2>>)> GetMultipleAsync<T1, T2, T3>(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {
                try
                {
                    await ConnectionOpenAsync();
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var resultList = await connection.QueryMultipleAsync(sqlQuery, parameter);

                    var t1 = resultList.ReadAsync<T1>();
                    var t2 = resultList.ReadAsync<T2>();
                    var t3 = resultList.ReadAsync<T2>();

                    ConnectionClose();
                    return (t1, t2, t3);
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }

            }
        }


        public async Task<IEnumerable<TEntity>> GetListAsync(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {
                try
                {
                    await ConnectionOpenAsync();
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var resultList = await connection.QueryAsync<TEntity>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {

                try
                {
                    await ConnectionOpenAsync();
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var resultList = await connection.QueryAsync<T>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }

        public async Task<TEntity> GetSingleAsync(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {

                try
                {
                    await ConnectionOpenAsync();
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var resultList = await connection.QueryFirstOrDefaultAsync<TEntity>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }

        public async Task<T> GetSingleAsync<T>(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {

                try
                {
                    await ConnectionOpenAsync();
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var resultList = await connection.QueryFirstOrDefaultAsync<T>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }

        public async Task<string> GetSingleStringAsync(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {


                try
                {
                    await ConnectionOpenAsync();
                    var resultList = await connection.QueryFirstOrDefaultAsync<string>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }
        public async Task<Int32> GetSingleIntAsync(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {

                try
                {
                    await ConnectionOpenAsync();
                    var resultList = await connection.QueryFirstOrDefaultAsync<Int32>(sqlQuery, parameter);
                    ConnectionClose();
                    return resultList;
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }
        public async Task<ResultModel> SetSingleAsync(string sqlQuery, DynamicParameters parameter)
        {
            using (var connection = _con)
            {

                try
                {
                    await ConnectionOpenAsync();



                    using (_trans = connection.BeginTransaction())
                    {
                        try
                        {


                            var affectedRows = await connection.ExecuteAsync(sqlQuery, parameter, commandType: CommandType.StoredProcedure, transaction: _trans);
                            _trans.Commit();
                            ConnectionClose();
                            var result = parameter.Get<string>("@message");
                            return new ResultModel { Result = true, Message = result };
                        }
                        catch (Exception ex)
                        {
                            _trans.Rollback();
                            return new ResultModel { Result = false, Message = ex.Message };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConnectionClose();
                    return new ResultModel { Result = false, Message = ex.Message };
                }
                finally
                {
                    ConnectionClose();
                }
            }
        }

        public async Task<ResultModel> SetMultipleAsync(string sqlQuery, List<DynamicParameters> parameter)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))

            {

                await connection.OpenAsync();

                using (_trans = connection.BeginTransaction())
                {

                    try
                    {
                        var affectedRows = await connection.ExecuteAsync(sqlQuery, parameter, commandType: CommandType.StoredProcedure, transaction: _trans);
                         _trans.Commit();
                        ConnectionClose();
                        return new ResultModel { Result = true };
                    }
                    catch (Exception ex)
                    {
                        _trans.Rollback();
                        return new ResultModel { Result = false, Message = ex.Message };
                    }
                }

            }

        }
    }

}
