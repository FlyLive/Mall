using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services
{
    public class UserService : IDisposable
    {
        private MallDBContext _db;

        public UserService()
        {
            _db = new MallDBContext();
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
