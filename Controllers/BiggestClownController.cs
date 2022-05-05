using Microsoft.AspNetCore.Mvc;
using BiggestClown.Json;
using BiggestClown;

namespace BiggestClown.Controllers;

[ApiController]
[Route("[controller]")]
public class BiggsetClownController : ControllerBase
{

    private readonly ILogger<BiggsetClownController> _logger;

    public BiggsetClownController(ILogger<BiggsetClownController> logger)
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
