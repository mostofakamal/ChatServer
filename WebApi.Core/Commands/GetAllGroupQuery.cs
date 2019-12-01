using System.Collections.Generic;
using MediatR;
using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class GetAllGroupQuery : IRequest<IList<GroupDto>>
    {

    }
}