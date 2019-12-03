using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Moq;
using WebApi.Core.Domain.Commands;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using Xunit;

namespace WebApi.Core.UnitTests.Domain.Commands
{
    public class JoinGroupCommandHandlerTests
    {
        [Fact]
        public async void Handle_GroupMaxMemberCountExceeded_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var mockSecurityDataProvider = new Mock<ISecurityDataProvider>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x=>x[It.Is<string>(s => s == "MaxAllowedGroupMemberCount")]).Returns("2");
            mockSecurityDataProvider.Setup(x => x.GetCurrentLoggedInPlayer()).ReturnsAsync(new Player("", "", "", "")
            {
                Id = 1
            });
            mockRepository.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<Group>>())).ReturnsAsync(new Group()
            {
                Id = 1,
                PlayerGroupMaps = new List<PlayerGroupMapping>()
                {
                    new PlayerGroupMapping()
                    {
                        PlayerId = 2,
                        GroupId = 1
                    },
                    new PlayerGroupMapping()
                    {
                        PlayerId = 3,
                        GroupId = 1
                    },
                }
            });

            // Act
            var handler = new JoinGroupCommandHandler(mockRepository.Object, mockSecurityDataProvider.Object,
                mockConfiguration.Object);
            var result = await Assert.ThrowsAsync<InvalidOperationException>(()=> handler.Handle(new JoinGroupCommand(1), new CancellationToken()));

            // Assert
            Assert.Equal($"Max group member limit exceeded. Allowed Limit: 2",result.Message);

        }


        [Fact]
        public async void Handle_PlayerAlreadyJoinedButTryToJoinAgain_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var mockSecurityDataProvider = new Mock<ISecurityDataProvider>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x[It.Is<string>(s => s == "MaxAllowedGroupMemberCount")]).Returns("2");
            mockSecurityDataProvider.Setup(x => x.GetCurrentLoggedInPlayer()).ReturnsAsync(new Player("", "", "", "")
            {
                Id = 1
            });
            mockRepository.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<Group>>())).ReturnsAsync(new Group()
            {
                Id = 1,
                PlayerGroupMaps = new List<PlayerGroupMapping>()
                {
                    new PlayerGroupMapping()
                    {
                        PlayerId = 1,
                        GroupId = 1
                    },
                    new PlayerGroupMapping()
                    {
                        PlayerId = 3,
                        GroupId = 1
                    },
                }
            });

            // Act
            var handler = new JoinGroupCommandHandler(mockRepository.Object, mockSecurityDataProvider.Object,
                mockConfiguration.Object);
            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(new JoinGroupCommand(1), new CancellationToken()));

            // Assert
            Assert.Equal($"Player already joined the group", result.Message);

        }

    }
}