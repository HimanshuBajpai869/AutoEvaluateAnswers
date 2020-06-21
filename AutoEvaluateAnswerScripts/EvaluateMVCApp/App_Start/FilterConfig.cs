// <copyright file="FilterConfig.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EvaluateMVCApp
{
    using System.Web.Mvc;

    /// <summary>
    /// Represents the Filter Configuration.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the Global Filters.
        /// </summary>
        /// <param name="filters">Global Filter Collections.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
