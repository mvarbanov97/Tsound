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
                .HasKey(pt => new { pt.PlaylistId, pt.TrackId });
            
            builder
                .HasOne(pt => pt.Playlist)
                .WithMany(playlist => playlist.Tracks)
                .HasForeignKey(pt => pt.PlaylistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pt => pt.Track)
                .WithMany(track => track.Playlists)
                .HasForeignKey(pt => pt.TrackId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
