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
                .HasKey(pg => new { pg.PlaylistId, pg.CategoryId });

            builder
                .HasOne(pg => pg.Playlist)
                .WithMany(playlist => playlist.Categories)
                .HasForeignKey(pg => pg.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pg => pg.Category)
                .WithMany(category => category.Playlists)
                .HasForeignKey(pg => pg.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
