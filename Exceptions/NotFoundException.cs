using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base() {}
    
    public NotFoundException(string message) : base(message) {}
}