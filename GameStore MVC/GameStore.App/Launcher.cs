namespace GameStore.App
{
    using GameStore.Data;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public class Launcher
    {
        static Launcher()
        {
            using (var db = new GameStoreMvcDbContext())
            {
                db.Database.Migrate();
            }
        }

        static void Main(string[] args)
            => MvcEngine.Run(new WebServer(1333, new ControllerRouter(), new ResourceRouter()));
    }
}
