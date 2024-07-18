using HashDiff.Controllers;
using HashDiff.Extensions;
using HashDiff.Models;
using HashDiff.Repositories;

namespace HashDiff.Services;

// This service acts as a shield between HTTP world of controller and business logic oblivious to its surroundings.
// It structures business logic calls and prepares response for a controller.

/// <inheritdoc cref="IDiffService"/>
public class DiffService : IDiffService
{
    private readonly ILogger<DiffService> logger;
    private readonly IDiffRepository diffRepository;
    private readonly IDiffComparer stringDiffService;

    public DiffService(ILogger<DiffService> logger, IDiffRepository diffRepository, IDiffComparer stringDiffService)
    {
        this.logger = logger;
        this.diffRepository = diffRepository;
        this.stringDiffService = stringDiffService;
    }

    public async Task<DiffServiceResult> SaveLeftPartAsync(int id, string data)
    {
        if (!data.IsValidBase64String())
            return new DiffServiceResult
            {
                ResultState = ResultState.BadRequest, 
                Message = "Provided string is not valid Base64"
            };

        var contents = DiffPostRequest.CreateFromBase64(data);

        DiffServiceResult result = new DiffServiceResult();
        try
        {
            var createdEntity = await diffRepository.SaveLeftPartAsync(id, contents.Input);
            result.ResultState = ResultState.Created;
            result.ObjectToReturn = createdEntity;
        }
        catch (InvalidDataException e)
        {
            logger.LogError(e, $"Wrong data provided");
            result.ResultState = ResultState.BadRequest;
        }
        // Other types of unexpected exceptions result in return of HTTP code 500 automatically, so there are not considered here.
        // Stack traces are hidden from the user because app is run in production mode. They are visible in the logs.
        // Logging is done to the console from there it can be redirected where needed.

        return result;
    }

    public async Task<DiffServiceResult> GetLeftPartAsync(int id)
    {
        var diff = await diffRepository.GetDiffAsync(id);
        if(diff != null)
            return new DiffServiceResult { ResultState = ResultState.Ok, ObjectToReturn = new Diff { Id = diff.Id, LeftPart = diff.LeftPart }};
            // Because this method exists to retrieve created left part of the diff, I'm not including the right part
            // even if it might exist

        return new DiffServiceResult { ResultState = ResultState.NotFound, Message = $"Left part with Id {id} was not found"};
    }

    public async Task<DiffServiceResult> SaveRightPartAsync(int id, string data)
    {
        if (!data.IsValidBase64String())
            return new DiffServiceResult
            {
                ResultState = ResultState.BadRequest, 
                Message = "Provided string is not valid Base64"
            };

        var contents = DiffPostRequest.CreateFromBase64(data);

        DiffServiceResult result = new DiffServiceResult();
        try
        {
            var createdEntity = await diffRepository.SaveRightPartAsync(id, contents.Input);
            result.ResultState = ResultState.Created;
            result.ObjectToReturn = createdEntity;
        }
        catch (InvalidDataException e)
        {
            logger.LogError(e, "Wrong data provided");
            result.ResultState = ResultState.BadRequest;
        }

        return result;
    }
    
    public async Task<DiffServiceResult> GetRightPartAsync(int id)
    {
        var diff = await diffRepository.GetDiffAsync(id);
        if(diff != null)
            return new DiffServiceResult { ResultState = ResultState.Ok,  ObjectToReturn = new Diff { Id = diff.Id, RightPart = diff.RightPart }};
            // Because this method exists to retrieve created right part of the diff, I'm not including the left part
            // even if it might exist

            return new DiffServiceResult { ResultState = ResultState.NotFound, Message = $"Right part with Id {id} was not found"};
    }

    public async Task<DiffServiceResult> GetDiff(int id)
    {
        var diff = await diffRepository.GetDiffAsync(id);
        if (diff == null)
            return new DiffServiceResult { ResultState = ResultState.NotFound, Message = $"Diff with id {id} was not found"};
        
        if (string.IsNullOrEmpty(diff.LeftPart) || string.IsNullOrEmpty(diff.RightPart))
            return new DiffServiceResult
            {
                ResultState = ResultState.BadRequest, 
                Message = "One of the parts has not been provided"
            };

        var result = stringDiffService.CompareParts(diff.LeftPart, diff.RightPart);

        if (result.Diffs != null)
            return new DiffServiceResult { ResultState = ResultState.Ok,  ObjectToReturn = result.Diffs };
        
        return new DiffServiceResult { ResultState = ResultState.Ok,  ObjectToReturn = result.Result };
    }
}