using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Rooms
{
    //сохраняю в Redis. Достаю все по требованию в handler и отправляю. 
    //Timeouts удаление надеюсь можно в Redis настроить, чтобы без timers обойтись.
    //Запрос на add (action = room) и на get (action = rooms) в одном hadler'е.
    interface IRooms 
    {
        Task AddAsync(Room room);

        Task<IEnumerable<Room>> GetAllAsync();
    }
}
