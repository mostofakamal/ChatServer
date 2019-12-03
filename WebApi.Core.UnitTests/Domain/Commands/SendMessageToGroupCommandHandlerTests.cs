using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Moq;
using WebApi.Core.Domain.Commands;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;
using Xunit;

namespace WebApi.Core.UnitTests.Domain.Commands
{
    public class SendMessageToGroupCommandHandlerTests
    {
        [Fact]
        public async void Handle_SendMessageToAGroupWhichThePlayerIsNotAMemberOf_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var mockSecurityDataProvider = new Mock<ISecurityDataProvider>();

            mockSecurityDataProvider.Setup(x => x.GetCurrentLoggedInPlayer()).ReturnsAsync(new Player("", "", "", "")
            {
                Id = 1
            });

            // Act
            var handler = new SendMessageToGroupCommandHandler(mockRepository.Object, mockSecurityDataProvider.Object);
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new SendMessageToGroupCommand(1, "Hello"), new CancellationToken()));

            // Assert
            Assert.Equal($"Can not send message to a group which the player has not joined yet", result.Message);

        }

        [Fact]
        public async void Handle_SendMessageToAGroupWhichThePlayerIsAMemberOf_ShouldAddToRepo()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var mockSecurityDataProvider = new Mock<ISecurityDataProvider>();

            mockSecurityDataProvider.Setup(x => x.GetCurrentLoggedInPlayer()).ReturnsAsync(new Player("", "", "", "")
            {
                Id = 1
            });
            mockRepository.Setup(x => x.GetSingleBySpec(It.IsAny<GetGroupMemberShipSpecification>())).ReturnsAsync(
                new PlayerGroupMapping()
                {
                    PlayerId = 1,
                    GroupId = 1
                });

            // Act
            var handler = new SendMessageToGroupCommandHandler(mockRepository.Object, mockSecurityDataProvider.Object);
            var result = await handler.Handle(new SendMessageToGroupCommand(1, "Hello"), new CancellationToken());

            // Assert
            mockRepository.Verify(x => x.Add(It.IsAny<MessageHistory>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(1, result.PlayerId);
            Assert.Equal(1, result.GroupId);
        }
    }
}
