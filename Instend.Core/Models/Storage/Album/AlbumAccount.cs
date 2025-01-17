﻿using System.ComponentModel.DataAnnotations.Schema;
using Instend.Core.Models.Abstraction;

namespace Instend.Core.Models.Storage.Album
{
    [Table("albums_accounts")]
    public class AlbumAccount : AccessBase
    {
        public Album Album { get; set; } = null!;

        private AlbumAccount() { }

        public AlbumAccount(Album album, Configuration.EntityRoles role) : base(role)
        {
            Album = album;
        }
    }
}