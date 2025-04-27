using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Extensions
{
    public static class StateExtension
    {
        public static string OperationType(this EntityState state)
        {
            return state switch
            {
                EntityState.Added => "Insert",
                EntityState.Deleted => "Delete",
                EntityState.Modified => "Update",
                _ => string.Empty
            };
        }
    }
}
