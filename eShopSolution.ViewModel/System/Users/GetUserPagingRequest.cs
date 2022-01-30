using eShopSolution.ViewModel.CommonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.System.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}