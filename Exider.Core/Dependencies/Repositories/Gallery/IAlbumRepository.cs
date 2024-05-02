﻿using CSharpFunctionalExtensions;
using Exider.Core.Models.Gallery;
using Exider.Services.External.FileService;

namespace Exider.Repositories.Gallery
{
    public interface IAlbumRepository
    {
        Task<Result<AlbumModel>> AddAsync(Guid ownerId, byte[] cover, string name, string description);
        Task<Result<AlbumModel[]>> GetAlbums(IImageService imageService, Guid userId);
    }
}