using System.Data.Entity.Migrations;

namespace CascadiaCollege.Migrations
{
    public partial class ComplexDataModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Student", "FirstMidName", "FirstName");
            CreateTable(
                    "dbo.Department",
                    c => new
                    {
                        DepartmentID = c.Int(false, true),
                        Name = c.String(maxLength: 50),
                        Budget = c.Decimal(false, storeType: "money"),
                        StartDate = c.DateTime(false),
                        InstructorID = c.Int()
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Instructor", t => t.InstructorID)
                .Index(t => t.InstructorID);

            CreateTable(
                    "dbo.Instructor",
                    c => new
                    {
                        ID = c.Int(false, true),
                        LastName = c.String(false, 50),
                        FirstName = c.String(false, 50),
                        HireDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.OfficeAssignment",
                    c => new
                    {
                        InstructorID = c.Int(false),
                        Location = c.String(maxLength: 50)
                    })
                .PrimaryKey(t => t.InstructorID)
                .ForeignKey("dbo.Instructor", t => t.InstructorID)
                .Index(t => t.InstructorID);

            CreateTable(
                    "dbo.CourseInstructor",
                    c => new
                    {
                        CourseID = c.Int(false),
                        InstructorID = c.Int(false)
                    })
                .PrimaryKey(t => new {t.CourseID, t.InstructorID})
                .ForeignKey("dbo.Course", t => t.CourseID, true)
                .ForeignKey("dbo.Instructor", t => t.InstructorID, true)
                .Index(t => t.CourseID)
                .Index(t => t.InstructorID);

            // Create  a department for course to point to.
            Sql("INSERT INTO dbo.Department (Name, Budget, StartDate) VALUES ('Temp', 0.00, GETDATE())");
            //  default value for FK points to department created above.
            AddColumn("dbo.Course", "DepartmentID", c => c.Int(false, defaultValue: 1));
            //AddColumn("dbo.Course", "DepartmentID", c => c.Int(nullable: false));

            AlterColumn("dbo.Course", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "LastName", c => c.String(false, 50));
            AlterColumn("dbo.Student", "FirstName", c => c.String(false, 50));
            CreateIndex("dbo.Course", "DepartmentID");
            AddForeignKey("dbo.Course", "DepartmentID", "dbo.Department", "DepartmentID", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course");
            DropForeignKey("dbo.Course", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.CourseInstructor", new[] {"InstructorID"});
            DropIndex("dbo.CourseInstructor", new[] {"CourseID"});
            DropIndex("dbo.OfficeAssignment", new[] {"InstructorID"});
            DropIndex("dbo.Department", new[] {"InstructorID"});
            DropIndex("dbo.Course", new[] {"DepartmentID"});
            AlterColumn("dbo.Student", "FirstName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Course", "Title", c => c.String());
            DropColumn("dbo.Course", "DepartmentID");
            DropTable("dbo.CourseInstructor");
            DropTable("dbo.OfficeAssignment");
            DropTable("dbo.Instructor");
            DropTable("dbo.Department");
            RenameColumn("dbo.Student", "FirstName", "FirstMidName");
        }
    }
}