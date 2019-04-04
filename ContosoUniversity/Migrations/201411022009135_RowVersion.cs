using System;
using System.Data.Entity.Migrations;

namespace CascadiaCollege.Migrations
{
    public partial class RowVersion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Department", "RowVersion",
                c => c.Binary(false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterStoredProcedure(
                "dbo.Department_Insert",
                p => new
                {
                    Name = p.String(50),
                    Budget = p.Decimal(19, 4, storeType: "money"),
                    StartDate = p.DateTime(),
                    InstructorID = p.Int()
                },
                @"INSERT [dbo].[Department]([Name], [Budget], [StartDate], [InstructorID])
                      VALUES (@Name, @Budget, @StartDate, @InstructorID)
                      
                      DECLARE @DepartmentID int
                      SELECT @DepartmentID = [DepartmentID]
                      FROM [dbo].[Department]
                      WHERE @@ROWCOUNT > 0 AND [DepartmentID] = scope_identity()
                      
                      SELECT t0.[DepartmentID], t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );

            AlterStoredProcedure(
                "dbo.Department_Update",
                p => new
                {
                    DepartmentID = p.Int(),
                    Name = p.String(50),
                    Budget = p.Decimal(19, 4, storeType: "money"),
                    StartDate = p.DateTime(),
                    InstructorID = p.Int(),
                    RowVersion_Original = p.Binary(8, true, storeType: "rowversion")
                },
                @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [InstructorID] = @InstructorID
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );

            AlterStoredProcedure(
                "dbo.Department_Delete",
                p => new
                {
                    DepartmentID = p.Int(),
                    RowVersion_Original = p.Binary(8, true, storeType: "rowversion")
                },
                @"DELETE [dbo].[Department]
                      WHERE (([DepartmentID] = @DepartmentID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
        }

        public override void Down()
        {
            DropColumn("dbo.Department", "RowVersion");
            throw new NotSupportedException(
                "Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}