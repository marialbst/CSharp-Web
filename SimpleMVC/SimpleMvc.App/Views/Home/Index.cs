﻿namespace SimpleMvc.App.Views.Home
{
    using Framework.Contracts;

    public class Index : IRenderable
    {
        public string Render()
        {
            return "<h1>Test</h1>";
        }
    }
}
