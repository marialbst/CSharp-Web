namespace SimpleMvc.App
{
    using Framework;
    using Framework.Routers;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Data;
    using WebServer;

    public class Launcher
    {
        static Launcher()
        {
            using (var db = new NotesDbContext())
            {
                db.Database.Migrate();
            }
        }

        public static void Main()
        {
            MvcEngine.Run(new WebServer(4333, new ControllerRouter()));
        }
    }
}
