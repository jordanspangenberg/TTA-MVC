using System.Data.Entity.Migrations;

namespace CascadiaCollege.Migrations
{
    public partial class DepartmentSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
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
                      
                      SELECT t0.[DepartmentID]
                      FROM [dbo].[Department] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartmentID] = @DepartmentID"
            );

            CreateStoredProcedure(
                "dbo.Department_Update",
                p => new
                {
                    DepartmentID = p.Int(),
                    Name = p.String(50),
                    Budget = p.Decimal(19, 4, storeType: "money"),
                    StartDate = p.DateTime(),
                    InstructorID = p.Int()
                },
                @"UPDATE [dbo].[Department]
                      SET [Name] = @Name, [Budget] = @Budget, [StartDate] = @StartDate, [InstructorID] = @InstructorID
                      WHERE ([DepartmentID] = @DepartmentID)"
            );

            CreateStoredProcedure(
                "dbo.Department_Delete",
                p => new
                {
                    DepartmentID = p.Int()
                },
                @"DELETE [dbo].[Department]
                      WHERE ([DepartmentID] = @DepartmentID)"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.Department_Delete");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Insert");
        }
    }
}