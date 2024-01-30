using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Principal;
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
    public DbSet<TaggedClientMessage> TaggedClientMessages { get; set; }

    public TalkBuddyContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {       
        modelBuilder.Entity<Friendship>()
                .HasOne(x => x.Client1)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.Client1Id)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>()
                .HasOne(x => x.InformantClient)
                .WithMany(x => x.InformantClients)
                .HasForeignKey(x => x.InformantClientId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>()
                .HasOne(x => x.ReportedClient)
                .WithMany(x => x.ReportedClients)
                .HasForeignKey(x => x.ReportedClientId)
                .OnDelete(DeleteBehavior.Cascade);

       
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