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
                .ForMember(dest => dest.DurationPlaylist, opt => opt.MapFrom(src => src.Songs.Select(x => x.Song.Duration).Sum()))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Songs.Select(x => x.Song.Rank).Average()));

        }
    }
}
