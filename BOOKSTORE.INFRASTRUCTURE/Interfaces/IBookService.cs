using BOOKSTORE.DOMAIN.Common;
using BOOKSTORE.DOMAIN.Model;
using BOOKSTORE.INFRASTRUCTURE.Services.BookServices.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOOKSTORE.INFRASTRUCTURE.Interfaces
{
   public interface IBookService:IDisposable
    {
        Task<List<Book>> GetAllEF(string searchBy);
        Task<List<Book>> GetAllSQL(string searchBy);
        Task<BookVM> GetByIdEF (long id );
        Task<BookVM> GetByIdSQL(long id);
        Task<ResultModel>InsertEF(BookVM book);
        Task<ResultModel> Update(BookVM book);
        Task<ResultModel> InsertOrUpdateSQL(BookVM book);
        Task<ResultModel> DeleteEF(long id);
        Task<ResultModel> DeleteSQL(long id);
    }

}
