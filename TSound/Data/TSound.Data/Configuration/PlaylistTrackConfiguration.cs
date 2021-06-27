using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.Models;

namespace TSound.Data.Configuration
{
    public class PlaylistTrackConfiguration : IEntityTypeConfiguration<PlaylistTrack>
    {
        public void Configure(EntityTypeBuilder<PlaylistTrack> builder)
        {
            builder
                .HasKey(playSong => new { playSong.PlaylistId, playSong.TrackId });
            
            builder
                .HasOne(playSong => playSong.Playlist)
                .WithMany(playlist => playlist.Tracks)
                .HasForeignKey(playSong => playSong.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(playSong => playSong.Track)
                .WithMany(song => song.Playlists)
                .HasForeignKey(playSong => playSong.TrackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
