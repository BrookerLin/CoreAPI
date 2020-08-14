using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreAPI.Migrations
{
    public partial class Others_Add_DateModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Person",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Department",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(
                @"CREATE TRIGGER [dbo].[Tr_Department_UPDATE] ON [dbo].[Department]
                    AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                  
                    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;
    
                    DECLARE @DepartmentID INT
        
                    SELECT @DepartmentID = INSERTED.DepartmentID
                    FROM INSERTED
          
                    UPDATE dbo.Department
                    SET DateModified = GETDATE()
                    WHERE DepartmentID = @DepartmentID
                END");

            migrationBuilder.Sql(
                @"CREATE TRIGGER [dbo].[Person_UPDATE] ON [dbo].[Person]
                    AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                  
                    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;
    
                    DECLARE @ID INT
        
                    SELECT @ID = INSERTED.ID
                    FROM INSERTED
          
                    UPDATE dbo.Person
                    SET DateModified = GETDATE()
                    WHERE ID = @ID
                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Department");

            migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Tr_Department_UPDATE]");
            migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Person_UPDATE]");
        }
    }
}
