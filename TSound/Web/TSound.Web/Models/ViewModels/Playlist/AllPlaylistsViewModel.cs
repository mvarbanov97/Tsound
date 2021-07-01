using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Web.Models.Contracts;
using TSound.Web.Models.ViewModels.Category;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class AllPlaylistsViewModel : IPagingCollection
    {
        public string Url { get; set; }

        public IEnumerable<PlaylistLightViewModel> CollectionPlaylists { get; set; }

        public IEnumerable<PlaylistLightViewModel> CollectionPlaylistsByPage { get; set; }

        public string NameToSearchForFilter { get; set; }

        public int DurationMinHoursFilter { get; set; }

        public int DurationMaxHoursFilter { get; set; }

        public int RankMinFilter { get; set; }

        public int RankMaxFilter { get; set; }

        public List<CategoryFullViewModel> Genres { get; set; }

        public List<Guid> GenresIdChosenByUserFilter { get; set; }

        public SortMethod SortMethod { get; set; }

        public SortOrder SortOrder { get; set; }

        public bool IsAscending { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalCount { get; set; }

        public int FirstPage => 1;

        public int LastPage
        {
            get
            {
                return (int)Math.Ceiling((double)this.TotalCount / this.PageSize);
            }
        }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public bool IsPreviousPageDisabled => this.CurrentPage == 1;

        public bool IsNextPageDisabled
        {
            get
            {
                return CurrentPage == Math.Ceiling((double)this.TotalCount / this.PageSize);
            }
        }
    }

    public enum SortMethod
    {
        Sort = 0,
        Rank = 1,
        Duration = 2,
        Name = 3
    }

    public enum SortOrder
    {
        Order = 0,
        Desc = 1,
        Asc = 2,
    }
}
