using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace pet_hotel
{
    public enum PetBreedType {
        Shepherd,
        Poodle,
        Beagle,
        Bulldog,
        Terrier,
        Boxer,
        Labrador,
        Retriever,
        Rottweiler,
        Hamster
    }
    // public enum PetColorType {
    //     White,
    //     Black,
    //     Golden,
    //     Tricolor,
    //     Spotted
    // }
    public class Pet {
        public int id {get; set;}
        public string name {get; set;}

        public string color {get; set;}
        
        public bool checkin {get; set;}

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetBreedType type {get; set;}

        // [JsonConverter(typeof(JsonStringEnumConverter))]

        // public PetColorType type2 {get; set;}


        [ForeignKey("petOwner")]
        public int petOwnerById { get; set; }
        public PetOwner petOwner { get; set; }
    }
}
