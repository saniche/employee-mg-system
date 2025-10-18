using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using EmployeeManagementAPI.Dtos;

namespace EmployeeManagementAPI.IntegrationTests;

[Collection("IntegrationTests")]
public class EmployeeControllerTests : BaseIntegrationTest
{
    public EmployeeControllerTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ReturnsSeededEmployees()
    {
        var client = CreateClient();
        var res = await client.GetAsync("/api/employee");
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await res.Content.ReadFromJsonAsync<EmployeeDto[]>();
        items.Should().NotBeNull();
        items!.Select(e => e.Email).Should().Contain(new[] { "john@example.com", "jane@example.com" });
    }

    [Fact]
    public async Task GetById_ReturnsEmployee()
    {
        var client = CreateClient();
        // first get all to obtain an id
        var all = await client.GetFromJsonAsync<EmployeeDto[]>("/api/employee");
        all.Should().NotBeNull();
        var id = all!.First().Id;

        var res = await client.GetAsync($"/api/employee/{id}");
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await res.Content.ReadFromJsonAsync<EmployeeDto>();
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetById_NotFound_Returns404()
    {
        var client = CreateClient();
        var res = await client.GetAsync($"/api/employee/{int.MaxValue}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPaged_ReturnsSubset()
    {
        var client = CreateClient();
        var res = await client.GetAsync("/api/employee/paged?pageNumber=1&pageSize=1");
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await res.Content.ReadFromJsonAsync<EmployeeDto[]>();
        items.Should().NotBeNull();
        items!.Length.Should().Be(1);
    }

    [Fact]
    public async Task Post_CreateAndDeleteEmployee()
    {
        var client = CreateClient();
        var create = new EmployeeCreateDto { FirstName = "New", LastName = "Person", Email = "new@example.com", PhoneNumber = "000", Position = "Intern", Salary = 1000 };
        var post = await client.PostAsJsonAsync("/api/employee", create);
        post.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await post.Content.ReadFromJsonAsync<EmployeeDto>();
        created.Should().NotBeNull();
        created!.Email.Should().Be("new@example.com");

        // cleanup
        var del = await client.DeleteAsync($"/api/employee/{created.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Post_Invalid_ReturnsBadRequest()
    {
        var client = CreateClient();
        // Missing required fields like Email and Name
        var invalid = new EmployeeCreateDto { FirstName = "", LastName = "", Email = "", PhoneNumber = "", Position = "", Salary = -1 };
        var res = await client.PostAsJsonAsync("/api/employee", invalid);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_UpdateEmployee()
    {
        var client = CreateClient();
        var all = await client.GetFromJsonAsync<EmployeeDto[]>("/api/employee");
        all.Should().NotBeNull();
        var first = all!.First();

        var update = new EmployeeUpdateDto { Id = first.Id, FirstName = first.FirstName, LastName = first.LastName, Email = first.Email, PhoneNumber = first.PhoneNumber, Position = "Updated", Salary = first.Salary };
        var put = await client.PutAsJsonAsync($"/api/employee/{first.Id}", update);
        put.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await put.Content.ReadFromJsonAsync<EmployeeDto>();
        updated.Should().NotBeNull();
        updated!.Position.Should().Be("Updated");
    }

    [Fact]
    public async Task Put_IdMismatch_ReturnsBadRequest()
    {
        var client = CreateClient();
        var all = await client.GetFromJsonAsync<EmployeeDto[]>("/api/employee");
        all.Should().NotBeNull();
        var first = all!.First();

        // Id in body doesn't match route id
        var update = new EmployeeUpdateDto { Id = first.Id + 1, FirstName = first.FirstName, LastName = first.LastName, Email = first.Email, PhoneNumber = first.PhoneNumber, Position = "Nope", Salary = first.Salary };
        var put = await client.PutAsJsonAsync($"/api/employee/{first.Id}", update);
        put.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_NotFound_Returns404()
    {
        var client = CreateClient();
        var update = new EmployeeUpdateDto { Id = int.MaxValue, FirstName = "X", LastName = "Y", Email = "x@y.com", PhoneNumber = "000", Position = "None", Salary = 0 };
        var put = await client.PutAsJsonAsync($"/api/employee/{int.MaxValue}", update);
        put.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotFound_Returns404()
    {
        var client = CreateClient();
        var del = await client.DeleteAsync($"/api/employee/{int.MaxValue}");
        del.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
