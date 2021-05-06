using System;

namespace Draughts.Shared.Models
{
    public record Game(Guid Id, string Name, bool IsPublic);
}