using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSound.Data.Models;

namespace TSound.Services.Models.MappingConfiguration
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<Playlist, PlaylistServiceModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => src.User.ImageUrl))
                .ForMember(dest => dest.DurationPlaylist, opt => opt.MapFrom(src => src.Songs.Sum(x => x.Song.Duration)))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => (int)src.Songs.Select(playlistSong => playlistSong.Song).Average(song => song.Duration)))
                .ReverseMap();

            this.CreateMap<Genre, GenreServiceModel>();

            this.CreateMap<User, UserServiceModel>().ReverseMap();

            this.CreateMap<Song, SongServiceModel>()
                .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.Album.Name))
                .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist.Name))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ReverseMap();
        }
    }
}
