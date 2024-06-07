﻿using CSharpFunctionalExtensions;
using Exider.Services.External.FileService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exider.Core.Models.Storage
{
    [Table("attachments")]
    public class AttachmentModel
    {
        [Column("id")][Key] public Guid Id { get; private set; }
        [Column("name")] public string Name { get; private set; } = string.Empty;
        [Column("path")] public string Path { get; private set; } = string.Empty;
        [Column("type")] public string? Type { get; private set; } = string.Empty;
        [Column("size")] public long Size { get; private set; } = 0;
        [Column("userId")] public Guid UserId { get; private set; }

        [NotMapped] public byte[] File { get; set; } = new byte[0];

        private AttachmentModel() { }

        public static Result<AttachmentModel> Create(string name, string? type, long size, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(name))
                return Result.Failure<AttachmentModel>("Invalid name");

            if (size < 0)
                return Result.Failure<AttachmentModel>("Invalid size");

            if (userId == Guid.Empty)
                return Result.Failure<AttachmentModel>("User not found");

            Guid id = Guid.NewGuid();
            string path = Configuration.SystemDrive + $"__attachments__/{id}";

            return new AttachmentModel()
            {
                Id = id,
                Name = name,
                Path = path,
                Type = type,
                Size = size,
                UserId = userId
            };
        }

        public async Task<Result> SetFile(IFileService fileService)
        {
            var result = await fileService.ReadFileAsync(Path);

            if (result.IsFailure)
            {
                return result;
            }

            File = result.Value;
            return Result.Success();
        }
    }
}