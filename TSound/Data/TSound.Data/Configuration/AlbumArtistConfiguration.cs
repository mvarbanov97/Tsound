using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.Models;

namespace TSound.Data.Configuration
{
    public class AlbumArtistConfiguration : IEntityTypeConfiguration<AlbumArtist>
    {
        public void Configure(EntityTypeBuilder<AlbumArtist> builder)
        {
            builder
                .HasKey(aa => new { aa.AlbumId, aa.ArtistId });

            builder
                .HasOne(aa => aa.Album)
                .WithMany(a => a.Artists)
                .HasForeignKey(aa => aa.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(aa => aa.Artist)
                .WithMany(a => a.Albums)
                .HasForeignKey(aa => aa.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
