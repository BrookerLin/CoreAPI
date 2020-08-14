using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreAPI.Migrations
{
    public partial class Course_Add_DateModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Course",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(
                @"CREATE TRIGGER [dbo].[Course_UPDATE] ON [dbo].[Course]
                    AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                  
                    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;
    
                    DECLARE @CourseID INT
        
                    SELECT @CourseID = INSERTED.CourseID
                    FROM INSERTED
          
                    UPDATE dbo.Course
                    SET DateModified = GETDATE()
                    WHERE CourseID = @CourseID
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Course");

            migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Course_UPDATE]");
        }
    }
}
