using AzureRedisWebApplication.Contracts;
using AzureRedisWebApplication.Data;
using AzureRedisWebApplication.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureRedisWebApplication.Managers
{
    public class ContactManager : IContactManager
    {
        public async Task<IEnumerable<Contact>> GetAll()
        {
            // Read Contacts from cache
            IDatabase cache = cacheContext.GetDatabase();

            var contacts = cache
                .HashGetAll(cacheKeyName)
                .Select(entry => JsonConvert.DeserializeObject<Contact>(entry.Value))
                .OrderBy(c => c.Name)
                .ToList();

            // Return the entries found in cache, if any
            // HashGetAll returns an empty list if no entries are found
            if (contacts.Count() > 0)
            {
                return contacts;
            }

            // Nothing found in cache, read from database
            contacts = databaseContext.Contacts
                .OrderBy(c => c.Name)
                .ToList();

            // Store in cache for next use
            if (contacts.Count > 0)
            {
                var entries = contacts
                    .Select(contact => new HashEntry(
                        name: contact.Id.ToString(),
                        value: JsonConvert.SerializeObject(contact)))
                    .ToArray();

                await cache.HashSetAsync(cacheKeyName, entries);
            }

            return contacts;
        }

        public async Task<Contact> Get(Guid id)
        {
            IDatabase cache = cacheContext.GetDatabase();

            var value = cache.HashGet(cacheKeyName, id.ToString());

            // Return the entry found in cache, if any
            // HashGetAsync returns a null RedisValue if no entry is found
            if (!value.IsNull)
            {
                return JsonConvert.DeserializeObject<Contact>(value.ToString());
            }

            // Nothing found in cache, read from database
            Contact contact = databaseContext.Contacts.Find(id);

            // Store in cache for next use
            if (contact != null)
            {
                HashEntry entry = new HashEntry(
                    name: id.ToString(),
                    value: JsonConvert.SerializeObject(contact));
                await cache.HashSetAsync(cacheKeyName, new[] { entry });
            }

            return contact;
        }

        public async Task Create(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            IDatabase cache = cacheContext.GetDatabase();

            // Add to database
            databaseContext.Contacts.Add(contact);
            await databaseContext.SaveChangesAsync();

            // Add to cache
            HashEntry entry = new HashEntry(
                name: contact.Id.ToString(),
                value: JsonConvert.SerializeObject(contact));
            await cache.HashSetAsync(cacheKeyName, new[] { entry });
        }

        public async Task Update(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            IDatabase cache = cacheContext.GetDatabase();

            // Update in database
            databaseContext.Entry(contact).State = EntityState.Modified;
            await databaseContext.SaveChangesAsync();

            // Update in cache
            HashEntry entry = new HashEntry(
                name: contact.Id.ToString(),
                value: JsonConvert.SerializeObject(contact));
            await cache.HashSetAsync(cacheKeyName, new[] { entry });
        }

        public async Task Delete(Contact contact)
        {
            if (contact == null)
            {
                return;
            }

            IDatabase cache = cacheContext.GetDatabase();

            // Delete from database
            databaseContext.Contacts.Remove(contact);
            await databaseContext.SaveChangesAsync();

            // Delete from cache
            await cache.HashDeleteAsync(cacheKeyName, contact.Id.ToString());
        }

        public void Dispose()
        {
            databaseContext.Dispose();
        }

        private DatabaseContext databaseContext = new DatabaseContext();

        private static readonly string cacheConnectionString = ConfigurationManager.AppSettings["CacheConnection"].ToString();
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(cacheConnectionString);
        });
        private static ConnectionMultiplexer cacheContext => lazyConnection.Value;
        private const string cacheKeyName = "Contacts";
    }
}