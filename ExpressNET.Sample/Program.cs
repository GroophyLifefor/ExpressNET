using ExpressNET;

Express app = new Express();

app.use(Express.Static("public"));

app.get("/", (request, response) =>
{
    Console.WriteLine(request.RequestRaw);
    response.sendFile("/views/index.html");
});

app.listen(3000, port => Console.WriteLine($"Server running at http://localhost:{port}"));

app.DoNotEndMyConsole();