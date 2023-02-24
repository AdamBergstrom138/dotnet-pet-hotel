using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        public IEnumerable<PetOwner> GetPets()
        {
            return _context.PetOwners;
        }
        // GET /api/petowners/:id
        [HttpGet("{id}")]
        public ActionResult<PetOwner> GetById(int id)
        {
            PetOwner petOwner = _context.PetOwners
                .SingleOrDefault(petOwner => petOwner.id == id);

            // Return a `404 Not Found` if the petowner doesn't exist
            if (petOwner is null)
            {
                return NotFound();
            }

            return petOwner;
        }
        
        [HttpPost]
        public IActionResult CreatePetOwner(PetOwner petOwner)
        {
            _context.Add(petOwner);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = petOwner.id }, petOwner);
        }

        //UPDDATE /api/petowners/:id
        [HttpPut("{id}")]
        public PetOwner Put(int id, PetOwner petOwner)
        {
            // Our DB context needs to know the id of the petowner to update
            petOwner.id = id;

            // Tell the DB context about our updated petowner object
            _context.Update(petOwner);

            // ...and save the petowner object to the database
            _context.SaveChanges();

            // Respond back with the created petowner object
            return petOwner;
        }

        // DELETE /api/petowners/:id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Find the pet owner, by ID
            PetOwner petOwner = _context.PetOwners.Find(id);

            if (petOwner == null)
            {
                return NotFound(); // return a 404 Not Found if the pet owner doesn't exist
            }

            // Tell the DB that we want to remove this pet owner
            _context.PetOwners.Remove(petOwner);

            // ...and save the changes to the database
            _context.SaveChanges();

            return NoContent(); // return a 204 No Content if the pet owner was successfully deleted
        }
    }
}