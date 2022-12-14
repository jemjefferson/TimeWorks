using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Manager = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartingPay = table.Column<double>(type: "float", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Admin = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logins_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeJobCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    JobCodeId = table.Column<int>(type: "int", nullable: false),
                    PayRate = table.Column<double>(type: "float", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeJobCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeJobCodes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeJobCodes_JobCodes_JobCodeId",
                        column: x => x.JobCodeId,
                        principalTable: "JobCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TimeEntered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EmployeeJobCodeId = table.Column<int>(type: "int", nullable: false),
                    PayPeriodId = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    EmployeeApproved = table.Column<bool>(type: "bit", nullable: false),
                    PayRate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hours_EmployeeJobCodes_EmployeeJobCodeId",
                        column: x => x.EmployeeJobCodeId,
                        principalTable: "EmployeeJobCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hours_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Hours_PayPeriods_PayPeriodId",
                        column: x => x.PayPeriodId,
                        principalTable: "PayPeriods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Breaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    HourId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breaks_Hours_HourId",
                        column: x => x.HourId,
                        principalTable: "Hours",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "City", "FirstName", "LastName", "Manager", "State", "Zip" },
                values: new object[,]
                {
                    { 1, "123 Street", "Wausau", "Jacob", "Mahner", 1, "WI", "54401-0400" },
                    { 2, "456 Street", "Wausau", "Zach", "Johnson", 1, "WI", "54401-0400" },
                    { 3, "789 Street", "Wausau", "Jordan", "Jefferson", 2, "WI", "54401-0400" }
                });

            migrationBuilder.InsertData(
                table: "JobCodes",
                columns: new[] { "Id", "Department", "JobTitle", "StartingPay" },
                values: new object[,]
                {
                    { 1, "Information Technology", "Timeworks Admin", 23.5 },
                    { 2, "Information Technology", "Timeworks Manager", 22.5 },
                    { 3, "Information Technology", "Timeworks Developer", 21.5 },
                    { 4, "Information Technology", "Application Support Senior", 20.0 }
                });

            migrationBuilder.InsertData(
                table: "PayPeriods",
                columns: new[] { "Id", "End", "Start" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2022, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "EmployeeJobCodes",
                columns: new[] { "Id", "Active", "EmployeeId", "JobCodeId", "PayRate" },
                values: new object[,]
                {
                    { 1, true, 1, 1, 24.0 },
                    { 2, true, 2, 2, 23.5 },
                    { 3, true, 3, 3, 21.5 },
                    { 4, true, 3, 4, 20.0 }
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Admin", "EmployeeID", "PasswordHash", "UserName" },
                values: new object[,]
                {
                    { 1, true, 1, "AQAAAAEAACcQAAAAEHQvShwMx9WV7o77eCrLDyweVJh+EWJtxBNYH72/qEm1WSi8SKm3uCBmMzzR6dkdaQ==", "jacobmahner1" },
                    { 2, false, 2, "AQAAAAEAACcQAAAAEDjKdRio1JIvInP+fSSt3L6ZzCN7xLJr+Oe8eRc0Yvh2FvW6utOsoI4qa/st5Cr1TQ==", "zachjohnson1" },
                    { 3, false, 3, "AQAAAAEAACcQAAAAEIZINxTPRI5W7NZ5Duo2Gffy1hnOUzWf7XU2u+F1MaRh4Y9tBAXdKI98rwe7ae0ZRw==", "jordanjefferson1" }
                });

            migrationBuilder.InsertData(
                table: "Hours",
                columns: new[] { "Id", "ApprovedBy", "Comment", "EmployeeApproved", "EmployeeId", "EmployeeJobCodeId", "PayPeriodId", "PayRate", "TimeEntered", "TimeIn", "TimeOut" },
                values: new object[,]
                {
                    { 1, 1, "Comment 1", true, 1, 1, 1, 24.0, new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Comment 2", true, 1, 1, 1, 24.0, new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, "Comment 3", true, 1, 1, 1, 24.0, new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "Comment 1", true, 2, 2, 1, 23.5, new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, "Comment 2", true, 2, 2, 1, 23.5, new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, "Comment 3", true, 2, 2, 1, 23.5, new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 2, "Comment 1", true, 3, 3, 1, 21.5, new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, "Comment 2", true, 3, 3, 1, 21.5, new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 2, "Comment 3", true, 3, 4, 1, 20.0, new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Breaks",
                columns: new[] { "Id", "Comment", "EndTime", "HourId", "Paid", "StartTime" },
                values: new object[] { 1, "Comment 1", new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified), 2, false, new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Breaks",
                columns: new[] { "Id", "Comment", "EndTime", "HourId", "Paid", "StartTime" },
                values: new object[] { 2, "Comment 2", new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified), 5, false, new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Breaks",
                columns: new[] { "Id", "Comment", "EndTime", "HourId", "Paid", "StartTime" },
                values: new object[] { 3, "Comment 3", new DateTime(2022, 9, 5, 3, 30, 0, 0, DateTimeKind.Unspecified), 8, true, new DateTime(2022, 9, 5, 3, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Breaks_HourId",
                table: "Breaks",
                column: "HourId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeJobCodes_EmployeeId",
                table: "EmployeeJobCodes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeJobCodes_JobCodeId",
                table: "EmployeeJobCodes",
                column: "JobCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hours_EmployeeId",
                table: "Hours",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hours_EmployeeJobCodeId",
                table: "Hours",
                column: "EmployeeJobCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hours_PayPeriodId",
                table: "Hours",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_EmployeeID",
                table: "Logins",
                column: "EmployeeID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breaks");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Hours");

            migrationBuilder.DropTable(
                name: "EmployeeJobCodes");

            migrationBuilder.DropTable(
                name: "PayPeriods");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "JobCodes");
        }
    }
}
