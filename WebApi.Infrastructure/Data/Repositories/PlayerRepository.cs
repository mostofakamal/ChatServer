using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Specifications;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Data.Repositories
{
    internal sealed class PlayerRepository : EfRepository<Player>, IPlayerRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        

        public PlayerRepository(UserManager<AppUser> userManager, IMapper mapper, GameDbContext appDbContext): base(appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<int> Create(string firstName, string lastName, string email, string userName, string password)
        {
            var appUser = new AppUser {Email = email, UserName = userName};
            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded) return default(int);
          
            var user = new Player(firstName, lastName, appUser.Id, appUser.UserName);
            GameDbContext.Players.Add(user);
            await GameDbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task<Player> FindByName(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return appUser == null ? null : _mapper.Map(appUser, await GetSingleBySpec(new PlayerSearchSpecification(appUser.Id)));
        }

        public async Task<bool> CheckPassword(Player user, string password)
        {
            return await _userManager.CheckPasswordAsync(_mapper.Map<AppUser>(user), password);
        }
    }
}
