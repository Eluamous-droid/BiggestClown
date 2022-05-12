using Microsoft.AspNetCore.Mvc;
using BiggestClown.Json;

namespace BiggestClown.Controllers;

[ApiController]
[Route("[controller]")]
public class BiggestClownController : ControllerBase
{

    private readonly ILogger<BiggestClownController> _logger;

    public BiggestClownController(ILogger<BiggestClownController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBiggestClownSorted")]
    public IEnumerable<Player> Get()
    {   
        List<Player> players = ClownGenerator.getSortedListOfClowns();
        return players;

    }

}
