using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSound.Data.Models;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Models;
using TSound.Services.Models.News;
using TSound.Web.Models.ViewModels;
using TSound.Web.Models.ViewModels.Album;
using TSound.Web.Models.ViewModels.Artist;
using TSound.Web.Models.ViewModels.Category;
using TSound.Web.Models.ViewModels.Playlist;
using TSound.Web.Models.ViewModels.Track;
using TSound.Web.Models.ViewModels.User;

namespace TSound.Web.MappingConfiguration
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<Playlist, PlaylistServiceModel>()
                .ForMember(dest => dest.CategoriesCount, opt => opt.MapFrom(src => src.Categories.Count()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.UserName}"))
                .ForMember(dest => dest.UserImageUrl, opt => opt.MapFrom(src => src.User.ImageUrl))
                .ForMember(dest => dest.DurationPlaylist, opt => opt.MapFrom(src => src.Tracks.Sum(x => x.Track.DurationMs)))
                .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Tracks.Count()));

            this.CreateMap<Playlist, PlaylistServiceModel>()
                .ForMember(dest => dest.Rank, opt => opt.Ignore());

            this.CreateMap<PlaylistServiceModel, Playlist>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            this.CreateMap<Category, CategoryServiceModel>();

            this.CreateMap<SpotifyTrack, Track>()
                .ForMember(dest => dest.SpotifyCategoryId, opt => opt.MapFrom(src => src.SpotifyCategoryId))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artists[0]))
                .ForMember(dest => dest.AlbumId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SpotifyId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            this.CreateMap<SpotifyAlbum, Album>()
                .ForMember(dest => dest.Artists, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SpotifyId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            this.CreateMap<SpotifyArtist, Artist>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SpotifyId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            this.CreateMap<User, UserServiceModel>().ReverseMap();

            this.CreateMap<Track, TrackServiceModel>()
                .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.Album.Name))
                .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist.Name))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            this.CreateMap<Artist, ArtistServiceModel>();

            this.CreateMap<CategoryServiceModel, CategoryViewModel>().ReverseMap();
            this.CreateMap<CategoryServiceModel, CategoryFullViewModel>().ReverseMap();

            this.CreateMap<PlaylistServiceModel, PlaylistViewModel>()
                .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Tracks.Count()))
                .ReverseMap();
            this.CreateMap<PlaylistServiceModel, PlaylistLightViewModel>()
                .ForMember(dest => dest.SongsCount, opt => opt.MapFrom(src => src.Tracks.Count()))
                .ForMember(dest => dest.CategoriesCount, opt => opt.MapFrom(src => src.CategoriesCount))
                .ReverseMap();

            this.CreateMap<TrackServiceModel, TrackViewModel>().ReverseMap();
            this.CreateMap<TrackServiceModel, TrackLightViewModel>().ReverseMap();

            this.CreateMap<AlbumServiceModel, AlbumViewModel>().ReverseMap();

            this.CreateMap<ArtistServiceModel, ArtistViewModel>().ReverseMap();

            this.CreateMap<UserServiceModel, UserViewModel>().ReverseMap();

            this.CreateMap<NewsServiceModel, NewsViewModel>()
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name));
        }
    }
}
