using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCMoviesND.Models;
using System.Net;

namespace MVCMoviesND.Controllers
{
    public class HomeController : Controller
    {
        private MoviesNorikaEntities db = new MoviesNorikaEntities();

        // GET: Home
        public ActionResult Index(string movieGenre, string searchString )

        {

            //genre search
            var movies = from m in db.Movies
                         select m;

            //first put genres into the genre dropdown list
            var GenreList = new List<string>();

            //Linq query to retrieve genre names from db to popultate the genre dropdown list
            var GenreQuery = from g in db.Movies
                             orderby g.Genre
                             select g.Genre;

            //removes all duplicates
            GenreList.AddRange(GenreQuery.Distinct());


            //put the list of genre into the view bag to pass it to the index view
            ViewBag.movieGenre = new SelectList(GenreList);

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            //LINQ query to return all movies from the database

            //title search
            if(!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

          
            return View(movies);
        }



        public ActionResult Details(int? id)
        {

            //if the movie id missing
            if(id==null)

            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get the movie
            Movie movie = db.Movies.Find(id);
            // if the movie id wasnt in the db


          //  if the movie id wasnt found in the database
            if(movie==null)

            {
                return HttpNotFound();

            }


            return View(movie);
        }

        [HttpGet]
        public ActionResult Create()
        {

           
            return View();
        }


        [HttpPost]
        public ActionResult Create(Movie movie)
        {

            if(ModelState.IsValid)
            {

                //get the edited data
                db.Movies.Add(movie);


                //save changes to db
                db.SaveChanges();

               // goto back to index action method
                return RedirectToAction("Index");

            }

            //add movie to db using data returned from view
         

           
            return View(movie);
        }




        [HttpGet]
        //to get the dATA FROM USER
        public ActionResult Edit(int? id)
        {
            Movie movie = db.Movies.Find(id);

            return View(movie);
        }



        [HttpPost]
        public ActionResult Edit(Movie movie)

        //[Bind(Include = "Id,Title,ReleaseDate,Genre,Price")] could be placed in above

        {
            //using bind to specify fields to be returned protects from overposting attacks
            //get the edited data

            db.Entry(movie).State = EntityState.Modified;

            //save changes to the DB
            db.SaveChanges();

            //go back to the Index action method
            return RedirectToAction("Index");


        }

        [HttpGet]
        public ActionResult Delete(int? id)

        {

            //if movie id is missing
            if(id==null)

            {
                return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = db.Movies.Find(id);

            if (movie==null)
            {
                return HttpNotFound();
            }

            return View(movie);

        }


        


        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)

        {

            Movie movie=db.Movies.Find(id);
            db.Movies.Remove(movie);

            //save changes to the DB
            db.SaveChanges();

            //go back to the Index action method
            return RedirectToAction("Index");





        }
    }

   

}