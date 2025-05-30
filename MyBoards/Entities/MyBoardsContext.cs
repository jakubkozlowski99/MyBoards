﻿using Microsoft.EntityFrameworkCore;

namespace MyBoards.Entities
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {
            
        }

        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .Property(i => i.Efford)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Epic>()
                .Property(e => e.EndDate)
                .HasPrecision(3);

            modelBuilder.Entity<Task>()
                .Property(t => t.Activity)
                .HasMaxLength(200);

            modelBuilder.Entity<Task>()
                .Property(t => t.RemainingWork)
                .HasPrecision(14, 2);

            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");

                eb.Property(wi => wi.Priority).HasDefaultValue(1);

                eb.HasMany(wi => wi.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

                eb.HasOne(wi => wi.Author)
                .WithMany(a => a.WorkItems)
                .HasForeignKey(wi => wi.AuthorId);

                eb.HasMany(wi => wi.Tags)
                .WithMany(t => t.WorkItems)
                .UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag)
                    .WithMany()
                    .HasForeignKey(wit => wit.TagId),

                    w => w.HasOne(wit => wit.WorkItem)
                    .WithMany()
                    .HasForeignKey(wit => wit.WorkItemId),

                    wit =>
                    {
                        wit.HasKey(x => new {x.TagId, x.WorkItemId});
                        wit.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                    }

                    );

                eb.HasOne(wi => wi.State)
                .WithMany()
                .HasForeignKey(wi => wi.StateId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(c => c.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(c => c.UpdatedDate).ValueGeneratedOnUpdate();
                eb.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Adress)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<WorkItemState>()
                .Property(s => s.Value)
                .IsRequired()
                .HasMaxLength(60);

            modelBuilder.Entity<WorkItemState>()
                .HasData(new WorkItemState() { Id = 1, Value = "To Do" },
                new WorkItemState() { Id = 2, Value = "Doing" },
                new WorkItemState() { Id = 3, Value = "Done" });

            modelBuilder.Entity<Tag>()
                .HasData(new Tag() { Id = 1, Value = "Web" },
                new Tag() { Id = 2, Value = "UI" },
                new Tag() { Id = 3, Value = "Desktop" },
                new Tag() { Id = 4, Value = "API" },
                new Tag() { Id = 5, Value = "Service" });

        }
    }
}
