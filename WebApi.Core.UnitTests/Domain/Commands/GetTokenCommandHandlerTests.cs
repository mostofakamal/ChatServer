using System.Threading;
using Moq;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Domain.Queries;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using Xunit;

namespace WebApi.Core.UnitTests.Domain.Commands
{
    public class GetTokenCommandHandlerTests
    {

        [Fact]
        public async void Handle_GivenValidCredentials_ShouldSucceed()
        {
            // arrange
            var mockPlayerRepository = new Mock<IPlayerRepository>();
            mockPlayerRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new Player("Abcd", "Xyz", "124523", "Test"));

            mockPlayerRepository.Setup(repo => repo.CheckPassword(It.IsAny<Player>(), It.IsAny<string>())).ReturnsAsync(true);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            var mockTokenFactory = new Mock<ITokenFactory>();
            var getTokenCommandHandler = new GetTokenCommandHandler(mockPlayerRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // Act
            var response = await getTokenCommandHandler.Handle(new GetTokenCommand()
            {
                UserName = "userName",
                Password = "password",
                RemoteIpAddress = "127.0.0.1"
            }, new CancellationToken());

            // assert
            Assert.NotNull(response);
            Assert.NotNull(response.AccessToken.Token);
        }

        [Fact]
        public async void Handle_GivenIncompleteCredentials_ShouldReturnNull()
        {
            // arrange
            var mockPlayerRepository = new Mock<IPlayerRepository>();
            mockPlayerRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new Player("", "", "", ""));

            mockPlayerRepository.Setup(repo => repo.CheckPassword(It.IsAny<Player>(), It.IsAny<string>())).ReturnsAsync(false);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new AccessToken("", 0));

            var mockTokenFactory = new Mock<ITokenFactory>();


            var getTokenCommandHandler = new GetTokenCommandHandler(mockPlayerRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);
        
            // Act
            var response = await getTokenCommandHandler.Handle(new GetTokenCommand()
            {
                UserName = "userName",
                Password = "password",
                RemoteIpAddress = "127.0.0.1"
            }, new CancellationToken());

            // assert
            Assert.Null(response);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);
        }

    }
}
