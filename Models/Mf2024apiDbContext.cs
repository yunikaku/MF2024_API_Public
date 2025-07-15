using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MF2024_API.Models;
using MySql.EntityFrameworkCore;


namespace MF2024_API.Models;

public partial class Mf2024apiDbContext : IdentityDbContext<User>
{
    public Mf2024apiDbContext()
    {
    }

    public Mf2024apiDbContext(DbContextOptions<Mf2024apiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Nfc> Nfcs { get; set; }

    public virtual DbSet<Nfcallotment> Nfcallotments { get; set; }

    public virtual DbSet<NoReservation> NoReservations { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<OptOut> OptOuts { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<RoomImage> Images { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<Entrants> Entrants { get; set; }





    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            //SQLServerの場合
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            //MySQLの場合
            //optionsBuilder.UseMySQL(configuration.GetConnectionString("DefaultConnection"));


        }


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD8440F670");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.DepartmentNameKana)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.AddUser).WithMany(p => p.DepartmentUpDate)
                .HasForeignKey(d => d.DepartmentUpDateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Department_UpDateUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.DepartmentAdd)
            .HasForeignKey(d => d.DepartmentAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Department_AddUserID");



        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK__Device__49E1233136A11D86");

            entity.ToTable("Device");

            entity.Property(e => e.DeviceId)
                .ValueGeneratedNever()
                .HasColumnName("DeviceID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DeviceLocation).HasMaxLength(50);
            entity.Property(e => e.DeviceName).HasMaxLength(50);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.DeviceUpDateTime).HasColumnType("datetime");



            entity.HasOne(d => d.UpdateUser).WithMany(p => p.DeviceUpdate)
                .HasForeignKey(d => d.DeviceUpdateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Device_UserID");

            entity.HasOne(d => d.AddUser).WithMany(p => p.DeviceAdd)
            .HasForeignKey(d => d.DeviceAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Device_AddUserID");
        });

        modelBuilder.Entity<Nfc>(entity =>
        {
            entity.HasKey(e => e.NfcId).HasName("PK__NFC__628F58637E500CAF");

            entity.ToTable("NFC");

            entity.Property(e => e.NfcId)
                .ValueGeneratedNever()
                .HasColumnName("NfcID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NfcAddTime).HasColumnType("datetime");
            entity.Property(e => e.NfcUid).HasColumnName("NfcUID");
            entity.Property(e => e.NfcUpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.AddUser).WithMany(p => p.NfcAdd)
                .HasForeignKey(d => d.NfcAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFC_AddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.NfcUpdate)
                .HasForeignKey(d => d.NfcUpdateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFC_UpDateUserID");
        });

        modelBuilder.Entity<Nfcallotment>(entity =>
        {
            entity.HasKey(e => e.NfcallotmentId).HasName("PK__NFCAllot__5C5F4CB94877DFBC");

            entity.ToTable("NFCAllotment");

            entity.Property(e => e.NfcallotmentId)
                .ValueGeneratedNever()
                .HasColumnName("NFCAllotmentID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.AllotmentTime).HasColumnType("datetime");
            entity.Property(e => e.NfcId).HasColumnName("NfcID");
            entity.Property(e => e.NoReservationId).HasColumnName("NoReservationID");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

            entity.HasOne(d => d.Nfc).WithMany(p => p.Nfcallotments)
                .HasForeignKey(d => d.NfcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFCAllotment_NfcID");

            entity.HasOne(d => d.NoReservation).WithMany(p => p.Nfcallotments)
                .HasForeignKey(d => d.NoReservationId)
                .HasConstraintName("FK_NFCAllotment_NoReservationID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Nfcallotments)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_NFCAllotment_ReservationID");

            entity.HasOne(d => d.AddUser).WithMany(p => p.NfcallotmentAdd)
            .HasForeignKey(d => d.AddUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFCAllotment_AddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.NfcallotmentUpDate)
            .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NFCAllotment_UpdateUserID");
        });

        modelBuilder.Entity<NoReservation>(entity =>
        {
            entity.HasKey(e => e.NoReservationId).HasName("PK__NoReserv__64ADE52735B426D1");

            entity.ToTable("NoReservation");

            entity.Property(e => e.NoReservationId)
                .ValueGeneratedNever()
                .HasColumnName("NoReservationID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.NoReservationCompanyName).HasMaxLength(50);
            entity.Property(e => e.NoReservationCompanyNameKana).HasMaxLength(50);
            entity.Property(e => e.NoReservationCompanyPosition).HasMaxLength(50);
            entity.Property(e => e.NoReservationDate).HasColumnType("datetime");
            entity.Property(e => e.NoReservationEmail).HasMaxLength(50);
            entity.Property(e => e.NoReservationName).HasMaxLength(50);
            entity.Property(e => e.NoReservationNameKana).HasMaxLength(50);
            entity.Property(e => e.NoReservationPhoneNumber).HasMaxLength(50);

            entity.HasOne(d => d.AddUser).WithMany(p => p.NoReservationAdd)
                .HasForeignKey(d => d.NoReservationAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoReservation_AddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.NoReservationUpdate)
            .HasForeignKey(d => d.NoReservationUpdateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoReservation_UpdateUserID");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.OfficeId).HasName("PK__Office__4B61930F387A0FF8");

            entity.ToTable("Office");

            entity.Property(e => e.OfficeId)
                .ValueGeneratedNever()
                .HasColumnName("OfficeID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.OfficeLocation).HasMaxLength(50);
            entity.Property(e => e.OfficeName).HasMaxLength(50);
            entity.Property(e => e.OfficeNameKana).HasMaxLength(50);

            entity.HasOne(d => d.AddUser).WithMany(p => p.OfficeAdd)
                .HasForeignKey(d => d.OfficeAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_AddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.OfficeUpdate)
            .HasForeignKey(d => d.OfficeUpDateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_UpDateUserID");
        });

        modelBuilder.Entity<OptOut>(entity =>
        {
            entity.HasKey(e => e.OptOutId).HasName("PK__OptOut__82926A1C961131F5");

            entity.ToTable("OptOut");

            entity.Property(e => e.OptOutId)
                .ValueGeneratedNever()
                .HasColumnName("OptOutID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.NfcallotmentId).HasColumnName("NFCAllotmentID");
            entity.Property(e => e.OptOutTime).HasColumnType("datetime");

            entity.HasOne(d => d.Device).WithMany(p => p.OptOuts)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OptOut_Device");

            entity.HasOne(d => d.Nfcallotment).WithMany(p => p.OptOuts)
                .HasForeignKey(d => d.NfcallotmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OptOut_NFCAllotment");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PK__Reservat__B7EE5F0420AEDDCE");

            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId)
                .ValueGeneratedNever()
                .HasColumnName("ReservationID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.ReservationCode).HasMaxLength(50);
            entity.Property(e => e.ReservationCompanyName).HasMaxLength(50);
            entity.Property(e => e.ReservationCompanyNameKana).HasMaxLength(50);
            entity.Property(e => e.ReservationCompanyPosition).HasMaxLength(50);
            entity.Property(e => e.ReservationDate).HasColumnType("datetime");
            entity.Property(e => e.ReservationEmail).HasMaxLength(50);
            entity.Property(e => e.ReservationName).HasMaxLength(50);
            entity.Property(e => e.ReservationNameKana).HasMaxLength(50);
            entity.Property(e => e.ReservationPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.ReservationQrcode).HasColumnName("ReservationQRcode");

            entity.HasOne(d => d.AddUser).WithMany(p => p.ReservationAdd)
            .HasForeignKey(d => d.ReservationAddUserID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Reservation_AddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.ReservationUpdate)
            .HasForeignKey(d => d.ReservationUpdateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservation_UpdateUserID");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__328639194D197077");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId)
                .ValueGeneratedNever()
                .HasColumnName("RoomID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.RoomName).HasMaxLength(50);
            entity.Property(e => e.SectionId).HasColumnName("SectionID");

            entity.HasOne(d => d.Office).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_OfficeID");

            entity.HasOne(d => d.Section).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_Room_SectionID");

            entity.HasOne(d => d.AddUser).WithMany(p => p.RoomAdd)
            .HasForeignKey(d => d.RoomAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_SectionAddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.RoomUpdate)
            .HasForeignKey(d => d.RoompDateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_SectionUpDateUserID");

        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasKey(e => e.RoomImageID).HasName("PK__Image__80EF08927BE474C1");

            entity.ToTable("Image");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentID).HasName("PK__Equipment__80EF08927BE474C1");

            entity.ToTable("Equipment");
        });


        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK__Section__80EF08927BE474C1");

            entity.ToTable("Section");

            entity.Property(e => e.SectionId)
                .ValueGeneratedNever()
                .HasColumnName("SectionID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.SectionName).HasMaxLength(50);
            entity.Property(e => e.SectionNameKana).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.Sections)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_DepartmentID");

            entity.HasOne(d => d.AddUser).WithMany(p => p.SectionAdd)
            .HasForeignKey(d => d.SectionAddUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_SectionAddUserID");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.SectionUpdate)
            .HasForeignKey(d => d.SectionUpDateUserID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Section_SectionUpDateUserID");
        });



        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__1788CC4C3A3D3A3A");

            entity.ToTable("User");
        });





        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<MF2024_API.Models.ConferenceRoomReservation> ConferenceRoomReservation { get; set; } = default!;
}
