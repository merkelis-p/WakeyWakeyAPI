using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WakeyWakeyAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Salt = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<int>(type: "int(11)", nullable: false),
                    score = table.Column<int>(type: "int(11)", nullable: true),
                    user_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.id);
                    table.ForeignKey(
                        name: "course_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    recurring = table.Column<int>(type: "int(11)", nullable: true),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.id);
                    table.ForeignKey(
                        name: "event_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<int>(type: "int(11)", nullable: false),
                    score = table.Column<int>(type: "int(11)", nullable: true),
                    score_weight = table.Column<int>(type: "int(11)", nullable: true),
                    course_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.id);
                    table.ForeignKey(
                        name: "subject_ibfk_1",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "reminder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    reminder_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    event_id = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reminder", x => x.id);
                    table.ForeignKey(
                        name: "reminder_ibfk_1",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    category = table.Column<int>(type: "int(11)", nullable: false),
                    parent_id = table.Column<int>(type: "int(11)", nullable: true),
                    subject_id = table.Column<int>(type: "int(11)", nullable: true),
                    user_id = table.Column<int>(type: "int(11)", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estimated_duration = table.Column<int>(type: "int(11)", nullable: true),
                    overall_duration = table.Column<int>(type: "int(11)", nullable: true),
                    deadline_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    score = table.Column<int>(type: "int(11)", nullable: true),
                    score_weight = table.Column<int>(type: "int(11)", nullable: true),
                    status = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_task_parent_id",
                        column: x => x.parent_id,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "task_ibfk_1",
                        column: x => x.subject_id,
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "task_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "record",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false),
                    category = table.Column<int>(type: "int(11)", nullable: false),
                    user_id = table.Column<int>(type: "int(11)", nullable: false),
                    task_id = table.Column<int>(type: "int(11)", nullable: true),
                    note = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    duration = table.Column<int>(type: "int(11)", nullable: true),
                    focus_duration = table.Column<int>(type: "int(11)", nullable: true),
                    break_duration = table.Column<int>(type: "int(11)", nullable: true),
                    break_frequency = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_record", x => x.id);
                    table.ForeignKey(
                        name: "record_ibfk_1",
                        column: x => x.task_id,
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "record_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "user_id",
                table: "course",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "user_id1",
                table: "event",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "task_id",
                table: "record",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "user_id2",
                table: "record",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "event_id",
                table: "reminder",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "course_id",
                table: "subject",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_parent_id",
                table: "task",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "subject_id",
                table: "task",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "user_id3",
                table: "task",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "record");

            migrationBuilder.DropTable(
                name: "reminder");

            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
