﻿using CSharpFunctionalExtensions;
using Exider.Core;
using Exider.Core.Dependencies.Repositories.Storage;
using Exider.Core.Models.Messenger;
using Exider.Core.TransferModels;
using Exider.Core.TransferModels.Account;
using Exider.Services.External.FileService;
using Microsoft.EntityFrameworkCore;

namespace Exider.Repositories.Messenger
{
    public class MessengerReposiroty : IMessengerReposiroty
    {
        private readonly DatabaseContext _context = null!;

        private readonly IAttachmentsRepository<MessageAttachmentLink> _attachmentsRepository = null!;

        public MessengerReposiroty(DatabaseContext context, IAttachmentsRepository<MessageAttachmentLink> attachmentsRepository)
        {
            _context = context;
            _attachmentsRepository = attachmentsRepository;
        }

        public async Task<DirectTransferModel[]> GetDirects(IFileService fileService, Guid userId, int count)
        {
            DirectTransferModel[] directs = await _context.Directs
                .Where(direct => direct.UserId == userId || direct.OwnerId == userId)
                .Skip(count)
                .Take(1)
                .Join(_context.Users,
                    direct => (direct.OwnerId == userId ? direct.UserId : direct.OwnerId),
                    user => user.Id,
                    (direct, user) => new
                    {
                        direct,
                        user
                    })
                .Join(_context.UserData,
                    prev => prev.user.Id,
                    userData => userData.UserId,
                    (prev, userData) => new
                    {
                        prev.direct,
                        prev.user,
                        userData
                    })
                .GroupJoin(_context.DirectLinks,
                    prev => prev.direct.Id,
                    link => link.ItemId,
                    (prev, links) => new
                    {
                        prev,
                        link = links.OrderByDescending(l => l.Date).FirstOrDefault()
                    })
                .Join(_context.Messages,
                    prev => prev.link.LinkedItemId,
                    message => message.Id,
                    (prev, message) =>
                        new DirectTransferModel
                        (
                            prev.prev.direct,
                            message,
                            new UserPublic()
                            {
                                Id = prev.prev.user.Id,
                                Name = prev.prev.user.Name,
                                Surname = prev.prev.user.Surname,
                                Nickname = prev.prev.user.Nickname,
                                Email = prev.prev.user.Email,
                                Avatar = prev.prev.userData.Avatar,
                                Header = prev.prev.userData.Header,
                                Description = prev.prev.userData.Description,
                                StorageSpace = prev.prev.userData.StorageSpace,
                                OccupiedSpace = prev.prev.userData.OccupiedSpace,
                                Balance = prev.prev.userData.Balance,
                                FriendCount = prev.prev.userData.FriendCount
                            }
                        ))
                .ToArrayAsync();

            foreach (var direct in directs)
            {
                if (direct.userPublic.Avatar == null)
                {
                    direct.userPublic.Avatar = Configuration.DefaultAvatar;
                }
                else
                {
                    var avatar = await fileService.ReadFileAsync(direct.userPublic.Avatar);

                    if (avatar.IsSuccess)
                    {
                        direct.userPublic.Avatar = Convert.ToBase64String(avatar.Value);
                    }
                    else
                    {
                        direct.userPublic.Avatar = Configuration.DefaultAvatar;
                    }
                }

                if (direct.messageModel != null)
                {
                    direct.messageModel.attachments = await _attachmentsRepository
                        .GetItemAttachmentsAsync(direct.messageModel.Id);
                }
            }

            return directs;
        }

