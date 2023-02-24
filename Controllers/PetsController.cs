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
        public PetsController(ApplicationContext context)
        {
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
                // Include the `petOwner` property
                // which is a list of `Baker` objects
                // .NET will do a JOIN for us!
                // .Include(pet => pet.petOwnerById);
                .Include(pet => pet.petOwner);
        }
        [HttpGet("{id}")]
        public ActionResult<Pet> GetById(int id)
        {
            Pet pet = _context.Pets
                .Include(pet => pet.petOwner)
                .SingleOrDefault(pet => pet.id == id);

            if (pet is null)
            {
                return NotFound();
            }

            return pet;
        }
        [HttpPost]
        public IActionResult CreatePet(Pet pet)
        {
            _context.Add(pet);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = pet.id }, pet);
        }

        //UPDATE /api/pets/:id
        [HttpPut("{id}")]
        public Pet Put(int id, Pet pet)
        {
            // Our DB context needs to know the id of the pet to update
            pet.id = id;

            // Tell the DB context about our updated pet object
            _context.Update(pet);

            // ...and save the bread pet to the database
            _context.SaveChanges();

            // Respond back with the created pet object
            return pet;
        }

        [HttpPut("{id}/checkin")]
        public IActionResult CheckIn(int id)
        {
            // Find the pet by ID
            Pet pet = _context.Pets.Find(id);

            if (pet == null)
            {
                return NotFound(); // return a 404 Not Found if the pet doesn't exist
            }

            // Update the pet's check-in status to true
            pet.checkedInAt = DateTime.Now;

            // Save the changes to the database
            _context.SaveChanges();

            // Return the updated pet object with the name property
            Pet updatedPet = _context.Pets.Find(id);

            if (updatedPet == null)
            {
                return NotFound(); // return a 404 Not Found if the pet doesn't exist
            }

            return Ok(updatedPet); // return the updated pet object with a 200 OK status code
        }


        [HttpPut("{id}/checkout")]
        public IActionResult CheckOut(int id)
        {
            // Find the pet by ID
            Pet pet = _context.Pets.Find(id);

            if (pet == null)
            {
                return NotFound(); // return a 404 Not Found if the pet doesn't exist
            }

            // Update the pet's check-in status to false and set the checked-in-at property to null to indicate that the pet has been checked out
            pet.checkedInAt = null;

            // Save the changes to the database
            _context.SaveChanges();

            Pet updatedPet = _context.Pets.Find(id);

            if (updatedPet == null)
            {
                return NotFound(); // return a 404 Not Found if the pet doesn't exist
            }

            return Ok(updatedPet); // return a 200 OK status code to indicate success
        }







        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Find the pet, by ID
            Pet pet = _context.Pets.Find(id);

            if (pet == null)
            {
                return NotFound(); // return a 404 Not Found if the pet doesn't exist
            }

            // Tell the DB that we want to remove this pet
            _context.Pets.Remove(pet);

            // ...and save the changes to the database
            _context.SaveChanges();

            return NoContent(); // return a 204 No Content if the pet was successfully deleted
        }

    }
}