using System;

namespace Draughts.Server.Models
{
    public class ListGameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}