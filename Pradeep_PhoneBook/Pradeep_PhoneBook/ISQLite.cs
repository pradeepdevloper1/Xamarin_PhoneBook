using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Pradeep_PhoneBook.Model;

namespace Pradeep_PhoneBook
{
    public interface ISQLite
    {
        SQLiteConnection GetConnectionWithCreateDatabase();

        bool SaveUser(User user);

        List<User> GetUser();
        bool UpdateUser(User user);

        bool DeleteUser(int Id);
    }
}
