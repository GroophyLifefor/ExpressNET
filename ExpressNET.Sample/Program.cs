using ExpressNET;

Express app = new Express();
app.port = 3000;

app.staticDir("public");

app.route("/", (req, res) =>
{
    res.sendFile("views/index.html");
});

app.listen(port => Console.WriteLine($"Server was running at http://localhost:{port}"));