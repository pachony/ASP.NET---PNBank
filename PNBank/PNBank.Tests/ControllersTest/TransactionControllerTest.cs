using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PNBank.Controllers;
using PNBank.Models;
using PNBank.Services;

namespace AutomatedTellerMachine.Tests.ControllersTest
{
    [TestClass]
    public class TransactionControllerTest
    {
        [TestMethod]
        public void IsMoneySendCorrectlyAfterTransfer()
        {
            var transfer = new TransferViewModel
            {
                CheckingAccountId = 1,
                DestinationCheckingAccountNumber = "000124TEST",
                Amount = 20
            };
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.CheckingAccounts = new FakeDbSet<CheckingAccount>();
            var checkingAccount = new CheckingAccount { Id = 1, AccountNumber = "000123TEST", Balance = 100 };
            var destinationCheckingAccount = new CheckingAccount { Id = 2, AccountNumber = "000124TEST", Balance = 0 };
            fakeDb.CheckingAccounts.Add(checkingAccount);
            fakeDb.CheckingAccounts.Add(destinationCheckingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();

            fakeDb.Transactions.Add(new Transaction { CheckingAccountId = 1, Amount = 100 });
            fakeDb.Transactions.Add(new Transaction { CheckingAccountId = transfer.CheckingAccountId, Amount = -transfer.Amount });
            fakeDb.Transactions.Add(new Transaction { CheckingAccountId = destinationCheckingAccount.Id, Amount = transfer.Amount });
            fakeDb.SaveChanges();

            var service = new CheckingAccountService(fakeDb);

            service.UpdateBalance(transfer.CheckingAccountId);
            service.UpdateBalance(destinationCheckingAccount.Id);

            Assert.AreEqual(20, destinationCheckingAccount.Balance);
            Assert.AreEqual(80, checkingAccount.Balance);
        }
        [TestMethod]
        public void IsBalanceCorrectAfterWithdrawal()
        {
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.CheckingAccounts = new FakeDbSet<CheckingAccount>();
            var checkingAccount = new CheckingAccount { Id = 1, AccountNumber = "000123TEST", Balance = 100 };
            fakeDb.CheckingAccounts.Add(checkingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();

            var service = new CheckingAccountService(fakeDb);
            var transaction = new Transaction { CheckingAccountId = 1, Amount = 20 };

            transaction.Amount = -transaction.Amount;
            fakeDb.Transactions.Add(new Transaction { Amount = 100, CheckingAccountId = 1 });
            fakeDb.Transactions.Add(transaction);
            fakeDb.SaveChanges();

            service.UpdateBalance(transaction.CheckingAccountId);

            Assert.AreEqual(80, checkingAccount.Balance);
        }

        [TestMethod]
        public void IsBalanceCorrectAfterDeposit()
        {
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.CheckingAccounts = new FakeDbSet<CheckingAccount>();
            var checkingAccount = new CheckingAccount { Id = 1, AccountNumber = "000123TEST", Balance = 0 };
            fakeDb.CheckingAccounts.Add(checkingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();
            var transactionController = new TransactionController(fakeDb);

            transactionController.Deposit(new Transaction { CheckingAccountId = 1, Amount = 25 });

            Assert.AreEqual(25, checkingAccount.Balance);
        }
    }
}
