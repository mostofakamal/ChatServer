using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Interfaces.Repositories;

namespace WebApi.Core.Commands
{
    public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, CreatePlayerResponse>
    {
        private readonly IPlayerRepository _repository;
        public CreatePlayerCommandHandler(IPlayerRepository repository)
        {
            _repository = repository;
        }
        public async Task<CreatePlayerResponse> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
        {
            var response = await _repository.Create(request.FirstName, request.LastName, request.Email, request.UserName, request.Password);
            return new CreatePlayerResponse
            {
                Id = response
            };
        }
    }
}