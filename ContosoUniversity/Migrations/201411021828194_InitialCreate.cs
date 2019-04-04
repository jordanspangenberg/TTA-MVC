using System.Data.Entity.Migrations;

namespace CascadiaCollege.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Course",
                    c => new
                    {
                        CourseID = c.Int(false),
                        Title = c.String(),
                        Credits = c.Int(false)
                    })
                .PrimaryKey(t => t.CourseID);

            CreateTable(
                    "dbo.Enrollment",
                    c => new
                    {
                        EnrollmentID = c.Int(false, true),
                        CourseID = c.Int(false),
                        StudentID = c.Int(false),
                        Grade = c.Int()
                    })
                .PrimaryKey(t => t.EnrollmentID)
                .ForeignKey("dbo.Course", t => t.CourseID, true)
                .ForeignKey("dbo.Student", t => t.StudentID, true)
                .Index(t => t.CourseID)
                .Index(t => t.StudentID);

            CreateTable(
                    "dbo.Student",
                    c => new
                    {
                        ID = c.Int(false, true),
                        LastName = c.String(),
                        FirstMidName = c.String(),
                        EnrollmentDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Enrollment", "StudentID", "dbo.Student");
            DropForeignKey("dbo.Enrollment", "CourseID", "dbo.Course");
            DropIndex("dbo.Enrollment", new[] {"StudentID"});
            DropIndex("dbo.Enrollment", new[] {"CourseID"});
            DropTable("dbo.Student");
            DropTable("dbo.Enrollment");
            DropTable("dbo.Course");
        }
    }
}