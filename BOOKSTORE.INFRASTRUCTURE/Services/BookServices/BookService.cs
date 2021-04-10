using BOOKSTORE.DOMAIN.Common;
using BOOKSTORE.DOMAIN.Model;
using BOOKSTORE.INFRASTRUCTURE.Common;
using BOOKSTORE.INFRASTRUCTURE.Interfaces;
using BOOKSTORE.INFRASTRUCTURE.Persistence;
using BOOKSTORE.INFRASTRUCTURE.Services.BookServices.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.INFRASTRUCTURE.Services.BookServices
{
    public class BookService : SqlDbContext<Book>, IBookService
    {
        private readonly BOOKSTORE.INFRASTRUCTURE.Persistence.AppContext _appContext;
        public BookService(BOOKSTORE.INFRASTRUCTURE.Persistence.AppContext appContext)
        {
            _appContext = appContext;
        }
        public void Dispose()
        {
            _appContext.Dispose();
        }


        public async Task<List<Book>> GetAllEF(string searchBy = null)
        {
            var list = _appContext.Books.Where(x => x.Deleted == false).AsQueryable();
            if (!string.IsNullOrEmpty(searchBy))
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchBy.ToLower())).AsQueryable();
            }

            return await list.ToListAsync();
        }

        public async Task<List<Book>> GetAllSQL(string searchBy = null)
        {
            string query = "select * from BOOKS where 1=1 and Deleted=0";
            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query + " and ((lower(Name) like lower( '%" + searchBy + "%')  or upper(Name)like upper('%" + searchBy + "%')  ) or  (lower(Code) like lower( '%" + searchBy + "%')  or upper(Code)like upper('%" + searchBy + "%')  ) )";
            }
            var bookList = await GetListAsync(query, null);
            var data = bookList.ToList();
            return data;

        }
        public async Task<BookVM> GetByIdEF(long id)
        {
            var entity = await _appContext.Books.FindAsync(id);
            var model = new BookVM();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Code = entity.Code;
            return model;
        }

        public async Task<BookVM> GetByIdSQL(long id)
        {
            string query = "(select top 1 * from BOOKS where Deleted=0 and Id = '" + id + "')";
            var entity = await GetSingleAsync(query, null);
            var model = new BookVM();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Code = entity.Code;
            return model;
        }

        //public async Task<Book> GetBookByNameSql(string name)
        //{
        //    string query = "(select top 1 * from BOOKS where Deleted=0 and LOWER(Name) = LOWER('" + name + "'))";
        //    var entity = await GetSingleAsync(query, null);
        //    return entity;
        //}

        //public async Task<Book> GetBookByCodeSql(string code)
        //{
        //    string query = "(select top 1 * from BOOKS where Deleted=0 and LOWER(Code) = LOWER('" + code + "'))";
        //    var entity = await GetSingleAsync(query, null);
        //    return entity;
        //}
        public async Task<Book> GetBookByName(string name)
        {
            return await _appContext.Books.Where(x => x.Name.Trim().Equals(name.Trim())).FirstOrDefaultAsync();
        }
        public async Task<Book> GetBookByCode(string code)
        {
            return await _appContext.Books.Where(x => x.Code.Trim().Equals(code.Trim())).FirstOrDefaultAsync();
        }

        public async Task<ResultModel> InsertEF(BookVM model)
        {

            try
            {
                if (await GetBookByName(model.Name) != null)
                {
                    return new ResultModel { Result = false, Message = $"{model.Name} is Already Exist!!" };
                }
                if (await GetBookByCode(model.Code) != null)
                {
                    return new ResultModel { Result = false, Message = $"{model.Code} is Already Exist!!" };
                }

                var entity = new Book
                {

                    Name = model.Name,
                    Code=model.Code,
                    Deleted = false,
                    CreateAt = DateTime.Now
                };

                _appContext.Books.Add(entity);
                await _appContext.SaveChangesAsync();
                return new ResultModel { Result = true, Message = $"{model.Name} is inserted successfully", Id = entity.Id };
            }
            catch (Exception e)
            {
                return new ResultModel { Result = false, Message = "Something wrong!!" };
            }



        }
        public async Task<ResultModel> Update(BookVM model)
        {

            try
            {
                var entity = await _appContext.Books.FindAsync(model.Id);
                if (entity == null)
                {
                    return new ResultModel { Result = false, Message = "No Record Found!!" };
                }

                entity.Name = model.Name;
                entity.UpdateAt = DateTime.Now;
                _appContext.SaveChanges();
                return new ResultModel { Result = true, Message = $"{model.Name} is Updated successfully", Id = entity.Id};
            }
            catch (Exception e)
            {
                return new ResultModel { Result = false, Message = "Something wrong!!" };
            }


        }
        public async Task<ResultModel> InsertOrUpdateSQL(BookVM model)
        {
            string query = "pro_book_save";

            DynamicParameters parameter = new DynamicParameters();

            parameter.Add("@p_book_id", model.Id, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@p_book_name", model.Name, DbType.String, ParameterDirection.Input);
            parameter.Add("@p_book_code", model.Code, DbType.String, ParameterDirection.Input);
            parameter.Add("@message", "", DbType.String, ParameterDirection.Output);

            return await SetSingleAsync(query, parameter);
        }

        public async Task<ResultModel> DeleteEF(long id)
        {
            try
            {
                var model = await _appContext.Books.FindAsync(id);
                if (model != null)
                {
                    model.Deleted = true;
                    await _appContext.SaveChangesAsync();
                    return new ResultModel { Result = true, Message = $"{model.Name} is Deleted Successfully" };
                }
                else
                {
                    return new ResultModel { Result = false, Message = $"No Record Found!!" };
                }
               
            }
            catch (Exception e)
            {
                return new ResultModel { Result = false, Message = e.Message };
            }
        }

        public async Task<ResultModel> DeleteSQL(long id)
        {
            string query = "pro_book_delete";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@book_id", id, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@message", "", DbType.String, ParameterDirection.Output);

            return await SetSingleAsync(query, parameter);
        }


    }
}
