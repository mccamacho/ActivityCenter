using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using DojoActivity.Models;



namespace DojoActivity.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId {get; set;}
        [Required]
        [MinLength(2,ErrorMessage = "This field require at least 2 Characters")]
        
        public string Title { get; set;}
        [Required]
  
        [DataType(DataType.Date)]
        public DateTime Date { get; set;}

        [Required]
  
        [DataType(DataType.Time)]
        public DateTime Time { get; set;}

        [Required]
        public int Duration { get; set;}

        [Required]
        [MinLength(2,ErrorMessage = "Description must be at least 2 Characters")]
        
        public string Description { get; set;}
        public int Coordinator { get; set;}
        public string CoordinatorName { get; set;}
        public List<UserActivity> JoiningUser{ get; set;}
        public Activity()
        {
            JoiningUser = new List<UserActivity>();
        }

    }
}