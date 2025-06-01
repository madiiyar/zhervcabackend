using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace vc.Models;

public partial class VcdbContext : DbContext
{
    public VcdbContext()
    {
    }

    public VcdbContext(DbContextOptions<VcdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Businessmodel> Businessmodels { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Developmentstage> Developmentstages { get; set; }

    public virtual DbSet<Emailotp> Emailotps { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<Innovationmethod> Innovationmethods { get; set; }

    public virtual DbSet<Investmentstage> Investmentstages { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<Salesmodel> Salesmodels { get; set; }

    public virtual DbSet<Sourceinfo> Sourceinfos { get; set; }

    public virtual DbSet<Startup> Startups { get; set; }

    public virtual DbSet<Startupinvestoroffer> Startupinvestoroffers { get; set; }

    public virtual DbSet<Supportmessage> Supportmessages { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=vcdb.postgres.database.azure.com;Port=5432;Database=vcdb;Username=madiyar;Password=Admin777.;SSL Mode=Require;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Businessmodel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("businessmodels_pkey");

            entity.ToTable("businessmodels");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Developmentstage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("developmentstages_pkey");

            entity.ToTable("developmentstages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Emailotp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("emailotps_pkey");

            entity.ToTable("emailotps");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property(e => e.Expiresat).HasColumnName("expiresat");
            entity.Property(e => e.Isused)
                .HasDefaultValue(false)
                .HasColumnName("isused");
            entity.Property(e => e.Otpcode)
                .HasMaxLength(6)
                .HasColumnName("otpcode");
            entity.Property(e => e.Purpose)
                .HasMaxLength(20)
                .HasColumnName("purpose");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Emailotps)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("emailotps_userid_fkey");
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("industries_pkey");

            entity.ToTable("industries");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Innovationmethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("innovationmethods_pkey");

            entity.ToTable("innovationmethods");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Investmentstage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("investmentstages_pkey");

            entity.ToTable("investmentstages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Investor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("investors_pkey");

            entity.ToTable("investors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contactfullname)
                .HasMaxLength(150)
                .HasColumnName("contactfullname");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Hasstartuppilotexperience).HasColumnName("hasstartuppilotexperience");
            entity.Property(e => e.Identificationnumber)
                .HasMaxLength(50)
                .HasColumnName("identificationnumber");
            entity.Property(e => e.Investmentamount)
                .HasPrecision(18, 2)
                .HasColumnName("investmentamount");
            entity.Property(e => e.Investortype)
                .HasMaxLength(50)
                .HasColumnName("investortype");
            entity.Property(e => e.Investsinstartups).HasColumnName("investsinstartups");
            entity.Property(e => e.Logopath).HasColumnName("logopath");
            entity.Property(e => e.Organizationname)
                .HasMaxLength(255)
                .HasColumnName("organizationname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Profilephotopath).HasColumnName("profilephotopath");
            entity.Property(e => e.Publicemail)
                .HasMaxLength(255)
                .HasColumnName("publicemail");
            entity.Property(e => e.Sourceinfoid).HasColumnName("sourceinfoid");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");

            entity.HasOne(d => d.Country).WithMany(p => p.Investors)
                .HasForeignKey(d => d.Countryid)
                .HasConstraintName("investors_countryid_fkey");

            entity.HasOne(d => d.Sourceinfo).WithMany(p => p.Investors)
                .HasForeignKey(d => d.Sourceinfoid)
                .HasConstraintName("investors_sourceinfoid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Investors)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("investors_userid_fkey");

            entity.HasMany(d => d.Developmentstages).WithMany(p => p.Investors)
                .UsingEntity<Dictionary<string, object>>(
                    "Investordevelopmentstage",
                    r => r.HasOne<Developmentstage>().WithMany()
                        .HasForeignKey("Developmentstageid")
                        .HasConstraintName("investordevelopmentstages_developmentstageid_fkey"),
                    l => l.HasOne<Investor>().WithMany()
                        .HasForeignKey("Investorid")
                        .HasConstraintName("investordevelopmentstages_investorid_fkey"),
                    j =>
                    {
                        j.HasKey("Investorid", "Developmentstageid").HasName("investordevelopmentstages_pkey");
                        j.ToTable("investordevelopmentstages");
                        j.IndexerProperty<int>("Investorid").HasColumnName("investorid");
                        j.IndexerProperty<int>("Developmentstageid").HasColumnName("developmentstageid");
                    });

            entity.HasMany(d => d.Industries).WithMany(p => p.Investors)
                .UsingEntity<Dictionary<string, object>>(
                    "Investorindustry",
                    r => r.HasOne<Industry>().WithMany()
                        .HasForeignKey("Industryid")
                        .HasConstraintName("investorindustries_industryid_fkey"),
                    l => l.HasOne<Investor>().WithMany()
                        .HasForeignKey("Investorid")
                        .HasConstraintName("investorindustries_investorid_fkey"),
                    j =>
                    {
                        j.HasKey("Investorid", "Industryid").HasName("investorindustries_pkey");
                        j.ToTable("investorindustries");
                        j.IndexerProperty<int>("Investorid").HasColumnName("investorid");
                        j.IndexerProperty<int>("Industryid").HasColumnName("industryid");
                    });

            entity.HasMany(d => d.Innovationmethods).WithMany(p => p.Investors)
                .UsingEntity<Dictionary<string, object>>(
                    "Investorinnovationmethod",
                    r => r.HasOne<Innovationmethod>().WithMany()
                        .HasForeignKey("Innovationmethodid")
                        .HasConstraintName("investorinnovationmethods_innovationmethodid_fkey"),
                    l => l.HasOne<Investor>().WithMany()
                        .HasForeignKey("Investorid")
                        .HasConstraintName("investorinnovationmethods_investorid_fkey"),
                    j =>
                    {
                        j.HasKey("Investorid", "Innovationmethodid").HasName("investorinnovationmethods_pkey");
                        j.ToTable("investorinnovationmethods");
                        j.IndexerProperty<int>("Investorid").HasColumnName("investorid");
                        j.IndexerProperty<int>("Innovationmethodid").HasColumnName("innovationmethodid");
                    });

            entity.HasMany(d => d.Technologies).WithMany(p => p.Investors)
                .UsingEntity<Dictionary<string, object>>(
                    "Investortechnology",
                    r => r.HasOne<Technology>().WithMany()
                        .HasForeignKey("Technologyid")
                        .HasConstraintName("investortechnologies_technologyid_fkey"),
                    l => l.HasOne<Investor>().WithMany()
                        .HasForeignKey("Investorid")
                        .HasConstraintName("investortechnologies_investorid_fkey"),
                    j =>
                    {
                        j.HasKey("Investorid", "Technologyid").HasName("investortechnologies_pkey");
                        j.ToTable("investortechnologies");
                        j.IndexerProperty<int>("Investorid").HasColumnName("investorid");
                        j.IndexerProperty<int>("Technologyid").HasColumnName("technologyid");
                    });
        });

        modelBuilder.Entity<Salesmodel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("salesmodels_pkey");

            entity.ToTable("salesmodels");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Sourceinfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sourceinfos_pkey");

            entity.ToTable("sourceinfos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Startup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("startups_pkey");

            entity.ToTable("startups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activelylookingforinvestment).HasColumnName("activelylookingforinvestment");
            entity.Property(e => e.Contactfullname)
                .HasMaxLength(150)
                .HasColumnName("contactfullname");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Developmentstageid).HasColumnName("developmentstageid");
            entity.Property(e => e.Employeecount).HasColumnName("employeecount");
            entity.Property(e => e.Foundingyear).HasColumnName("foundingyear");
            entity.Property(e => e.Hassales).HasColumnName("hassales");
            entity.Property(e => e.Identificationnumber)
                .HasMaxLength(50)
                .HasColumnName("identificationnumber");
            entity.Property(e => e.Investmentstageid).HasColumnName("investmentstageid");
            entity.Property(e => e.Investorlist).HasColumnName("investorlist");
            entity.Property(e => e.Logopath).HasColumnName("logopath");
            entity.Property(e => e.Organizationname)
                .HasMaxLength(255)
                .HasColumnName("organizationname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Presentationpath).HasColumnName("presentationpath");
            entity.Property(e => e.Publicemail)
                .HasMaxLength(255)
                .HasColumnName("publicemail");
            entity.Property(e => e.Publicname)
                .HasMaxLength(150)
                .HasColumnName("publicname");
            entity.Property(e => e.Sourceinfoid).HasColumnName("sourceinfoid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Totalpreviousinvestment)
                .HasPrecision(18, 2)
                .HasColumnName("totalpreviousinvestment");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");

            entity.HasOne(d => d.Country).WithMany(p => p.Startups)
                .HasForeignKey(d => d.Countryid)
                .HasConstraintName("startups_countryid_fkey");

            entity.HasOne(d => d.Developmentstage).WithMany(p => p.Startups)
                .HasForeignKey(d => d.Developmentstageid)
                .HasConstraintName("startups_developmentstageid_fkey");

            entity.HasOne(d => d.Investmentstage).WithMany(p => p.Startups)
                .HasForeignKey(d => d.Investmentstageid)
                .HasConstraintName("startups_investmentstageid_fkey");

            entity.HasOne(d => d.Sourceinfo).WithMany(p => p.Startups)
                .HasForeignKey(d => d.Sourceinfoid)
                .HasConstraintName("startups_sourceinfoid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Startups)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("startups_userid_fkey");

            entity.HasMany(d => d.Businessmodels).WithMany(p => p.Startups)
                .UsingEntity<Dictionary<string, object>>(
                    "Startupbusinessmodel",
                    r => r.HasOne<Businessmodel>().WithMany()
                        .HasForeignKey("Businessmodelid")
                        .HasConstraintName("startupbusinessmodels_businessmodelid_fkey"),
                    l => l.HasOne<Startup>().WithMany()
                        .HasForeignKey("Startupid")
                        .HasConstraintName("startupbusinessmodels_startupid_fkey"),
                    j =>
                    {
                        j.HasKey("Startupid", "Businessmodelid").HasName("startupbusinessmodels_pkey");
                        j.ToTable("startupbusinessmodels");
                        j.IndexerProperty<int>("Startupid").HasColumnName("startupid");
                        j.IndexerProperty<int>("Businessmodelid").HasColumnName("businessmodelid");
                    });

            entity.HasMany(d => d.Countries).WithMany(p => p.StartupsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Startuptargetcountry",
                    r => r.HasOne<Country>().WithMany()
                        .HasForeignKey("Countryid")
                        .HasConstraintName("startuptargetcountries_countryid_fkey"),
                    l => l.HasOne<Startup>().WithMany()
                        .HasForeignKey("Startupid")
                        .HasConstraintName("startuptargetcountries_startupid_fkey"),
                    j =>
                    {
                        j.HasKey("Startupid", "Countryid").HasName("startuptargetcountries_pkey");
                        j.ToTable("startuptargetcountries");
                        j.IndexerProperty<int>("Startupid").HasColumnName("startupid");
                        j.IndexerProperty<int>("Countryid").HasColumnName("countryid");
                    });

            entity.HasMany(d => d.Industries).WithMany(p => p.Startups)
                .UsingEntity<Dictionary<string, object>>(
                    "Startupindustry",
                    r => r.HasOne<Industry>().WithMany()
                        .HasForeignKey("Industryid")
                        .HasConstraintName("startupindustries_industryid_fkey"),
                    l => l.HasOne<Startup>().WithMany()
                        .HasForeignKey("Startupid")
                        .HasConstraintName("startupindustries_startupid_fkey"),
                    j =>
                    {
                        j.HasKey("Startupid", "Industryid").HasName("startupindustries_pkey");
                        j.ToTable("startupindustries");
                        j.IndexerProperty<int>("Startupid").HasColumnName("startupid");
                        j.IndexerProperty<int>("Industryid").HasColumnName("industryid");
                    });

            entity.HasMany(d => d.Salesmodels).WithMany(p => p.Startups)
                .UsingEntity<Dictionary<string, object>>(
                    "Startupsalesmodel",
                    r => r.HasOne<Salesmodel>().WithMany()
                        .HasForeignKey("Salesmodelid")
                        .HasConstraintName("startupsalesmodels_salesmodelid_fkey"),
                    l => l.HasOne<Startup>().WithMany()
                        .HasForeignKey("Startupid")
                        .HasConstraintName("startupsalesmodels_startupid_fkey"),
                    j =>
                    {
                        j.HasKey("Startupid", "Salesmodelid").HasName("startupsalesmodels_pkey");
                        j.ToTable("startupsalesmodels");
                        j.IndexerProperty<int>("Startupid").HasColumnName("startupid");
                        j.IndexerProperty<int>("Salesmodelid").HasColumnName("salesmodelid");
                    });

            entity.HasMany(d => d.Technologies).WithMany(p => p.Startups)
                .UsingEntity<Dictionary<string, object>>(
                    "Startuptechnology",
                    r => r.HasOne<Technology>().WithMany()
                        .HasForeignKey("Technologyid")
                        .HasConstraintName("startuptechnologies_technologyid_fkey"),
                    l => l.HasOne<Startup>().WithMany()
                        .HasForeignKey("Startupid")
                        .HasConstraintName("startuptechnologies_startupid_fkey"),
                    j =>
                    {
                        j.HasKey("Startupid", "Technologyid").HasName("startuptechnologies_pkey");
                        j.ToTable("startuptechnologies");
                        j.IndexerProperty<int>("Startupid").HasColumnName("startupid");
                        j.IndexerProperty<int>("Technologyid").HasColumnName("technologyid");
                    });
        });

        modelBuilder.Entity<Startupinvestoroffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("startupinvestoroffers_pkey");

            entity.ToTable("startupinvestoroffers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Investorid).HasColumnName("investorid");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Requestedby)
                .HasMaxLength(50)
                .HasColumnName("requestedby");
            entity.Property(e => e.Startupid).HasColumnName("startupid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Investor).WithMany(p => p.Startupinvestoroffers)
                .HasForeignKey(d => d.Investorid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("startupinvestoroffers_investorid_fkey");

            entity.HasOne(d => d.Startup).WithMany(p => p.Startupinvestoroffers)
                .HasForeignKey(d => d.Startupid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("startupinvestoroffers_startupid_fkey");
        });

        modelBuilder.Entity<Supportmessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supportmessages_pkey");

            entity.ToTable("supportmessages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Message).HasColumnName("message");
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("technologies_pkey");

            entity.ToTable("technologies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(150)
                .HasColumnName("fullname");
            entity.Property(e => e.Isemailconfirmed)
                .HasDefaultValue(false)
                .HasColumnName("isemailconfirmed");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updatedat");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
