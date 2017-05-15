using AzureRedisWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureRedisWebApplication.Contracts
{
    public interface IContactManager : IDisposable
    {
        Task<IEnumerable<Contact>> GetAll();

        Task<Contact> Get(Guid id);

        Task Create(Contact contact);

        Task Update(Contact contact);

        Task Delete(Contact contact);
    }
}