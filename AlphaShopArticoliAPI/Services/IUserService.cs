using AlphaShopArticoliAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShopArticoliAPI.Services
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);

        Task<Utenti> GetUser(string username);
    }
}
