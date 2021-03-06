﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomsService.Common.GetAllRooms;
using RoomsService.Common.GetRoom;

namespace RoomsService.Controllers.Rooms
{
    [Authorize]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private IGetRoomStrategy getRoomStrategy;
        private IGetAllRoomsStrategy getAllRoomsStrategy;

        public RoomsController(IGetRoomStrategy getRoomStrategy, IGetAllRoomsStrategy getAllRoomsStrategy)
        {
            this.getRoomStrategy = getRoomStrategy;
            this.getAllRoomsStrategy = getAllRoomsStrategy;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            var rooms = await getAllRoomsStrategy.GetAsync();
            return Ok(rooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomByIdAsync(string roomId)
        {
            var room = await getRoomStrategy.GetByOwnerAsync(roomId);
            return Ok(room);
        }
    }
}