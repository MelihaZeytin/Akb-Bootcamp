using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace odev1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    public ProductController()
    {

    }
    

    private static List<Product> _products = new List<Product> {
        new Product {Id = 1, Name = "Product1", Price = 12.99m },
        new Product {Id = 2, Name = "Product2", Price = 20.49m}
    };

    // GET: api/products
    [HttpGet]
    [Route("list")]
    public IActionResult GetProducts()
    {
        return Ok(_products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var item = _products.FirstOrDefault(x => x.Id == id);
        if(item == null)
        {
            return NotFound();
        }
        return Ok(item);
            
    }


    // POST: api/products
    [HttpPost]
    [Route("AddNewProduct")]
    public IActionResult AddNewProduct([FromBody] Product request)
    {
        if(request == null || request.Price <= 0 || string.IsNullOrEmpty(request.Name))
        {
            return BadRequest("Invalid product data");
        }
        request.Id = _products.Count + 1;
        _products.Add(request);

        return CreatedAtAction(nameof(GetProductById), new { id = request.Id }, request);
        
    }

    [HttpPut("{id}")]
    public IActionResult EditProduct(int id, [FromBody] Product request)
    {
        var item = _products.FirstOrDefault(x => x.Id == id);

        if(string.IsNullOrEmpty(request.Name) || request.Price <= 0){
            return BadRequest("Invalid product data");
        }

        if(item != null)
        {
            item.Name = request.Name;
            item.Price = request.Price;
            return Ok(item);
        }

        return NotFound();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var item = _products.FirstOrDefault(x => x.Id == id);

        if(item != null)
        {
            _products.Remove(item);
            return NoContent();
        }
        return NotFound();
    }
// PATCH: api/products/{id}
    [HttpPatch("{id}")]
    public IActionResult PatchProduct(int id, [FromBody] JsonPatchDocument<Product> patchEntity)
    {
        var item = _products.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        patchEntity.ApplyTo(item, ModelState);
        return Ok(item);
    }

    [HttpPost]
    [Route("AddNewProductFromQuery")]
    public IActionResult AddNewProductFromQuery([FromQuery] int id, [FromQuery] string name, [FromQuery] decimal price)
    {
        var request = new Product { Id = id, Name = name, Price = price };

        if (request == null || request.Price <= 0 || string.IsNullOrEmpty(request.Name))
        {
            return BadRequest("Invalid product data");
        }

        request.Id = _products.Count + 1;
        _products.Add(request);

        return CreatedAtAction(nameof(GetProductById), new { id = request.Id }, request);
    }

    [HttpGet("listByName")]
    public IActionResult GetProductsByName([FromQuery] string name)
    {
        IEnumerable<Product> result = _products;

        if (!string.IsNullOrEmpty(name))
        {
            result = result.Where(x => x.Name != null && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(result.ToList());
    }

    [HttpGet("listByPrice")]
    public IActionResult GetProductsByPrice([FromQuery] decimal price)
    {
        IEnumerable<Product> result = _products;

        if (price != 0)
        {
            result = result.Where(x => x.Price != 0 && x.Price.Equals(price));
        }

        return Ok(result.ToList());
    }

    [HttpGet]
    [Route("sortByName")]
    public IActionResult GetProducts([FromQuery] string sortOrder)
    {
        IEnumerable<Product> result = _products;

        if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() == "asc")
        {
            result = result.OrderBy(x => x.Price);
        }

        
        else if (!string.IsNullOrEmpty(sortOrder) && sortOrder.ToLower() == "desc")
        {
            result = result.OrderByDescending(x => x.Price);
        }

        return Ok(result.ToList());
    }

    



}

    

