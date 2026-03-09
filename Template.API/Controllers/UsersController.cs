using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Application.DTOs.User;
using Template.Application.Commands.Users;
using Template.Application.Queries.Users;

namespace Template.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        if (result is null)
            return NotFound();

        return Ok(result);
    }
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _mediator.Send(new GetUserByEmailQuery(email));
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserDTO dto)
    {
        var id = await _mediator.Send(new CreateUserCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserDTO dto)
    {
        await _mediator.Send(new UpdateUserCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}