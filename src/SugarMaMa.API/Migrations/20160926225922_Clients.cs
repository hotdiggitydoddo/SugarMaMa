using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SugarMaMa.API.Migrations
{
    public partial class Clients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    AppointmentRemindersViaEmail = table.Column<bool>(nullable: false),
                    AppointmentRemindersViaText = table.Column<bool>(nullable: false),
                    NoShows = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    BusinessDayId = table.Column<int>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    EstheticianId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_BusinessDays_BusinessDayId",
                        column: x => x.BusinessDayId,
                        principalTable: "BusinessDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shifts_Estheticians_EstheticianId",
                        column: x => x.EstheticianId,
                        principalTable: "Estheticians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "SpaServices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Appointments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Appointments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "RemindViaEmail",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RemindViaText",
                table: "Appointments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftId",
                table: "Appointments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SpaServices_AppointmentId",
                table: "SpaServices",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClientId",
                table: "Appointments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ShiftId",
                table: "Appointments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ApplicationUserId",
                table: "Client",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_BusinessDayId",
                table: "Shifts",
                column: "BusinessDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EstheticianId",
                table: "Shifts",
                column: "EstheticianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Client_ClientId",
                table: "Appointments",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Shifts_ShiftId",
                table: "Appointments",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpaServices_Appointments_AppointmentId",
                table: "SpaServices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Client_ClientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Shifts_ShiftId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_SpaServices_Appointments_AppointmentId",
                table: "SpaServices");

            migrationBuilder.DropIndex(
                name: "IX_SpaServices_AppointmentId",
                table: "SpaServices");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ClientId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_ShiftId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "SpaServices");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RemindViaEmail",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RemindViaText",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Shifts");
        }
    }
}
