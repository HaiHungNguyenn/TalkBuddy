using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Principal;
using TalkBuddy.DAL.Interfaces;
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

    public TalkBuddyContext(DbContextOptions<TalkBuddyContext> options) : base(options)
    {
        
    }
    public TalkBuddyContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Client1)
            .WithMany(u => u.Friends)
            .HasForeignKey(f => f.Client1Id)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.Client2)
            .WithMany()
            .HasForeignKey(f => f.Client2Id);
        

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