﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Service.PartnerManagement.Client.Api;
using MAVN.Service.PartnerManagement.Client.Models.Location;
using MAVN.Service.PartnerManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MAVN.Service.PartnerManagement.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationsController : ControllerBase, ILocationsApi
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public LocationsController(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets location by external identifier.
        /// </summary>
        /// <param name="externalId">string</param>
        /// <returns><see cref="LocationInfoResponse"/></returns>
        /// <response code="200">An base location information for given filter</response>
        [HttpGet("byexternalid")]
        public async Task<LocationInfoResponse> GetByExternalId2Async(string externalId)
        {
            var result = await _locationService.GetByExternalIdAsync(externalId);

            return _mapper.Map<LocationInfoResponse>(result);
        }

        /// <summary>
        /// Gets location by id.
        /// </summary>
        /// <param name="id">string</param>
        /// <returns><see cref="LocationInfoResponse"/></returns>
        /// <response code="200">An base location information for given filter</response>
        [HttpGet("byid")]
        public async Task<LocationInfoResponse> GetByIdAsync(Guid id)
        {
            var result = await _locationService.GetByIdAsync(id);

            return _mapper.Map<LocationInfoResponse>(result);
        }

        /// <summary>
        /// Gets country ISO3 code for all locations
        /// </summary>
        /// <returns><see cref="GetCountryIso3CodeForAllLocationsResponse"/></returns>
        /// <response code="200">List of iso3 codes</response>
        [HttpGet("countrycodes")]
        public async Task<GetCountryIso3CodeForAllLocationsResponse> GetCountryIso3CodeForAllLocations()
        {
            var result = await _locationService.GetIso3CodesForLocations();

            return new GetCountryIso3CodeForAllLocationsResponse
            {
                CountriesIso3Codes = result
            };
        }
    }
}
