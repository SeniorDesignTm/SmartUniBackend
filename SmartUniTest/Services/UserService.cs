using Microsoft.EntityFrameworkCore;
using SmartUniTest.Helpers;
using SmartUniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartUniTest.Services
{
    public class UserService : IUserService
    {
        private DataContext _context;
        public UserService(DataContext context)

        {
            _context = context;



            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            ////string password = "12345";
            ////User user = new User { FirstName = "ahm", LastName = "Saad", Username = "Ahmed@hotmail.com" };
            ////Event @event = new Event { Description = "TEST", Date = "2019" };
            ////UserEvent userEvent = new UserEvent();
            ////byte[] passwordHash, passwordSalt;
            ////CreatePasswordHash(password, out passwordHash, out passwordSalt);

            ////Event @event = new Event { Description = "TEST", Date = "2019" };
            ////User user = context.Users.FirstOrDefault(s => s.Id == 1);
            ////@event.us

            ////user.PasswordHash = passwordHash;
            ////user.PasswordSalt = passwordSalt;
            ///


            //var users = new[]
            //{
            //        new User {FirstName = "ahm", LastName = "Saad", Username = "Ahmed@hotmail.com" },
            //         new User {FirstName = "test1", LastName = "fatherOfTest1", Username = "test1@hotmail.com" },
            //          new User {FirstName = "test2", LastName = "fatherOfTest2", Username = "test2@hotmail.com" }
            //    };

            //var events = new[]
            //{
            //        new Event { Description = "TEST", Date = "2019-01-28" },
            //        new Event { Description = "T55T", Date = "2017-03-21" },
            //        new Event { Description = "T442ST", Date = "2018-01-08" }
            //};
            //context.AddRange(new UserEvent { User = users[0], Event = events[0] },
            //    new UserEvent { User = users[0], Event = events[1] },
            //    new UserEvent { User = users[0], Event = events[2] },
            //    new UserEvent { User = users[1], Event = events[0] },
            //    new UserEvent { User = users[2], Event = events[1] }

            //    );



            //_context.Users.FirstOrDefault(u => u.Id == 1).
            _context.SaveChanges();
        }

        User IUserService.Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;

        }

        IQueryable IUserService.GetAll()
        {
            
            return (from User in _context.Users from Event in User.UserEvents orderby Event.Event.Date select new { User.Id, User.FirstName, User.LastName, User.Username, User.PasswordHash, User.PasswordSalt, Event.Event.Date, Event.Event.Description });
        }

        IQueryable IUserService.GetById(int id)
        {

            //var users = _context.Users.Include(ue => ue.UserEvents).ThenInclude(e => e.Event).ToList();
            var data = from User in _context.Users where User.Id == id
                       from Event in User.UserEvents
                       orderby Event.Event.Date
                       select new { Event.Event };
            return data;
            //return _context.Users.Include("Events").Where(u => u.Id == id).FirstOrDefault<User>();
            //return  _context.UserEvents.Include(i => i.User).FirstOrDefault(i => i.UserId == id);
        }

        User IUserService.Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        void IUserService.Update(User userParam, string password)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {

                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }


            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;


            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        void IUserService.Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            };
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    //    void IUserService.UpdateEvent(User userParam, Event @event)
    //    {
    //        var user = _context.Users.Find(userParam.Id);

    //        if (user == null)
    //            throw new AppException("User not found");

    //        UserEvent userEvent = new UserEvent { User = userParam, Event = @event };
    //        //user.UserEvents.Add(userEvent);

    //        _context.Users.Update(user);
    //        _context.SaveChanges();
    //    }
    //    IQueryable IUserService.GetEventsById(int id)
    //    {
    //        return from User in _context.Users where User.Id == id
    //               from Event in User.UserEvents select new { Event.Event };
    //    }
    }

}
