﻿using CSharpFunctionalExtensions;
using Exider.Core;
using Exider.Core.Models.Gallery;
using Exider.Services.External.FileService;
using Microsoft.EntityFrameworkCore;

namespace Exider.Repositories.Gallery
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly DatabaseContext _context = null!;

        public AlbumRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Result<AlbumModel>> AddAsync(Guid ownerId, byte[] cover, string name, string? description)
        {
            var albumModel = AlbumModel.Create(name, description, DateTime.Now, DateTime.Now, ownerId, Configuration.AccessTypes.Private);

            if (albumModel.IsFailure)
            {
                return Result.Failure<AlbumModel>(albumModel.Error);
            }

            await File.WriteAllBytesAsync(albumModel.Value.Cover, cover);

            await _context.AddAsync(albumModel.Value);
            await _context.SaveChangesAsync();

            return Result.Success(albumModel.Value);
        }

        public async Task<Result<AlbumModel[]>> GetAlbums(IImageService imageService, Guid userId)
        {
            AlbumModel[] albums =  await _context.Albums.AsNoTracking()
                .Where(x => x.OwnerId == userId).ToArrayAsync();

            foreach (AlbumModel album in albums)
            {
                await album.SetCover(imageService);
            }

            return Result.Success(albums);
        }

        public async Task<Result<AlbumModel>> DeleteAlbumAsync(Guid id, Guid userId)
        {
            AlbumModel? album = await _context.Albums
                .FirstOrDefaultAsync(x => x.Id == id);

            if (album == null)
            {
                return Result.Failure<AlbumModel>("Album not found");
            }
            
            int result = await _context.Albums.Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (result == 0)
            {
                return Result.Failure<AlbumModel>("Album not found");
            }

            File.Delete(album.Cover);
            return Result.Success(album);
        }

        public async Task<AlbumModel?> GetByIdAsync(Guid id)
        {
            return await _context.Albums.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Result> UpdateAlbum(Guid id, byte[] cover, string? name, string? description)
        {
            AlbumModel? albumModel = await _context.Albums
                .FirstOrDefaultAsync(x => x.Id == id);

            if (albumModel == null)
            {
                return Result.Failure("Album not found");
            }

            albumModel.Update(name, description);

            await File.WriteAllBytesAsync(albumModel.Cover, cover);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}