using System.Data.Entity.Migrations;
using System.Diagnostics;

namespace CascadiaCollege.Migrations
{
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            // Drop foreign keys and indexes that point to tables we're going to drop.
            Debug.Print("Start");
            DropForeignKey("dbo.Enrollment", "StudentID", "dbo.Student");
            DropIndex("dbo.Enrollment", new[] {"StudentID"});

            RenameTable("dbo.Instructor", "Person");
            AddColumn("dbo.Person", "EnrollmentDate", c => c.DateTime());
            AddColumn("dbo.Person", "Discriminator", c => c.String(false, 128, defaultValue: "Instructor"));
            AlterColumn("dbo.Person", "HireDate", c => c.DateTime());
            AddColumn("dbo.Person", "OldId", c => c.Int(true));

            // Copy existing Student data into new Person table.
            Sql(
                "INSERT INTO dbo.Person (LastName, FirstName, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, null AS HireDate, EnrollmentDate, 'Student' AS Discriminator, ID AS OldId FROM dbo.Student");

            // Fix up existing relationships to match new PK's.
            Sql(
                "UPDATE dbo.Enrollment SET StudentId = (SELECT ID FROM dbo.Person WHERE OldId = Enrollment.StudentId AND Discriminator = 'Student')");

            // Remove temporary key
            DropColumn("dbo.Person", "OldId");

            DropTable("dbo.Student");

            // Re-create foreign keys and indexes pointing to new table.
            AddForeignKey("dbo.Enrollment", "StudentID", "dbo.Person", "ID", true);
            CreateIndex("dbo.Enrollment", "StudentID");
            Debug.Print("end");
        }

        public override void Down()
        {
            CreateTable(
                    "dbo.Student",
                    c => new
                    {
                        ID = c.Int(false, true),
                        LastName = c.String(false, 50),
                        FirstName = c.String(false, 50),
                        EnrollmentDate = c.DateTime(false)
                    })
                .PrimaryKey(t => t.ID);

            AlterColumn("dbo.Person", "HireDate", c => c.DateTime(false));
            DropColumn("dbo.Person", "Discriminator");
            DropColumn("dbo.Person", "EnrollmentDate");
            RenameTable("dbo.Person", "Instructor");
        }
    }
}