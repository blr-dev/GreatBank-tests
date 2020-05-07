using Microsoft.VisualStudio.TestTools.UnitTesting;
using core;
using core.DbModels;
using System.Linq;

namespace WseiTests
{
    [TestClass]
    public class UserTransactionServiceTests
    {
        [TestMethod]
        public void GetUser_AfterRegister()
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
            UserTransactionService transactionService = new UserTransactionService();

            //act
            var user = transactionService.GetUser(user1.Id);

            //Assert
            // var transaction = transactionService.GetLastTransactionForUser(user.Id);
            Assert.IsNotNull(user, "user is null after register");
        }

        [TestMethod]
        public void Transaction_LastTransactionNotNull()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.Deposit(user.Id, 5);

            //Assert
            var lastTransaction = transactionService.GetLastTransactionForUser(user.Id);
            Assert.IsNotNull(lastTransaction, "Wrong last transaction is null");
        }

        [TestMethod]
        public void RawTransaction_WithValidAmount_AddedToTransactionList()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.AddTransaction(user.Id, 5, Transaction.TransactionType.Deposit);

            //Assert
            var allTransactions = transactionService.GetTransactionsForUser(user.Id);
            Assert.IsTrue(allTransactions.Count() > 0, "Wrong transaction not added");
        }

        [TestMethod]
        public void Deposit_WithValidAmount_AddedToTransactionList()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.Deposit(user.Id, 5);

            //Assert
            var allTransactions = transactionService.GetTransactionsForUser(user.Id);
            Assert.IsTrue(allTransactions.Count() > 0, "Wrong transaction list count");
        }

        [TestMethod]
        public void Deposit_LastTransactionSameType()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.Deposit(user.Id, 5);

            //Assert
            var lastTransaction = transactionService.GetLastTransactionForUser(user.Id);
            Assert.IsNotNull(lastTransaction, "lastTransaction is null");
            Assert.AreEqual(Transaction.TransactionType.Deposit, lastTransaction.GetTransactionType() == Transaction.TransactionType.Deposit, "Wrong last transaction type");
        }

        [TestMethod]
        public void Deposit_WithValidAmount_UpdatesLastTransactionBalance()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);

            decimal depositAmount1 = 5;
            decimal depositAmount2 = 2;
            decimal expected = depositAmount1 + depositAmount2;
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.Deposit(user.Id, depositAmount1);
            transactionService.Deposit(user.Id, depositAmount2);

            //Assert
            var balance = transactionService.GetCurrentBalanceForUser(user.Id);
            Assert.AreEqual(expected, balance, "Wrong balance value");
        }

        [TestMethod]
        public void Withdraw_WithValidAmount_AfterDeposit_UpdatesLastTransactionBalance()
        {
            //Arrange
            var user = new User
            {
                Username = "demouser",
                FirstName = "Demo",
                LastName = "User",
                Password = "demopass"
            };

            new AuthService().Register(user);

            decimal depositAmount = 500;
            decimal withdrawAmount = 150;
            decimal expected = depositAmount - withdrawAmount;
            UserTransactionService transactionService = new UserTransactionService();

            //act
            transactionService.Deposit(user.Id, depositAmount);
            transactionService.Withdraw(user.Id, withdrawAmount);

            //Assert
            var balance = transactionService.GetCurrentBalanceForUser(user.Id);
            Assert.AreEqual(expected, balance, "Wrong balance value");
        }
    }
}
