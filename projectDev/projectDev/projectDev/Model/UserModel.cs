using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace projectDev.Model
{
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ProfName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfImage { get; set; }
    }
}
