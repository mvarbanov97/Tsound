using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.Models;

namespace TSound.Data.Configuration
{
    public class PlaylistSongConfiguration : IEntityTypeConfiguration<PlaylistSong>
    {
        public void Configure(EntityTypeBuilder<PlaylistSong> builder)
        {
            builder
                .HasKey(playSong => new { playSong.PlaylistId, playSong.SongId });

            builder
                .HasOne(playSong => playSong.Playlist)
                .WithMany(playlist => playlist.Songs)
                .HasForeignKey(playSong => playSong.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(playSong => playSong.Song)
                .WithMany(song => song.Playlists)
                .HasForeignKey(playSong => playSong.SongId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
