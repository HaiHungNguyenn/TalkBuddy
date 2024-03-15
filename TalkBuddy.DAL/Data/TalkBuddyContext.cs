using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.DAL.Data;

public partial class TalkBuddyContext : DbContext
{
    public DbSet<ChatBox> ChatBoxes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientChatBox> ClientChatBoxes { get; set; }
    public DbSet<ClientMessage> ClientMessages { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }

    public TalkBuddyContext()
    {
    }
    public TalkBuddyContext(DbContextOptions<TalkBuddyContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseSqlServer(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build() .GetConnectionString("DefaultConnection"));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Sender)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.SenderID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(m=>m.Sender)
            .WithMany(s=>s.Messages)
            .HasForeignKey(f => f.SenderId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Client>()
           .HasMany(c=>c.Messages)
           .WithOne(m => m.Sender)
           .HasForeignKey(m=>m.SenderId)
           .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Receiver)
            .WithMany()
            .HasForeignKey(f => f.ReceiverId);

        modelBuilder.Entity<Report>()
                .HasOne(x => x.InformantClient)
                .WithMany(x => x.InformantClients)
                .HasForeignKey(x => x.InformantClientId)
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Report>()
                .HasOne(x => x.ReportedClient)
                .WithMany(x => x.ReportedClients)
                .HasForeignKey(x => x.ReportedClientId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>()
                .HasOne(x => x.ReportedClient)
                .WithMany(x => x.ReportedClients)
                .HasForeignKey(x => x.ReportedClientId)
                .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ReportEvidence>()
                .HasOne(x => x.Report)
                .WithMany(x => x.ReportEvidences)
                .HasForeignKey(x => x.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClientChatBox>()
                .HasOne(x => x.ChatBox)
                .WithMany(x => x.ClientChatBoxes)
                .HasForeignKey(x => x.ChatBoxId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClientChatBox>()
                .HasOne(x => x.Client)
                .WithMany(x => x.InChatboxes)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ClientMessage>()
                .HasOne(x => x.Client)
                .WithMany(x => x.ClientMessages)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Notification>()
            .ToTable("Notification")
            .HasOne(x => x.Client)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OtpCode>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Codes)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        UpdateVersionForModifiedEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateVersionForModifiedEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateVersionForModifiedEntities()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }
    }
}