using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DojoActivity.Models
{
    public class UserActivity
    {
        [Key]
        public int UserActivityId {get; set;}
        public int UserId {get; set;}
        public User JoiningUser{get; set;}
        public int ActivityId {get; set;}
        public Activity JoiningActivity{get; set;}
    }
}