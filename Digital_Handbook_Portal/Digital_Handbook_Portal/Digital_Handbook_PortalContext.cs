using Microsoft.EntityFrameworkCore;

public class Digital_Handbook_PortalContext(DbContextOptions<Digital_Handbook_PortalContext> options) : DbContext(options)
{
    public DbSet<Digital_Handbook_Portal.Models.BragBook> BragBook { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Community> Community { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Policy> Policy { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.User> User { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Resource> Resource { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Quiz> Quiz { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.ActivityLog> ActivityLog { get; set; }
}
