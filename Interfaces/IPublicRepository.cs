using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFill_API.Interfaces
{
    public interface IPublicRepository
    {
        IUserRepository GetUserRepository { get; }
    }
}
