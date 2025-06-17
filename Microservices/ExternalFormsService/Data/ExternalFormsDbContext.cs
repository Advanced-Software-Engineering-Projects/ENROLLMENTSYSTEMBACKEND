using ExternalFormsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ExternalFormsService.Data
{
    public class ExternalFormsDbContext : DbContext
    {
        public ExternalFormsDbContext(DbContextOptions<ExternalFormsDbContext> options)
            : base(options)
        {
        }

        public DbSet<FormSubmission> FormSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FormSubmission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentId).IsRequired();
                entity.Property(e => e.FormType).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.SubmissionDate).IsRequired();

                // Configure JSON serialization for FormData
                entity.Property(e => e.FormData)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null));

                // Configure JSON serialization for Attachments
                entity.Property(e => e.Attachments)
                    .HasConversion(
                        v => System.Text.Json.JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));
            });
        }
    }
} 