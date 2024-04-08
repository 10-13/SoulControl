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

// äîáàâëÿåì â ïðèëîæåíèå ñåðâèñû Razor Pages
builder.Services.AddRazorPages(options =>
{
    // îòêëþ÷àåì ãëîáàëüíî Antiforgery-òîêåí
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

var app = builder.Build();

// äîáàâëÿåì ïîääåðæêó ìàðøðóòèçàöèè äëÿ Razor Pages
app.MapRazorPages();

app.Run();







class Person
{
    public string name { get; set; }
    public int age { get; set; }

    public Person() { }

}
