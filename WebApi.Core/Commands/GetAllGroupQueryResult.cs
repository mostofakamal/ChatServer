using System.Collections.Generic;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class GetAllGroupQueryResult
    {
        public GetAllGroupQueryResult(IList<GroupDto> groups)
        {
            Groups = groups;
        }

        public IList<GroupDto> Groups { get;  }

    }


}