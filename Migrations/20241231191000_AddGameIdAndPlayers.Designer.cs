﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAspNetCoreApp.Data;

#nullable disable

namespace MyAspNetCoreApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241231191000_AddGameIdAndPlayers")]
    partial class AddGameIdAndPlayers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("MyAspNetCoreApp.Models.GameState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Board")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerO")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerX")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Winner")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("MyAspNetCoreApp.Models.PlayerScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PlayerScores");
                });
#pragma warning restore 612, 618
        }
    }
}