        public async Task<DirectTransferModel?> GetDirect(IFileService fileService, Guid id, Guid userId)
        {
            DirectTransferModel? model = await _context.Directs
                .Where(direct => (direct.UserId == userId || direct.OwnerId == userId) && direct.Id == id)
                .Join(_context.Users,
                    direct => (direct.OwnerId == userId ? direct.UserId : direct.OwnerId),
                    user => user.Id,
                    (direct, user) => new
                    {
                        direct,
                        user
                    })
                .Join(_context.UserData,
                    prev => prev.user.Id,
                    userData => userData.UserId,
                    (prev, userData) => new
                    {
                        prev.direct,
                        prev.user,
                        userData
                    })
                .GroupJoin(_context.DirectLinks,
                    prev => prev.direct.Id,
                    link => link.ItemId,
                    (prev, links) => new
                    {
                        prev,
                        link = links.OrderByDescending(l => l.Date).FirstOrDefault()
                    })
                .Join(_context.Messages,
                    prev => prev.link.LinkedItemId,
                    message => message.Id,
                    (prev, message) =>
                        new DirectTransferModel
                        (
                            prev.prev.direct,
                            message,
                            new UserPublic()
                            {
                                Id = prev.prev.user.Id,
                                Name = prev.prev.user.Name,
                                Surname = prev.prev.user.Surname,
                                Nickname = prev.prev.user.Nickname,
                                Email = prev.prev.user.Email,
                                Avatar = prev.prev.userData.Avatar,
                                Header = prev.prev.userData.Header,
                                Description = prev.prev.userData.Description,
                                StorageSpace = prev.prev.userData.StorageSpace,
                                OccupiedSpace = prev.prev.userData.OccupiedSpace,
                                Balance = prev.prev.userData.Balance,
                                FriendCount = prev.prev.userData.FriendCount
                            }
                        ))
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            if (model.userPublic.Avatar == null)
            {
                model.userPublic.Avatar = Configuration.DefaultAvatar;
            }
            else
            {
                var avatar = await fileService.ReadFileAsync(model.userPublic.Avatar);

                if (avatar.IsSuccess)
                {
                    model.userPublic.Avatar = Convert
                        .ToBase64String(avatar.Value);
                }
                else
                {
                    model.userPublic.Avatar = Configuration
                        .DefaultAvatar;
                }
            }

            if (model.messageModel != null)
            {
                model.messageModel.attachments = await _attachmentsRepository
                    .GetItemAttachmentsAsync(model.messageModel.Id);
            }

            return model;
        }

        public async Task<Result<bool>> ChangeAcceptState(Guid directId, Guid userId, bool isAccept)
        {
            int result = 0;

            if (isAccept)
            {
                result = await _context.Directs.AsNoTracking()
                    .Where(direct => direct.Id == directId && direct.UserId == userId)
                    .ExecuteUpdateAsync(direct => direct.SetProperty(d => d.IsAccepted, isAccept));
            }
            else
            {
                result = await _context.Directs.AsNoTracking()
                    .Where(direct => direct.Id == directId && direct.UserId == userId)
                    .ExecuteDeleteAsync();
            }

            if (result <= 0)
            {
                return Result.Failure<bool>("Chat not found");
            }

            return Result.Success(isAccept);
        }

        public async Task<Result<Guid>> DeleteMessage(Guid id, Guid userId)
        {
            return await _context.Database.CreateExecutionStrategy().ExecuteAsync(async Task<Result<Guid>> () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var messagesToDelete = await _context.Messages.AsNoTracking()
                            .Where(x => x.Id == id && x.UserId == userId)
                            .ToListAsync();

                        _context.Messages.RemoveRange(messagesToDelete);
                        await _context.SaveChangesAsync();

                        var linksToDelete = await _context.DirectLinks.AsNoTracking()
                            .Where(link => link.LinkedItemId == id)
                            .FirstOrDefaultAsync();

                        if (linksToDelete == null)
                        {
                            return Result.Failure<Guid>("Not found");
                        }

                        _context.DirectLinks.Remove(linksToDelete);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                        return Result.Success(linksToDelete.ItemId);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return Result.Failure<Guid>("Not found");
                    }
                }
            });
        }

        /// <summary>
        /// Возможны проблемы с безопасностью
        /// когда можно прикрепить сообщение зная только id, но как будто срал я на эту безопасность =)
        /// </summary>
        /// <returns>Результат операции</returns>
        public async Task<bool> ChangePinnedState(Guid messageId, bool pinnedState)
        {
            int result = await _context.Messages
                .Where(x => x.Id == messageId)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsPinned, pinnedState));

            return result != 0;
        }

        public async Task<bool> ViewMessage(Guid messageId, Guid userId)
        {
            var result = await _context.Messages
                .Where(x => x.Id == messageId && x.UserId != userId)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsViewed, true));

            return result != 0;
        }
    }
}