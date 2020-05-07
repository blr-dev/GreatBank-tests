using Microsoft.VisualStudio.TestTools.UnitTesting;
using core;
using core.DbModels;
using System.Linq;

namespace WseiTests
{
    [TestClass]
    public class AuthServiceTests
    {
        [TestMethod]
        public void RegisterUser_IsRegistered() {
            //Arrange
            var user1 = new User
            {
                Username = "user1",
                FirstName = "test1",
                LastName = "test1",
                Password = "123"
            };

            AuthService auth = new AuthService();

            //act
            auth.Register(user1);

            //Assert
            User findUser;
            using (var db = new AppDbContext())
            {
                findUser = db.Users.FirstOrDefault(x => x.Id == user1.Id);
            }
            Assert.IsNotNull(findUser, "user is null after register");
        }

        [TestMethod]
        public void LoginRegisteredUser_IsAutchenticated()
        {
            //Arrange
            var user1 = new User
            {
                Username = "user1",
                FirstName = "test1",
                LastName = "test1",
                Password = "123"
            };

            AuthService auth = new AuthService();
            auth.Register(user1);

            //act
            var feedback = auth.Login(user1.Username, user1.Password);

            //Assert
            Assert.IsTrue(auth.IsAuthenticated(feedback.AuthKey), "user is not logged");
        }

        [TestMethod]
        public void LogoutRegisteredUser_IsAutchenticated()
        {
            //Arrange
            var user1 = new User
            {
                Username = "user1",
                FirstName = "test1",
                LastName = "test1",
                Password = "123"
            };

            AuthService auth = new AuthService();
            auth.Register(user1);
            var feedback = auth.Login(user1.Username, user1.Password);


            //act
            auth.Logout(feedback.AuthKey);

            //Assert
            Assert.IsFalse(auth.IsAuthenticated(feedback.AuthKey), "user is still logged");
        }
    }
}
