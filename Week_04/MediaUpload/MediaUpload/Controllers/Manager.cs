using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using MediaUpload.Models;

namespace MediaUpload.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper components
        MapperConfiguration config;
        public IMapper mapper;

        public Manager()
        {
            // If necessary, add your own code here

            // Configure AutoMapper...
            config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Employee, EmployeeBase>();

                // Attention 01 - Infrastructure - AutoMapper create maps

                cfg.CreateMap<Models.Book, Controllers.BookBase>();
                cfg.CreateMap<Models.Book, Controllers.BookWithMediaInfo>();
                cfg.CreateMap<Models.Book, Controllers.BookWithMedia>();

                cfg.CreateMap<Controllers.BookAdd, Models.Book>();
            });

            mapper = config.CreateMapper();


            // Data-handling configuration

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // Add methods below
        // Controllers will call these methods
        // Ensure that the methods accept and deliver ONLY view model objects and collections
        // The collection return type is almost always IEnumerable<T>

        // Suggested naming convention: Entity + task/action
        // For example:
        // ProductGetAll()
        // ProductGetById()
        // ProductAdd()
        // ProductEdit()
        // ProductDelete()




        // ############################################################
        // Book

        public IEnumerable<BookWithMediaInfo> BookGetAll()
        {
            // Call the base class method
            var o = ds.Books.OrderBy(b => b.Title);

            return mapper.Map<IEnumerable<BookWithMediaInfo>>(o);
        }

        // Attention 10 - Notice the difference between these two methods...
        // BookGetById - delivers media item info
        // BookGetByIdWithMedia - ALSO delivers the media item bytes

        public BookWithMediaInfo BookGetById(int id)
        {
            // Attempt to fetch the object
            var o = ds.Books.Find(id);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<BookWithMediaInfo>(o);
        }

        public BookWithMedia BookGetByIdWithMedia(int id)
        {
            // Attempt to fetch the object
            var o = ds.Books.Find(id);

            // Return the result, or null if not found
            return (o == null) ? null : mapper.Map<BookWithMedia>(o);
        }

        public BookWithMediaInfo BookAdd(BookAdd newItem)
        {
            // Attention 11 - Book add - make sure that the book does not exist before attempting to add it
            // Lookup using the ISBN

            // Sanitize the incoming ISBN
            var isbn = newItem.ISBN13.Trim().ToUpper();
            isbn = isbn.Replace(" ", "");
            isbn = isbn.Replace("-", "");

            var existingItem = ds.Books.SingleOrDefault
                (b => b.ISBN13 == isbn);

            if (existingItem != null) { return null; }

            // Attempt to add the new object
            var addedItem = ds.Books.Add(mapper.Map<Book>(newItem));
            // Replace the ISBN with the sanitized version
            addedItem.ISBN13 = isbn;
            ds.SaveChanges();

            // Return the object
            return (addedItem == null) ? null : mapper.Map<BookWithMediaInfo>(addedItem);
        }

        public bool BookSetPhoto(int id, string contentType, byte[] photo)
        {
            // Attention 12 - Notice the return type of this method - bool
            // This is an incremental attempt at improving the command pattern
            // In the controller, we could use the return value, if we wished

            // Ensure that we can continue
            if (string.IsNullOrEmpty(contentType) | photo == null) { return false; }

            // Attempt to find the matching object
            var storedItem = ds.Books.Find(id);

            // Ensure that we can continue
            if (storedItem == null) { return false; }

            // Save the photo
            storedItem.ContentType = contentType;
            storedItem.Photo = photo;

            // Attempt to save changes
            // Do you understand the following? If not, ask
            return (ds.SaveChanges() > 0) ? true : false;
        }


    }
}