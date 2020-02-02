using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Class.Data
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Int64 PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public bool? Confirmed { get; set; }
    }
    public class CandidateContextFactory : IDesignTimeDbContextFactory<CandidateContext>
    {
        public CandidateContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}Class.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();
            return new CandidateContext(config.GetConnectionString("ConStr"));
        }
    }
    public class CandidateContext : DbContext
    {
        private string _connectionString;
        public CandidateContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<Candidate> Candidates { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(_connectionString);
        }
    }
    public class CandidateRepository
    {
        private string _connectionString;
        public CandidateRepository(string connectionString)
        {
            _connectionString = connectionString;

        }
        public void AddCandidate(Candidate c)
        {
            using (var context = new CandidateContext(_connectionString))
            {
                context.Candidates.Add(c);
                context.SaveChanges();
            }

        }
        public IEnumerable<Candidate> GetPending()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Where(i => i.Confirmed == null).ToList();
            }
        }
        public int GetPendingCount()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Where(i => i.Confirmed == null).ToList().Count();
            }
        }
        public Candidate GetCandidate(int id)
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.FirstOrDefault(i => i.Id == id);

            }
        }
        public IEnumerable<Candidate> GetConfirmed()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Where(i => i.Confirmed == true).ToList();
            }
        }
        public int GetConfirmedCount()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Count(i => i.Confirmed == true);
            }
        }

        public IEnumerable<Candidate> GetDeclined()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Where(i => i.Confirmed == false).ToList();
            }
        }
        public int GetDeclinedCount()
        {
            using (var context = new CandidateContext(_connectionString))
            {
                return context.Candidates.Where(i => i.Confirmed == false).ToList().Count();
            }
        }
        public  void Confirm (int id)
        {
            using (var context = new CandidateContext(_connectionString))

            {
                Candidate i = context.Candidates.FirstOrDefault(p => p.Id == id);
                i.Confirmed=true;
                context.Entry(i).State = EntityState.Modified;
                context.SaveChanges();


            }
        }
        public void Decline(int id)
        {
            using (var context = new CandidateContext(_connectionString))

            {
                Candidate i = context.Candidates.FirstOrDefault(p => p.Id == id);
                i.Confirmed = false;
                context.Entry(i).State = EntityState.Modified;
                context.SaveChanges();


            }
        }
    }
}
