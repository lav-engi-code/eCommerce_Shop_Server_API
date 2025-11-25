using eCommerce_Shop_Server_API.Modals;
using eCommerce_Shop_Server_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKernel();

var aiConfig = builder.Configuration.GetSection("OpenAI");
var apiKey = aiConfig["ApiKey"];
var model = aiConfig["Model"];

builder.Services.AddOpenAIChatCompletion(
    modelId: model!,
    apiKey: apiKey!
);

// Add services 

builder.Services.AddOpenApi();
builder.Services.AddSingleton<Fast_food_Services>();
builder.Services.AddSingleton<Address_Services>();
builder.Services.AddSingleton<Testimonial__Services>();
builder.Services.AddSingleton<Contact_Services>();
builder.Services.AddSingleton<Chat_Services>();
builder.Services.AddSingleton<Cart_Services>();
builder.Services.AddScoped<UserServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy.WithOrigins("https://localhost:7249")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


//Post Methods


app.MapPost("/add-food", ([FromBody] Product Eda, Fast_food_Services tsk) =>
{
    tsk.AddFoodMethod(Eda);
    return Results.Ok("Food added successfully");
});
app.MapPost("/address/add", (Address ad, Address_Services ads) =>
{
    return ads.AddAddressMethod(ad);
});
app.MapPost("/add-review", ([FromBody] Testimonial Eda, Testimonial__Services ads) =>
{
    ads.AddReviewMethod(Eda);
    return Results.Ok("Review added successfully");
});
app.MapPost("/add-contact", ([FromBody] Contact Eda, Contact_Services ads) =>
{
    ads.AddContactMethod(Eda);
    return Results.Ok("Contact added successfully");
});
app.MapPost("/chat-send", async ([FromBody] Chat_Message msg, Chat_Services chatService) =>
{
    var response = await chatService.SendMessage(msg);
    return Results.Ok(response);
});
app.MapPost("/cart/add", ([FromBody] Product Ed, Cart_Services cs) =>
{
    return cs.AddCartMethod(Ed);
});
app.MapPost("/register-user", async (RegisterModel model, UserServices us) =>
{
    var result = await us.RegisterUser(model);
    if (result == "ok")
        return Results.Ok("User registered successfully");
    return Results.BadRequest(result);
});
app.MapPost("/loginag", async (LoginModel model, UserServices service) =>
{
    var result = await service.LoginUser(model);
    return Results.Ok(result);
});


// Get Methods


app.MapGet("/chat-test", () =>
{
    return Results.Ok("Chat API is working!");
});
app.MapGet("/address/get", async (Address_Services ads) =>
{
    return await ads.GetAllAddressMethod();
});
app.MapGet("/get-food", (Fast_food_Services fs) =>
{
    return fs.GetFoodMethod();
});
app.MapGet("/food/{id}", (int id, Fast_food_Services configure) =>
{
    return configure.GetFoodByIdMethod(id);
});
app.MapGet("/get-review", (Testimonial__Services fs) =>
{
    return fs.GetReviewMethod();
});
app.MapGet("/get-contact", (Contact_Services fs) =>
{
    return fs.GetContactMethod();
});
app.MapGet("/cart/get/{id}", async (int id, Cart_Services cs) =>
{
    return await cs.GetCartByIdMethod(id);
});
app.MapGet("/cart/get", async (Cart_Services cs) =>
{
    var result = await cs.GetAllCartItems();
    return Results.Ok(result);
});


// Put Method


app.MapPut("/cart/update", ([FromBody] Product Ed, Cart_Services cs) =>
{
    return cs.UpdateCartMethod(Ed);
});
app.MapPut("/cart/update-qty/{id}/{qty}", (int id, int qty, Cart_Services cs) =>
{
    return cs.UpdateCartQtyById(id, qty);
});
app.MapPut("/update-review-by-name/{name}", ([FromRoute] string name,[FromBody] Testimonial Ed,Testimonial__Services rs) =>
{
    Ed.Reviewer_Name = name;
    return rs.UpdateReviewMethodByName(Ed);
});
app.MapPut("/update-food/{id}", (int id, Product updatedProduct, Fast_food_Services service) =>
{
    return service.UpdateFoodMethod(id, updatedProduct);
});



// Delete Method


app.MapDelete("/cart/remove/{id}", (int id, Cart_Services cs) =>
{
    return cs.DeleteCartMethod(id);
});
app.MapDelete("/delete-review-by-name/{name}", ([FromRoute] string name, Testimonial__Services rs) =>
{
    return rs.DeleteReviewByName(name);
});
app.MapDelete("/delete-food/{id}", (int id ,Fast_food_Services ffs) =>
{
    return ffs.DeleteFoodMethod(id);
});
app.MapDelete("/delete-contact/{phone}", (string phone, Contact_Services service) =>
{
    return service.DeleteContactByPhone(phone);
});



app.UseCors("AllowBlazor");
app.Run();