using SoulControl;
using SoulControl.Unit;
using SoulControl.NPC;
using SoulControl.Map;
using SoulControl.Utils;
using System;
using System.Net.Mime;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using Jint;
using ASPTest.ModelAPI;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

void SerializeToConsole<T>(object obj)
{
    XmlSerializer s = new XmlSerializer(typeof(T));
    if (File.Exists("model.xml"))
        File.Delete("model.xml");
    s.Serialize(new FileStream("model.xml", FileMode.OpenOrCreate), obj);
}

Options options = new Options();
options.AllowClr();
options.Interop.AllowGetType = true;
options.Interop.AllowSystemReflection = true;
options.Interop.AllowWrite = true;
options.Interop.AllowOperatorOverloading = true;
var eng = new Engine(options);

eng.SetValue("CreateTile", (Func<Tile>)(() => new Tile()));
eng.SetValue("CreateEntity", (Func<Entity>)(() => new Entity()));
eng.SetValue("CreateNPC", (Func<NPC>)(() => new NPC()));
eng.SetValue("CreatePlayer", (Func<Player>)(() => new Player()));

ModelGenerator generator = new ModelGenerator(Console.Out);

/*generator.Model.Player = new Player();
var pl = generator.Model.Player;
pl.Air = 3;
pl.TotalHP = 100;
pl.HP = 100;
pl.MoveMdoe = 0x1111;
pl.Moves = 15;
pl.Position = new System.Drawing.Point(1, 1);*/

generator.SetClearQueue(true, true);

eng.SetValue("log", (Action<string>)((string str) => {
    Console.Write(str);
}));
eng.SetValue("gen", generator);

eng.Execute(File.ReadAllText("genScript.js"));


ModelAPI.Model = generator.Model;

// добавляем в приложение сервисы Razor Pages
builder.Services.AddRazorPages(options =>
{
    // отключаем глобально Antiforgery-токен
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

var app = builder.Build();

// добавляем поддержку маршрутизации для Razor Pages
app.MapRazorPages();

app.Run();


















/*
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;

    PathString pathOuter = context.Request.Path;
    if (pathOuter.StartsWithSegments("/game")) 
    {
        pathOuter.StartsWithSegments("/game", out pathOuter);

        if (pathOuter.StartsWithSegments("/redirect"))
        {
            context.Response.Redirect("/game/time?help");
        }
        else if (pathOuter.StartsWithSegments("/time"))
        {
            pathOuter.StartsWithSegments("/time", out pathOuter);

            if (context.Request.Query.ContainsKey("help"))
            {
                context.Response.ContentType = "html";
                await context.Response.WriteAsync("<h1>/game/time HELP TABLE</h1>");
                await context.Response.WriteAsync("</br><sapn>Query arguments:</span>");
                await context.Response.WriteAsync("<table>");

                await context.Response.WriteAsync(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", "mode", "values: \"UTC\",\"SERVER\",\"SERVER-ticks\"</br>[Shows time depends on mode { defaultMode: \"UTC-ticks\";}]"));
                await context.Response.WriteAsync(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", "help", "values: NO\n</br>[Shows help(this) menu]"));

                await context.Response.WriteAsync("</table>");
            }
            else if (context.Request.Query.ContainsKey("mode"))
            {
                if (context.Request.Query["mode"] == "UTC")
                    await context.Response.WriteAsync(DateTime.UtcNow.ToString());
                else if (context.Request.Query["mode"] == "SERVER")
                    await context.Response.WriteAsync(DateTime.Now.ToString());
                else if (context.Request.Query["mode"] == "SERVER-ticks")
                    await context.Response.WriteAsync(DateTime.Now.Ticks.ToString());
                else
                    await context.Response.WriteAsync(DateTime.UtcNow.Ticks.ToString());
            }
            else
            {
                await context.Response.WriteAsync(DateTime.UtcNow.Ticks.ToString());
            }
        }
        else if(pathOuter.StartsWithSegments("/json"))
        {
            pathOuter.StartsWithSegments("/json", out pathOuter);

            if (context.Request.Query.ContainsKey("mode"))
            {
                response.Headers.ContentType = "application/json; charset=utf-8";
                await response.WriteAsync("{'name':'Tom', 'age':37}");
            }
            else
            {
                KeyValuePair<string, int> tom = new("Tom", 22);
                await context.Response.WriteAsJsonAsync(tom);
            }
        }
        else if(pathOuter.StartsWithSegments("/user"))
        {
            pathOuter.StartsWithSegments("/user", out pathOuter);

            var message = "Некорректные данные";   // содержание сообщения по умолчанию
            try
            {
                // пытаемся получить данные json
                var person = await request.ReadFromJsonAsync<Person>();
                if (person != null) // если данные сконвертированы в Person
                    message = $"Name: {person.name}  Age: {person.age}";
            }
            catch { }
            // отправляем пользователю данные
            await response.WriteAsJsonAsync(new { text = message });
        }
        else if(pathOuter.StartsWithSegments("/index.html"))
        {
            response.ContentType = "text/html; charset=utf-8";
            await response.SendFileAsync("html/index.html");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Game.Model RequestError:\nNotFound");
        }
    }
});
app.Run();

/*


    if (request.Path == "/api/user")
    {
        var message = "Некорректные данные";   // содержание сообщения по умолчанию
        try
        {
            // пытаемся получить данные json
            var person = await request.ReadFromJsonAsync<Person>();
            if (person != null) // если данные сконвертированы в Person
                message = $"Name: {person.Name}  Age: {person.Age}";
        }
        catch { }
        // отправляем пользователю данные
        await response.WriteAsJsonAsync(new { text = message });
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
 
*/

class Person
{
    public string name { get; set; }
    public int age { get; set; }

    public Person() { }

}