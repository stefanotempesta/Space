using SolidMvcSharePoint.Interfaces;
using SolidMvcSharePoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.SharePoint.Client;

namespace SolidMvcSharePoint.Services
{
    public class SharePointContext : IRepositoryContext
    {
        public SharePointContext(string sharePointSiteUrl)
        {
            Context = new ClientContext(sharePointSiteUrl);
        }

        public ClientContext Context { get; protected set; }

        public static string SharePointSiteUrl => ConfigurationManager.AppSettings["SharePointSiteUrl"];

        public T Find<T>(int id) where T : class, new()
        {
            List list = GetList(typeof(T).Name, BaseType.GenericList);
            if (list == null)
            {
                return default(T);
            }

            ListItem item = list.GetItemById(id);
            if (item == null)
            {
                return default(T);
            }

            Context.Load(item);
            Context.ExecuteQuery();

            return MapListItemTo<T>(item);
        }

        public IEnumerable<T> Get<T>() where T : class, new()
        {
            List list = GetList(typeof(T).Name, BaseType.GenericList);
            if (list == null)
            {
                yield break;
            }

            ListItemCollection items = GetListItems(list);
            if (items == null)
            {
                yield break;
            }

            foreach (var item in items)
            {
                yield return MapListItemTo<T>(item);
            }
        }

        private T MapListItemTo<T>(ListItem item) where T : class, new()
        {
            switch (typeof(T).Name)
            {
                case "InstructorLedCourse":
                    return new InstructorLedCourse { Id = item.Id, Title = (string)item["Title"], Description = (string)item["Description"] } as T;

                case "Content":
                    return new Content { Id = item.Id, Name = (string)item["Title"], ContentType = GetMimeType((string)item["FileName"]) } as T;

                default:
                    return default(T);
            }

        }

        private List GetList(string title, BaseType listType)
        {
            Web web = Context.Web;
            Context.Load(web.Lists, lists => lists.Include(list => list.Id, list => list.Title).Where(list => list.BaseType == listType));
            Context.ExecuteQuery();

            if (web.Lists == null)
            {
                return null;
            }

            return web.Lists.GetByTitle(title);
        }

        private ListItemCollection GetListItems(List list)
        {
            CamlQuery query = CamlQuery.CreateAllItemsQuery();
            ListItemCollection items = list.GetItems(query);

            Context.Load(items);
            Context.ExecuteQuery();

            return items;
        }

        private ContentMimeType GetMimeType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return ContentMimeType.Undefined;
            }

            fileName = fileName.ToLower();
            string fileExt = fileName.Substring(fileName.LastIndexOf("."));

            switch (fileExt)
            {
                case ".mp3":
                    return ContentMimeType.Audio;

                case ".mp4":
                    return ContentMimeType.Video;

                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xslx":
                case ".ppt":
                case ".pptx":
                    return ContentMimeType.Document;

                case ".flv":
                case ".swf":
                    return ContentMimeType.Simulation;
                
                default:
                    if (fileName.StartsWith("http://") || fileName.StartsWith("https://"))
                    {
                        return ContentMimeType.URL;
                    }

                    return ContentMimeType.Undefined;
            }
        }

        #region IDisposable
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
