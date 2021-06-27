using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.Models;

namespace TSound.Data.Configuration
{
    public class PlaylistCategoryConfiguration : IEntityTypeConfiguration<PlaylistCategory>
    {
        public void Configure(EntityTypeBuilder<PlaylistCategory> builder)
        {
            builder
                .HasKey(playGenre => new { playGenre.PlaylistId, playGenre.CategoryId });

            builder
                .HasOne(playGenre => playGenre.Playlist)
                .WithMany(playlist => playlist.Categories)
                .HasForeignKey(playGenre => playGenre.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(playGenre => playGenre.Category)
                .WithMany(genre => genre.Playlists)
                .HasForeignKey(playGenre => playGenre.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
