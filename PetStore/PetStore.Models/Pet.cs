using System;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Pet
    {

        public Pet()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

    }
}
