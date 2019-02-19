using SmartUniTest.Models;
using SmartUniTest.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace SmartUniTest.Services
{
    public interface IUserService
    {
        User Authenticate(string user, string password);
        IQueryable GetAll();
        IQueryable GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password);
        void Delete(int id);

    }

}
