using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ENROLLMENTSYSTEMBACKEND.Data;

namespace ENROLLMENTSYSTEMBACKEND.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250601213630_AddUserLogs")]
    partial class AddUserLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ENROLLMENTSYSTEMBACKEND.Models.UserLog", b =>
            {
                b.Property<int>("UserLogId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserLogId"));

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("EmailAddress")
                    .IsRequired()
                    .HasColumnType("nvarchar(256)");

                b.Property<DateTime>("UserLogTimeStamp")
                    .HasColumnType("datetime2");

                b.Property<string>("UserLogActivity")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("UserProfileImagePath")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserLogId");

                b.HasIndex("UserId");

                b.ToTable("UserLogs");
            });

            modelBuilder.Entity("ENROLLMENTSYSTEMBACKEND.Models.UserLog", b =>
            {
                b.HasOne("ENROLLMENTSYSTEMBACKEND.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });
        }
    }
}