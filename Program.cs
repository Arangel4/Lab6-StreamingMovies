using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lab_6
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Deletes and Creates the Database.
            using (var db = new Lab_6Context())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            //  Adds studio 20th Century Fox with the following movies.
            using (var db = new Lab_6Context())
            {
                Studio studio = new Studio
                {
                    Name = "20th Century Fox",
                    Movies = new List<Movie>
                    {
                        new Movie {Title = "Avatar", Genre = "Action"},
                        new Movie {Title = "Deadpool", Genre = "Action"},
                        new Movie {Title = "Apollo 13", Genre = "Drama"},
                        new Movie {Title = "The Martian", Genre = "Sci-Fi"}
                    }
                };

                //  Adds the studio Universal Pictures.
                Studio studio2 = new Studio
                {
                    Name = "Universal Pictures"
                };

                db.Add(studio);
                db.Add(studio2);
                db.SaveChanges();
            }

            //  Adds Jurassic Park to the Universal Pictures studio.
            using (var db = new Lab_6Context())
            {
                Movie movie = new Movie {Title = "Jurassic Park", Genre = "Action"};
                Studio studioToUpdate = db.Studios.Include(s => s.Movies).Where(s => s.Name == "Universal Pictures").First();
                studioToUpdate.Movies.Add(movie);
                db.SaveChanges();   
            }

            //  Moves the movie Apollo 13 from 20th Century Fox to Universal Pictures studio.
            using (var db = new Lab_6Context())
            {
                Movie movie = db.Movies.Where(m => m.Title == "Apollo 13").First();
                movie.Studio = db.Studios.Where(s => s.Name == "Universal Pictures").First();
                db.SaveChanges();
            }

            //  Removies the movie Deadpool.
            using (var db = new Lab_6Context())
            {
                Movie movieToRemove = db.Movies.Where(m => m.Title == "Deadpool").First();
                db.Remove(movieToRemove);
                db.SaveChanges();
            }

            //  Lists all studios and their movies.
            using (var db = new Lab_6Context())
            {
                var studios = db.Studios.Include(s => s.Movies);
                foreach (var s in studios)
                {
                    Console.WriteLine(s);
                    foreach (var m in s.Movies)
                    {
                        Console.WriteLine("\t" + m);
                    }
                }
            }
        }
    }
}
