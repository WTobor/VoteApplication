﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VoteApplication.Repositories.Models;

namespace VoteApplication.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AddDefaultCandidates(modelBuilder);
            //this solution should work on sql database, but is not working on in-memory database
            modelBuilder.Entity<Vote>().HasIndex(v => v.UserNickname).IsUnique();
            modelBuilder.Entity<Vote>().HasOne(p => p.Candidate).WithMany(c => c.Votes).IsRequired();
            base.OnModelCreating(modelBuilder);
        }

        public void AddDefaultCandidates(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>().HasData(new List<Candidate>
            {
                new Candidate
                {
                    Id = 1,
                    Name = "Jan",
                    Surname = "Kowalski"
                },
                new Candidate
                {
                    Id = 2,
                    Name = "Anna",
                    Surname = "Nowak"
                },
                new Candidate
                {
                    Id = 3,
                    Name = "Michał",
                    Surname = "Malinowski"
                }
            });
        }
    }
}