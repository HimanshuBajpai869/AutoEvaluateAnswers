// <copyright file="RouteConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EvaluateMVCApp
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Represents the Route Configurations.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the Routes for the Application.
        /// </summary>
        /// <param name="routes">Route Collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                namespaces: new[] { "EvaluateMVCApp" },
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Submission", action = "UploadSubmission", id = UrlParameter.Optional });
        }
    }
}
