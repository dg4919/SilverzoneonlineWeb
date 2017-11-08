using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApsMind.Olympiad.Framework;
using ApsMind.Olympiad.Framework.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ApsMind.Olympiad.Framework
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DefaultConnection")
       {
            Database.SetInitializer<DatabaseContext>(null);
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
        public DbSet<QuizDetail> QuizDetails { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Para> Paras { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<AnswerSheet> AnswerSheets { get; set; }
        public DbSet<AnswerDetail> AnswerDetails { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<QuizPaperMapping> QuizPaperMappings { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<InfoZone> InfoZones { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Note> Notes { set; get; }
        public DbSet<QuizPurchaseOrder> QuizPurchaseOrders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Chapter> Chapter { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<StudentClass> StudentClass { get; set; }
        public DbSet<PaperType> PaperType { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<PackageDetails> PackageDetails { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

}

