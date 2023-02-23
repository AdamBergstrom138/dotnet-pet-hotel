using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context) {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        // [HttpGet]
        // public IEnumerable<Pet> GetPets() {
        //     return new List<Pet>();
        // }
        [HttpGet]
        public IEnumerable<Pet> GetPets() 
        {
            return _context.Pets
                // Include the `bakedBy` property
                // which is a list of `Baker` objects
                // .NET will do a JOIN for us!
                // .Include(pet => pet.petOwnerById);
                .Include(pet => pet.petOwner);
        }
         [HttpGet("{id}")]
        public ActionResult<Pet> GetById(int id) {
            Pet pet =  _context.Pets
                .Include(pet => pet.petOwner)
                .SingleOrDefault(pet => pet.id == id);
            
            if(pet is null) {
                return NotFound();
            }

            return pet;
        }
                [HttpPost]
        public Pet Post(Pet pet) 
        {
            // Tell the DB context about our new bread object
            _context.Add(pet);
            // ...and save the bread object to the database
            _context.SaveChanges();

            // Respond back with the created bread object
            return pet;
        }

                [HttpPut("{id}")]
        public Pet Put(int id, Pet pet) 
        {
            // Our DB context needs to know the id of the bread to update
            pet.id = id;

            // Tell the DB context about our updated bread object
            _context.Update(pet);

            // ...and save the bread object to the database
            _context.SaveChanges();

            // Respond back with the created bread object
            return pet;
        }
                [HttpDelete("{id}")]
        public void Delete(int id) 
        {
            // Find the bread, by ID
            Pet pet = _context.Pets.Find(id);

            // Tell the DB that we want to remove this bread
            _context.Pets.Remove(pet);

            // ...and save the changes to the database
            _context.SaveChanges();;
        }


        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<Pet> GetPets() {
        //     PetOwner blaine = new PetOwner{
        //         name = "Blaine"
        //     };

        //     Pet newPet1 = new Pet {
        //         name = "Big Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     Pet newPet2 = new Pet {
        //         name = "Little Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<Pet>{ newPet1, newPet2};
        // }
    }
}
