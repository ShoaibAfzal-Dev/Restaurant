﻿namespace MVC.Models
{
    public class User_Details
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Day { get; set; } = null!;
        public string time { get; set; } = null!;
        public bool Cash { get; set; }
        public bool Card { get; set; }
    }
}
