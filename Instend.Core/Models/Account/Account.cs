﻿using CSharpFunctionalExtensions;
using Instend.Core.Dependencies.Services.Internal.Services;
using Instend.Core.Models.Access;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Instend.Core.Models.Account
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")][Key] public Guid Id { get; private set; } = Guid.NewGuid();
        [Column("name")] public string Name { get; private set; } = null!;
        [Column("surname")] public string Surname { get; private set; } = null!;
        [Column("nickname")] public string Nickname { get; private set; } = null!;
        [Column("email")] public string Email { get; private set; } = null!;
        [Column("avatar")] public string? Avatar { get; set; } = string.Empty;
        [Column("header")] public string Header { get; set; } = string.Empty;
        [Column("balance")] public decimal Balance { get; private set; } = 0;
        [Column("password")] public string Password { get; private set; } = null!;
        [Column("storage_space")] public double StorageSpace { get; private set; } = 1073741824;
        [Column("occupied_space")] public double OccupiedSpace { get; private set; } = 0;
        [Column("is_confirmed")] public bool IsConfirmed { get; private set; } = false;
        [Column("creation_datetime")] public DateTime RegistrationDate { get; private set; } = DateTime.Now;
        [Column("friend_count")] public uint FriendCount { get; private set; } = 0;

        public IEnumerable<Account> Followers { get; set; } = [];
        public IEnumerable<Account> Following { get; set; } = [];
        public IEnumerable<CollectionAccount> Collections { get; set; } = [];
        public IEnumerable<FileAccount> Files { get; set; } = [];
        public IEnumerable<AlbumAccount> Albums { get; set; } = [];
        public IEnumerable<Public.Publication> Publications { get; set; } = [];

        private Account() { }

        public static Result<Account> Create (string name, string surname, string nickname, string email, string password)
        {
            Func<string, bool> ValidateVarchar = (x)
                => !(string.IsNullOrEmpty(x) || x.Length > 45 || string.IsNullOrWhiteSpace(x));

            if (Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$") == false)
                return Result.Failure<Account>("Invalid email address");

            if (ValidateVarchar(name) == false)
                return Result.Failure<Account>("Invalid name");

            if (ValidateVarchar(surname) == false)
                return Result.Failure<Account>("Invalid surname");

            if (ValidateVarchar(nickname) == false)
                return Result.Failure<Account>("Invalid nickname");

            if (ValidateVarchar(password) == false || password.Length < 8)
                return Result.Failure<Account>("Invalid nickname");

            var user = new Account()
            {
                Name = name,
                Surname = surname,
                Nickname = nickname,
                Email = email,
                Password = password,
            };

            return Result.Success(user);
        }

        public void HashPassword(IEncryptionService encryptionService) 
            => Password = encryptionService.HashUsingSHA256(Password);

        public Result RecoverPassword(IEncryptionService encryptionService, string password)
        {
            if (password.Length < 8 || string.IsNullOrWhiteSpace(password))
                return Result.Failure("Invalid password");

            Password = password;
            HashPassword(encryptionService);

            return Result.Success();
        }

        public Result UpdateData(string name, string surname, string nickname)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return Result.Failure("Name required");

            if (string.IsNullOrEmpty(surname) || string.IsNullOrWhiteSpace(surname))
                return Result.Failure("Surname required");

            if (string.IsNullOrEmpty(nickname) || string.IsNullOrWhiteSpace(nickname))
                return Result.Failure("Nickname required");

            Name = name;
            Surname = surname;
            Nickname = nickname;

            return Result.Success();
        }

        public Result<double> UpdateOccupiedSpaceValue(double amountInBytes)
        {
            var result = OccupiedSpace + amountInBytes;

            if (result > StorageSpace)
                return Result.Failure<double>("Not enough space to perform this operation.");

            OccupiedSpace = Math.Max(0, result);
            return OccupiedSpace;
        }

        public void IncrementFriendCount() => FriendCount++;
        public void DecrementFriendCount() => FriendCount--;
        public void ConfirmAccount() => IsConfirmed = true;
    }
}