using Connection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace Tests.Integration
{
    public class RoomsConnectionTests
    {
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(3);
        private readonly Uri address = new Uri("http://localhost:50557");

        [Fact]
        public async Task ConnectAndDisconnect()
        {
            var connectionTwoUserIdData = new MyData();
            var connectionTwoConnectedUserData = new List<ConnectedUserData>();
            var connectionTwoDisconnectedUserData = new List<DisconnectedUserData>();
            var connectionOne = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var userConnectedHandlerMock = new Mock<IObserver<ConnectedUserData>>();
            userConnectedHandlerMock.Setup(observer => observer.OnNext(It.IsAny<ConnectedUserData>()))
                                    .Callback<ConnectedUserData>(connectionTwoConnectedUserData.Add);
            connectionOne.Subscribe(userConnectedHandlerMock.Object);
            var userDisconnectedHandlerMock = new Mock<IObserver<DisconnectedUserData>>();
            userDisconnectedHandlerMock.Setup(observer => observer.OnNext(It.IsAny<DisconnectedUserData>()))
                                       .Callback<DisconnectedUserData>(connectionTwoDisconnectedUserData.Add);
            connectionOne.Subscribe(userDisconnectedHandlerMock.Object);
            var connectionTwo = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var myIdHandlerMock = new Mock<IObserver<MyData>>();
            myIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                           .Callback<MyData>(data => connectionTwoUserIdData = data)
                           .Verifiable();
            connectionTwo.Subscribe(myIdHandlerMock.Object);
            
            await connectionTwo.KnowAboutMeAsync();
            await Task.Delay(timeout);
            connectionTwo.Dispose();
            await Task.Delay(timeout);

            Mock.Verify(myIdHandlerMock);
            Assert.Contains(connectionTwoConnectedUserData, data => data.UserId == connectionTwoUserIdData.Id);
            Assert.Contains(connectionTwoDisconnectedUserData, data => data.UserId == connectionTwoUserIdData.Id);
        }

        [Fact]
        public async Task CreateAndDelete()
        {
            var ownerId = Guid.Empty;
            var header = "My test room";
            var description = "bla bla bla";
            RoomDescriptionData roomDescriptionData = null;
            var expellingData = new List<RoomExpellingData>();
            var connectionOwner = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var myIdHandlerMock = new Mock<IObserver<MyData>>();
            myIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                           .Callback<MyData>(data => ownerId = data.Id);
            connectionOwner.Subscribe(myIdHandlerMock.Object);
            var connectionClient = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var expellingDataHandlerMock = new Mock<IObserver<RoomExpellingData>>();
            expellingDataHandlerMock.Setup(observer => observer.OnNext(It.IsAny<RoomExpellingData>()))
                                    .Callback<RoomExpellingData>(expellingData.Add);
            connectionClient.Subscribe(expellingDataHandlerMock.Object);

            await connectionOwner.KnowAboutMeAsync();
            await connectionOwner.CreateMyRoomAsync(header);
            await connectionOwner.DescribeMyRoomAsync(description);
            await Task.Delay(timeout);
            var allRooms = await connectionClient.GetAllRoomsAsync();
            var roomData = allRooms.FirstOrDefault(room => room.OwnerId == ownerId);

            if(roomData != null)
            {
                roomDescriptionData = await connectionClient.GetRoomDescriptionAsync(roomData.OwnerId);
            }

            await connectionOwner.DeleteMyRoomAsync();
            await Task.Delay(timeout);
            var allRoomsAfterDeleting = await connectionClient.GetAllRoomsAsync();

            Assert.NotNull(roomDescriptionData);
            Assert.Equal(roomDescriptionData.Header, header);
            Assert.Equal(roomDescriptionData.Description, description);
            Assert.DoesNotContain(allRoomsAfterDeleting, room => room.OwnerId == ownerId);
            Assert.Contains(expellingData, data => data.OwnerId == ownerId);
        }

        [Fact]
        public async Task JoiningAndLeaving()
        {
            var connectionOneUserId = Guid.Empty;
            var connectionTwoUserId = Guid.Empty;
            var joiningData = new List<RoomJoiningData>();
            var joinedData = new List<RoomJoinedData>();
            var rejectedData = new List<RoomRejectedData>();
            var rejectedReason = "Test reason";
            var leavingData = new List<RoomLeavingData>();
            var connectionOne = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var userOneIdHandlerMock = new Mock<IObserver<MyData>>();
            userOneIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                                .Callback<MyData>(data => connectionOneUserId = data.Id);
            connectionOne.Subscribe(userOneIdHandlerMock.Object);
            var joiningHandlerMock = new Mock<IObserver<RoomJoiningData>>();
            joiningHandlerMock.Setup(observer => observer.OnNext(It.IsAny<RoomJoiningData>()))
                              .Callback<RoomJoiningData>(joiningData.Add);
            connectionOne.Subscribe(joiningHandlerMock.Object);
            var leavingHandlerMock = new Mock<IObserver<RoomLeavingData>>();
            leavingHandlerMock.Setup(observer => observer.OnNext(It.IsAny<RoomLeavingData>()))
                              .Callback<RoomLeavingData>(leavingData.Add);
            connectionOne.Subscribe(leavingHandlerMock.Object);
            var connectionTwo = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var userTwoIdHandlerMock = new Mock<IObserver<MyData>>();
            userTwoIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                                .Callback<MyData>(data => connectionTwoUserId = data.Id);
            connectionTwo.Subscribe(userTwoIdHandlerMock.Object);
            var joinedHandlerMock = new Mock<IObserver<RoomJoinedData>>();
            joinedHandlerMock.Setup(observer => observer.OnNext(It.IsAny<RoomJoinedData>()))
                             .Callback<RoomJoinedData>(joinedData.Add);
            connectionTwo.Subscribe(joinedHandlerMock.Object);
            var rejectedHandlerMock = new Mock<IObserver<RoomRejectedData>>();
            rejectedHandlerMock.Setup(observer => observer.OnNext(It.IsAny<RoomRejectedData>()))
                               .Callback<RoomRejectedData>(rejectedData.Add);
            connectionTwo.Subscribe(rejectedHandlerMock.Object);

            await connectionOne.KnowAboutMeAsync();
            await connectionTwo.KnowAboutMeAsync();
            await Task.Delay(timeout);
            await connectionTwo.AskToJoinToHisRoomAsync(connectionOneUserId);
            await connectionOne.JoinTheUserToMyRoomAsync(connectionTwoUserId);
            await connectionOne.DenyToUserToMyRoomJoiningAsync(connectionTwoUserId, rejectedReason);
            await connectionTwo.LeaveThisOwnerRoomAsync(connectionOneUserId);
            await Task.Delay(timeout);

            Assert.Contains(joiningData, data => data.UserId == connectionTwoUserId);
            Assert.Contains(joinedData, data => data.OwnerId == connectionOneUserId);
            Assert.Contains(leavingData, data => data.UserId == connectionTwoUserId);
            Assert.Contains(rejectedData, data => data.OwnerId == connectionOneUserId && data.Message == rejectedReason);
        }

        [Fact]
        public async Task Messaging()
        {
            var connectionOneUserId = Guid.Empty;
            var connectionTwoUserId = Guid.Empty;
            var ipData = new List<UserIPData>();
            var messagingStartingData = new List<MessagingStartingData>();
            var messagingStartedData = new List<MessagingStartedData>();
            var securityKey = Guid.NewGuid();
            var messagingAddress = IPAddress.Loopback;
            var messagingPort = 12345;
            var connectionOne = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var userOneIdHandlerMock = new Mock<IObserver<MyData>>();
            userOneIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                                .Callback<MyData>(data => connectionOneUserId = data.Id);
            connectionOne.Subscribe(userOneIdHandlerMock.Object);
            var messagingStartingHandlerMock = new Mock<IObserver<MessagingStartingData>>();
            messagingStartingHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MessagingStartingData>()))
                                        .Callback<MessagingStartingData>(messagingStartingData.Add);
            connectionOne.Subscribe(messagingStartingHandlerMock.Object);
            var userOneIpDataHandlerMock = new Mock<IObserver<UserIPData>>();
            userOneIpDataHandlerMock.Setup(observer => observer.OnNext(It.IsAny<UserIPData>()))
                                    .Callback<UserIPData>(ipData.Add);
            connectionOne.Subscribe(userOneIpDataHandlerMock.Object);
            var connectionTwo = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = address
            });
            var userTwoIdHandlerMock = new Mock<IObserver<MyData>>();
            userTwoIdHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MyData>()))
                                .Callback<MyData>(data => connectionTwoUserId = data.Id);
            connectionTwo.Subscribe(userTwoIdHandlerMock.Object);
            var messagingStartedHandlerMock = new Mock<IObserver<MessagingStartedData>>();
            messagingStartedHandlerMock.Setup(observer => observer.OnNext(It.IsAny<MessagingStartedData>()))
                                       .Callback<MessagingStartedData>(messagingStartedData.Add);
            connectionTwo.Subscribe(messagingStartedHandlerMock.Object);
            var userTwoIpDataHandlerMock = new Mock<IObserver<UserIPData>>();
            userTwoIpDataHandlerMock.Setup(observer => observer.OnNext(It.IsAny<UserIPData>()))
                                    .Callback<UserIPData>(ipData.Add);
            connectionTwo.Subscribe(userTwoIpDataHandlerMock.Object);

            await connectionOne.KnowAboutMeAsync();
            await connectionTwo.KnowAboutMeAsync();
            await Task.Delay(timeout);
            await connectionTwo.AskToStartMessagingAsync(connectionOneUserId);
            await connectionOne.StartMessagingAsync(connectionTwoUserId, securityKey.ToByteArray());
            await connectionTwo.TellMyIpAsync(connectionOneUserId, messagingAddress, messagingPort);
            await connectionOne.TellMyIpAsync(connectionTwoUserId, messagingAddress, messagingPort);
            await Task.Delay(timeout);

            Assert.Contains(messagingStartingData, data => data.UserId == connectionTwoUserId);
            Assert.Contains(messagingStartedData, data => data.UserId == connectionOneUserId && new Guid(data.SecurityKey) == securityKey);
            Assert.Contains(ipData, data => data.UserId == connectionOneUserId && data.IP.Address.Equals(messagingAddress) && data.IP.Port == messagingPort);
            Assert.Contains(ipData, data => data.UserId == connectionTwoUserId && data.IP.Address.Equals(messagingAddress) && data.IP.Port == messagingPort);
        }
    }
}
