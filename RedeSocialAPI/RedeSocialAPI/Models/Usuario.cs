﻿namespace RedeSocialAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int IsAdmin { get; set; } = 0;

        
    }
}