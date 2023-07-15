# ExpressNET

- Wrapper of expressjs

Fast, Easy, No-Depency

```cs
using ExpressNET;
Express app = new Express();

app.get("/", (req, res) => {
    resp.send("Hello World");
});

app.listen(3000);

// Or Console.ReadLine(); instead
app.DoNotEndMyConsole();
```

## Installation

> `Install-Package ExpressNET`

## Thanks

Special thanks to [MattIPv4](https://github.com/MattIPv4) and [Matt Welke](https://github.com/mattwelke).
