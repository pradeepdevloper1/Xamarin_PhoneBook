using System;
using System.Collections.Generic;
using System.Text;
using Pradeep_PhoneBook.Model;
using Xamarin.Forms;
using System.IO;
using Pradeep_PhoneBook;
using SQLite;
using Xamarin.Forms.Internals;

[assembly: Dependency(typeof(SQLite_All))]
namespace Pradeep_PhoneBook
{
    public class SQLite_All:ISQLite
    {
        SQLiteConnection con;
        public SQLiteConnection GetConnectionWithCreateDatabase()
        {
            string dbName = "mydatabase.sqlite";
            string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(dbPath, dbName);
            con = new SQLiteConnection(path);
            con.CreateTable<User>();
            return con;
        }

        public bool SaveUser(User user)
        {
            bool response = false;
            try
            {
                con.Insert(user);
                response = true;
            }
            catch (Exception ex)
            {

                response = false;
            }
            return response;
        }

        public List<User> GetUser()
        {
            string sql = "SELECT * FROM User";
            List<User> users = con.Query<User>(sql);
            return users;
        }

        public bool UpdateUser(User user)
        {
            bool response = false;
            try
            {
                string sql = $"UPDATE User SET Name='{user.Name}',Address='{user.Address}',PhoneNumber='{user.PhoneNumber}'," +
                           $"Email='{user.Email}' WHERE Id={user.Id}";

                con.Execute(sql);
                response = true;

            }
            catch (Exception ex)
            {
                

            }
            return response;
        }
        public bool DeleteUser(int Id)
        {
            bool response = false;
            try
            {
                string sql = $"DELETE FROM User WHERE Id={Id}";
                con.Execute(sql);
                response = true;
            }
            catch (Exception ex)
            {


            }
            return response;
        }
    }
}
