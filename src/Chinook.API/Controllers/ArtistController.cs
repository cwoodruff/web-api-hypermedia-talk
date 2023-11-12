﻿using System.Net;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Supervisor;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
public class ArtistController : ControllerBase
{
    private readonly IChinookSupervisor _chinookSupervisor;
    private readonly ILogger<ArtistController> _logger;

    public ArtistController(IChinookSupervisor chinookSupervisor, ILogger<ArtistController> logger)
    {
        _chinookSupervisor = chinookSupervisor;
        _logger = logger;
    }

    [HttpGet]
    [Produces("application/json")]
    public ActionResult<List<ArtistApiModel>> Get()
    {
        try
        {
            var artists = _chinookSupervisor.GetAllArtist();

            if (artists.Any())
            {
                return Ok(artists);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No Artists Could Be Found");
            }
        }
        catch (Exception ex)  
        {  
            _logger.LogError($"Something went wrong inside the ArtistController Get action: {ex}");  
            return StatusCode((int)HttpStatusCode.InternalServerError, "Error occurred while executing Get All Artists");  
        }  
    }

    [HttpGet("{id}", Name = "GetArtistById")]
    [Produces("application/json")]
    public ActionResult<ArtistApiModel> Get(int id)
    {
        try
        {
            var artist = _chinookSupervisor.GetArtistById(id);

            if (artist != null)
            {
                return Ok(artist);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Artist Not Found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController GetById action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get Artist By Id");
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<ArtistApiModel> Post([FromBody] ArtistApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Artist is null");
            }
            else
            {
                return Ok(_chinookSupervisor.AddArtist(input));
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController Add Artist action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add Artists");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController Add Artist action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add Artists");
        }
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<ArtistApiModel> Put(int id, [FromBody] ArtistApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Artist is null");
            }
            else
            {
                return Ok(_chinookSupervisor.UpdateArtist(input));
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController Update Artist action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update Artists");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController Update Artist action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update Artists");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            return Ok(_chinookSupervisor.DeleteArtist(id));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the ArtistController Delete action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Delete Artist");
        }
    }
}