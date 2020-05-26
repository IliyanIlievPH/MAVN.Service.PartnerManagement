﻿namespace MAVN.Service.PartnerManagement.Client.Models.Partner
{
    /// <summary>
    /// Request model
    /// </summary>
    public class GetNearPartnersByCoordinatesRequest
    {
        /// <summary>
        /// The latitude you want to search around
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// The longitude you want to search around
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// The radius you want to search in
        /// </summary>
        public double? RadiusInKm { get; set; }

        /// <summary>
        /// Iso 3 code of the country
        /// </summary>
        public string CountryIso3Code { get; set; }
    }
}
