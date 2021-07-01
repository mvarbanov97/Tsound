using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.Contracts
{
    public interface IPagingCollection
    {
        string Url { get; set; }

        int PageSize { get; set; }

        int CurrentPage { get; set; }

        int TotalCount { get; set; }

        int FirstPage => 1;

        int LastPage { get; }

        int PreviousPage => this.CurrentPage - 1;

        int NextPage => this.CurrentPage + 1;

        bool IsPreviousPageDisabled => this.CurrentPage == 1;

        bool IsNextPageDisabled { get; }
    }
}
