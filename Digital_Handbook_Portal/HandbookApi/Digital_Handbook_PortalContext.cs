using Microsoft.EntityFrameworkCore;

public class Digital_Handbook_PortalContext(DbContextOptions<Digital_Handbook_PortalContext> options) : DbContext(options)
{
    public DbSet<Digital_Handbook_Portal.Models.BragBook> BragBook { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Community> Community { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Policy> Policy { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.PolicyCategory> PolicyCategory { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.User> User { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Resource> Resource { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Quiz> Quiz { get; set; } = default!;
    public DbSet<Digital_Handbook_Portal.Models.Doctor> Doctor { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Digital_Handbook_Portal.Models.BragBook>(entity =>
        {
            entity.ToTable("BragBook");
            entity.HasKey(e => e.bragId);

            entity.Property(e => e.bragId).HasColumnName("bragId");
            entity.Property(e => e.content).HasColumnName("content");
            entity.Property(e => e.senderType).HasColumnName("senderType");
            entity.Property(e => e.recipientName).HasColumnName("recipientName");
            entity.Property(e => e.imageUrl).HasColumnName("imageUrl");
            entity.Property(e => e.datePosted).HasColumnName("datePosted");
        });

        modelBuilder.Entity<Digital_Handbook_Portal.Models.Doctor>(entity =>
        {
            entity.ToTable("Doctor");
            entity.HasKey(e => e.doctorId);

            entity.Property(e => e.doctorId).HasColumnName("doctorId");
            entity.Property(e => e.doctorImg).HasColumnName("doctorImg");
            entity.Property(e => e.fName).HasColumnName("fName");
            entity.Property(e => e.lName).HasColumnName("lName");
            entity.Property(e => e.email).HasColumnName("email");
            entity.Property(e => e.phone).HasColumnName("phone");
            entity.Property(e => e.suiteNumber).HasColumnName("suiteNumber");

            entity.Ignore(e => e.FullName);
        });

        modelBuilder.Entity<Digital_Handbook_Portal.Models.Policy>().ToTable("Policy");
        modelBuilder.Entity<Digital_Handbook_Portal.Models.PolicyCategory>().ToTable("PolicyCategory");
        modelBuilder.Entity<Digital_Handbook_Portal.Models.Community>().ToTable("Community");
        modelBuilder.Entity<Digital_Handbook_Portal.Models.User>().ToTable("User");
        modelBuilder.Entity<Digital_Handbook_Portal.Models.Resource>().ToTable("Resource");
        modelBuilder.Entity<Digital_Handbook_Portal.Models.Quiz>().ToTable("Quiz");
    }
}