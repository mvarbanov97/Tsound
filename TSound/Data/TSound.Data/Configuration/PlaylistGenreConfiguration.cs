using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.Models;

namespace TSound.Data.Configuration
{
    public class PlaylistGenreConfiguration : IEntityTypeConfiguration<PlaylistGenre>
    {
        public void Configure(EntityTypeBuilder<PlaylistGenre> builder)
        {
            builder
                .HasKey(playGenre => new { playGenre.PlaylistId, playGenre.GenreId });

            builder
                .HasOne(playGenre => playGenre.Playlist)
                .WithMany(playlist => playlist.Genres)
                .HasForeignKey(playGenre => playGenre.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(playGenre => playGenre.Genre)
                .WithMany(genre => genre.Playlists)
                .HasForeignKey(playGenre => playGenre.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
