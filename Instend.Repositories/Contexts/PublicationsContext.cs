﻿using Instend.Core.Models.Public;
using Instend.Core.Models.Publication;
using Microsoft.EntityFrameworkCore;

namespace Instend.Repositories.Contexts
{
    public abstract class PublicationsContext : MessagesContext
    {
        public DbSet<Publication> Publications { get; set; } = null!;
        public DbSet<PublicationAttachment> PublicationAttachments { get; set; } = null!;
        public DbSet<PublicationReaction> PublicationReactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Publication>()
                .HasMany(p => p.Attachments)
                .WithMany()
                .UsingEntity<PublicationAttachment>();

            modelBuilder
                .Entity<PublicationReaction>()
                .HasOne(pr => pr.Reaction)
                .WithMany();

            modelBuilder
                .Entity<Publication>()
                .HasMany(pr => pr.Comments)
                .WithOne();

            modelBuilder
                .Entity<PublicationReaction>()
                .HasOne(pr => pr.Publication)
                .WithMany(p => p.Reactions);

            modelBuilder
                .Entity<PublicationReaction>()
                .HasOne(pr => pr.Account)
                .WithMany();

            modelBuilder
                .Entity<PublicationReaction>()
                .HasOne(pr => pr.Reaction)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Core.Models.Account.Account>()
                .HasMany(x => x.Publications)
                .WithOne(x => x.Account)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}