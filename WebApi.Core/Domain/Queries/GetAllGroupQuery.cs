using System.Collections.Generic;
using MediatR;
using WebApi.Core.Dto;

namespace WebApi.Core.Domain.Queries
{
    public class GetAllGroupQuery : IRequest<IList<GroupDto>>
    {

    }
}