﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.PartnerManagement.Client.Api;
using Lykke.Service.PartnerManagement.Client.Enums;
using Lykke.Service.PartnerManagement.Client.Models.Partner;
using Lykke.Service.PartnerManagement.Domain.Exceptions;
using Lykke.Service.PartnerManagement.Domain.Models;
using Lykke.Service.PartnerManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.PartnerManagement.Controllers
{
    [Route("api/partners")]
    [ApiController]
    public class PartnersController : Controller, IPartnersApi
    {
        private readonly IPartnerService _partnerService;
        private readonly IMapper _mapper;
        private readonly ILog _log;

        public PartnersController(
            IPartnerService partnerService,
            IMapper mapper,
            ILogFactory logFactory)
        {
            _partnerService = partnerService;
            _mapper = mapper;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Gets all partners paginated.
        /// </summary>
        /// <param name="request">The paginated list request parameters.</param>
        /// <response code="200">A collection of partners.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PartnerListResponseModel), (int) HttpStatusCode.OK)]
        public async Task<PartnerListResponseModel> GetAsync([FromQuery] PartnerListRequestModel request)
        {
            var result = await _partnerService.GetAsync(
                request.CurrentPage,
                request.PageSize,
                request.Name,
                _mapper.Map<Vertical?>(request.Vertical));

            return new PartnerListResponseModel
            {
                CurrentPage = request.CurrentPage,
                TotalSize = result.totalSize,
                PartnersDetails = _mapper.Map<IReadOnlyCollection<PartnerListDetailsModel>>(result.partners)
            };
        }

        /// <summary>
        /// Gets partner by identifier.
        /// </summary>
        /// <param name="id">The partner identifier.</param>
        /// <response code="200">A detailed partner information.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PartnerDetailsModel), (int) HttpStatusCode.OK)]
        public async Task<PartnerDetailsModel> GetByIdAsync(Guid id)
        {
            var result = await _partnerService.GetByIdAsync(id);

            return _mapper.Map<PartnerDetailsModel>(result);
        }

        /// <summary>
        /// Gets partner by client identifier.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <response code="200">A detailed partner information.</response>
        [HttpGet("byClientId/{clientId}")]
        [ProducesResponseType(typeof(PartnerDetailsModel), (int) HttpStatusCode.OK)]
        public async Task<PartnerDetailsModel> GetByClientIdAsync(string clientId)
        {
            var result = await _partnerService.GetByClientIdAsync(clientId);

            return _mapper.Map<PartnerDetailsModel>(result);
        }

        /// <summary>
        /// Gets partner by location identifier.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <response code="200">A detailed partner information.</response>
        [HttpGet("byLocationId/{locationId}")]
        [ProducesResponseType(typeof(PartnerDetailsModel), (int) HttpStatusCode.OK)]
        public async Task<PartnerDetailsModel> GetByLocationIdAsync(Guid locationId)
        {
            var result = await _partnerService.GetByLocationIdAsync(locationId);

            return _mapper.Map<PartnerDetailsModel>(result);
        }

        /// <summary>
        /// Creates partner.
        /// </summary>
        /// <param name="partnerCreateModel">The partner creation details.</param>
        /// <response code="200">The result of partner creation.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PartnerCreateResponse), (int) HttpStatusCode.OK)]
        public async Task<PartnerCreateResponse> CreateAsync(PartnerCreateModel partnerCreateModel)
        {
            Guid id;
            try
            {
                id = await _partnerService.CreateAsync(_mapper.Map<Partner>(partnerCreateModel));
            }
            catch (ClientAlreadyExistException e)
            {
                _log.Info(e.Message, partnerCreateModel);

                return new PartnerCreateResponse
                {
                    ErrorCode = PartnerManagementError.AlreadyRegistered, ErrorMessage = e.Message
                };
            }
            catch (PartnerRegistrationFailedException e)
            {
                _log.Info(e.Message, partnerCreateModel);

                return new PartnerCreateResponse
                {
                    ErrorCode = PartnerManagementError.RegistrationFailed, ErrorMessage = e.Message
                };
            }
            catch (LocationContactRegistrationFailedException e)
            {
                _log.Info(e.Message, partnerCreateModel);

                return new PartnerCreateResponse
                {
                    ErrorCode = PartnerManagementError.RegistrationFailed, ErrorMessage = e.Message
                };
            }
            catch (LocationExternalIdNotUniqueException e)
            {
                _log.Info(e.Message, partnerCreateModel);

                return new PartnerCreateResponse
                {
                    ErrorCode = PartnerManagementError.LocationExternalIdNotUnique, ErrorMessage = e.Message
                };
            }

            return new PartnerCreateResponse {Id = id, ErrorCode = PartnerManagementError.None};
        }

        /// <summary>
        /// Updates partner.
        /// </summary>
        /// <param name="partnerUpdateModel">The partner update details.</param>
        /// <response code="200">The result of partner update.</response>
        [HttpPut]
        [ProducesResponseType(typeof(PartnerUpdateResponse), (int) HttpStatusCode.OK)]
        public async Task<PartnerUpdateResponse> UpdateAsync([FromBody] PartnerUpdateModel partnerUpdateModel)
        {
            try
            {
                await _partnerService.UpdateAsync(_mapper.Map<Partner>(partnerUpdateModel));
            }
            catch (PartnerNotFoundFailedException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.PartnerNotFound, ErrorMessage = e.Message
                };
            }
            catch (ClientAlreadyExistException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.AlreadyRegistered, ErrorMessage = e.Message
                };
            }
            catch (PartnerRegistrationFailedException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.RegistrationFailed, ErrorMessage = e.Message
                };
            }
            catch (LocationContactUpdateFailedException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.RegistrationFailed, ErrorMessage = e.Message
                };
            }
            catch (LocationContactRegistrationFailedException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.RegistrationFailed, ErrorMessage = e.Message
                };
            }
            catch (LocationExternalIdNotUniqueException e)
            {
                _log.Info(e.Message, partnerUpdateModel);

                return new PartnerUpdateResponse
                {
                    ErrorCode = PartnerManagementError.LocationExternalIdNotUnique, ErrorMessage = e.Message
                };
            }

            return new PartnerUpdateResponse();
        }

        /// <summary>
        /// Deletes partner by identifier.
        /// </summary>
        /// <param name="id">The partner identifier.</param>
        /// <response code="204">The partner successfully deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        public async Task DeleteAsync(Guid id)
        {
            await _partnerService.DeleteAsync(id);
        }

        /// <summary>
        /// Gets partners by collect of identifier.
        /// </summary>
        /// <param name="ids">The partners identifiers.</param>
        /// <response code="200">A collection of partners</response>
        [HttpPost("list")]
        [ProducesResponseType(typeof(IReadOnlyCollection<PartnerListDetailsModel>), (int)HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<PartnerListDetailsModel>> GetByIdsAsync([FromBody] Guid[] ids)
        {
            var result = await _partnerService.GetByIdsAsync(ids);

            return _mapper.Map<IReadOnlyCollection<PartnerListDetailsModel>>(result);
        }

    }
}
