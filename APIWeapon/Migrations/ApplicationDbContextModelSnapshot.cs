﻿// <auto-generated />
using APIWeapon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIWeapon.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.4.22229.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("APIWeapon.Models.AddFriendNoti", b =>
                {
                    b.Property<int>("AddFriId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddFriId"), 1L, 1);

                    b.Property<string>("DateTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HandleOrNot")
                        .HasColumnType("bit");

                    b.Property<string>("TheReceiver")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TheSender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddFriId");

                    b.ToTable("AddFriendNotis");
                });

            modelBuilder.Entity("APIWeapon.Models.CharacterModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CharacterName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Rule")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CharacterName")
                        .IsUnique();

                    b.ToTable("dbo.CharacterModels", (string)null);
                    b.HasData(
                        new
                        {
                            Id = 4214124,
                            CharacterName = "Terenas Menathil",
                            Password = "bb0f7e021d52a4e31613d463fc0525d8",
                            Gmail = "chaunguyengiang2000@gmail.com",
                            Class = "Paladin",
                            Rule = "King",
                            Token = ""
                        });
                });

            modelBuilder.Entity("APIWeapon.Models.FriendList", b =>
                {
                    b.Property<int>("FriendListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FriendListId"), 1L, 1);

                    b.Property<string>("FriendName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TheOwnered")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FriendListId");

                    b.ToTable("FriendLists");
                });

            modelBuilder.Entity("APIWeapon.Models.LoginModel", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LoginId");

                    b.ToTable("LoginModels");
                });

            modelBuilder.Entity("APIWeapon.Models.NotificationModel", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"), 1L, 1);

                    b.Property<string>("DateTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HandleOrNot")
                        .HasColumnType("bit");

                    b.Property<string>("TheReceiver")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TheSender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WeaponTrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationId");

                    b.ToTable("NotificationModels");
                });

            modelBuilder.Entity("APIWeapon.Models.PreviousPasswordModel", b =>
                {
                    b.Property<int>("DeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeId"), 1L, 1);

                    b.Property<bool>("Handle")
                        .HasColumnType("bit");

                    b.Property<string>("PreviousToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeId");

                    b.ToTable("PreviousPasswordModels");
                });

            modelBuilder.Entity("APIWeapon.Models.ResettingPasswordModel", b =>
                {
                    b.Property<int>("RsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RsId"), 1L, 1);

                    b.Property<string>("RsCharacterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RsClass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RsGmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RsRule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RsId");

                    b.ToTable("ResettingPasswordModels");
                });

            modelBuilder.Entity("APIWeapon.Models.ResettingToken", b =>
                {
                    b.Property<int>("TkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TkId"), 1L, 1);

                    b.Property<string>("ResetToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TkId");

                    b.ToTable("ResettingTokens");
                });

            modelBuilder.Entity("APIWeapon.Models.SearchingFriend", b =>
                {
                    b.Property<int>("SearchingFriendId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SearchingFriendId"), 1L, 1);

                    b.Property<string>("SearchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SearchingFriendId");

                    b.ToTable("SearchingFriends");
                });

            modelBuilder.Entity("APIWeapon.Models.SearchingWeapon", b =>
                {
                    b.Property<int>("SearchingWeaponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SearchingWeaponId"), 1L, 1);

                    b.Property<int>("SearchingWeaponAttack")
                        .HasColumnType("int");

                    b.Property<string>("SearchingWeaponAttribute")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SearchingWeaponDefense")
                        .HasColumnType("int");

                    b.Property<string>("SearchingWeaponName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SearchingWeaponId");

                    b.ToTable("SearchingWeapons");
                });

            modelBuilder.Entity("APIWeapon.Models.WeaponModel", b =>
                {
                    b.Property<int>("WeaponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WeaponId"), 1L, 1);

                    b.Property<int>("WeaponAttack")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("WeaponAttribute")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("WeaponDefense")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("WeaponName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("WeaponOwner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WeaponId");

                    b.ToTable("dbo.WeaponModels", (string)null);
                    b.HasData(
                            new
                            {
                                WeaponId = 4134322,
                                WeaponName = "Stormbreaker - Thunder Hummer",
                                WeaponAttack = 1500,
                                WeaponDefense = 1800,
                                WeaponAttribute = "Light",
                                WeaponOwner = "Terenas Menathil"
                            });
                });
            modelBuilder.Entity("APIWeapon.Models.SuggestionClassModel", b =>
            {
                b.Property<int>("SuggestionClassId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SuggestionClassId"), 1L, 1);

                b.Property<string>("SuggestionClassName")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestionClassDescription")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestionClassAttribute")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
                b.Property<string>("SuggestionClassWeapon")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
                b.Property<string>("SuggestionClassEffect")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.HasKey("SuggestionClassId");

                b.ToTable("dbo.SuggestionClass");
            });
            modelBuilder.Entity("APIWeapon.Models.SuggestionGameModel", b =>
            {
                b.Property<int>("SuggestionGameId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SuggestionGameId"), 1L, 1);

                b.Property<string>("SuggestionGameDescription")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestionGamePros")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestionGameCons")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
                b.Property<int>("SuggestionGameRate")
                    .IsRequired()
                    .HasColumnType("int");

                b.HasKey("SuggestionGameId");

                b.ToTable("dbo.SuggestionGame");
            });

            modelBuilder.Entity("APIWeapon.Models.SuggestionWeaponModel", b =>
            {
                b.Property<int>("SuggestWeaponId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SuggestWeaponId"), 1L, 1);

                b.Property<string>("SuggestWeaponName")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestWeaponEffect")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("SuggestWeaponAttribute")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
                b.Property<string>("SuggestWeaponDescription")
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");

                b.HasKey("SuggestWeaponId");

                b.ToTable("dbo.SuggestionWeapon");
            });
#pragma warning restore 612, 618
        }
    }
}
